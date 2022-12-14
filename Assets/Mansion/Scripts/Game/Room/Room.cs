using HFPS.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room : MonoBehaviour
{
    [SerializeField] string RoomID;
    [SerializeField] Transform[] doors;
    [SerializeField] Light[] lights;
    Dictionary<Light, float> lightsOriginalIntensities = new Dictionary<Light, float>();
    [SerializeField] List<ItemPlaceHolder> placeHolders = new List<ItemPlaceHolder>();


    private void Start()
    {
        foreach(Light l in lights)
        {
            lightsOriginalIntensities.Add(l, l.intensity);
        }
    }

    #region RoomEvents

    public string GetRoomID() => RoomID;


    public void TurnLights(bool enable, bool blink = false)
    {
        foreach(Light l in lights)
        {
            l.intensity = enable ? lightsOriginalIntensities[l] : 0;
        }
    }

    public void CloseDoors()
    {
        foreach (Transform d in doors)
        {
            d.transform.rotation = Quaternion.Euler(d.transform.rotation.eulerAngles.x, 0, d.transform.rotation.eulerAngles.z);
        }
    }

    public void PlaceItem(CustomDraggableObject item)
    {
        ItemPlaceHolder holder = GetRandomPlaceHolder();
        if(holder != null)
        {
            item.transform.position = holder.Holder.position;
            item.transform.rotation = Quaternion.Euler(holder.Rotation);
            holder.empty = false;

            holder.OnItemRemoved = () => { holder.empty = true; item.OnDragExit.RemoveListener(holder.OnItemRemoved); };
            item.OnDragEnter.AddListener(holder.OnItemRemoved);
        }
    }

    private ItemPlaceHolder GetRandomPlaceHolder()  //Warning: can cause infinite loop if all placeholders are filled
    {
        if(placeHolders.Count > 0)
        {
            int index = Random.Range(0, placeHolders.Count - 1);
            if (placeHolders[index].empty)
                return placeHolders[index];
            else
                return GetRandomPlaceHolder();
        }

        return null;
    }

    #endregion

    #region Collisions

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Definitions.LayerDefinitions.Player)
        {
            RoomManager.Instance.SetPlayerRoom(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Definitions.LayerDefinitions.Player)
        {
            RoomManager.Instance.SetPlayerRoom();
        }
    }
    #endregion
}
