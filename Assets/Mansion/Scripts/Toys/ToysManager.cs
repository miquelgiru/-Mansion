using HFPS.Systems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ToysManager : MonoBehaviour
{
    #region Intance
    public static ToysManager Instance { get { return m_intance; } }
    private static ToysManager m_intance;

    private void Awake()
    {
        if (m_intance == null)
            m_intance = this;
    }
    #endregion

    [SerializeField] float placingDistance = 1;
    [SerializeField] CustomDraggableObject[] toys;
    private List<CustomDraggableObject> placedToys = new List<CustomDraggableObject>();
    [SerializeField] private ToyPlacer[] placers;


    #region Toy Placing

    public void ToyPlaced(CustomDraggableObject toy)
    {
        if (!placedToys.Contains(toy))
            placedToys.Add(toy);
    }

    public void TryPlaceToy(CustomDraggableObject toy)
    {
        var placer = GetPlacer();
        if(Vector3.Distance(toy.transform.position, placer.transform.position) < placingDistance)
        {
            placer.isEmpty = false;
            placer.CurrentToy = toy;
            toy.transform.position = placer.transform.position;
            toy.transform.rotation = placer.transform.rotation;
            toy.gameObject.layer = 0;
            placedToys.Remove(toy);
        }
    }

    [ContextMenu(nameof(HideToy))]
    public void HideToy()
    {
        if(toys.Length > placedToys.Count)
        {
            var toHide = GetRandomToy();
            toHide.gameObject.layer = 9;
            ToyPlacer placer = placers.Where(p => p.CurrentToy == toHide).FirstOrDefault();
            if(placer != null)
            {
                placer.isEmpty = true;
                placer.CurrentToy = null;
            }
            RoomManager.Instance.PlaceItemInRoom(RoomManager.Instance.GetRandomRoomIDWithoutPlayer(), toHide);
        }
    }

    public void HideToys(int toyNumber)
    {
        for (int i = 0; i < toyNumber; i++)
            HideToy();
    }

    private CustomDraggableObject GetRandomToy()
    {
        List<CustomDraggableObject> availableToys = new List<CustomDraggableObject>();
        foreach(CustomDraggableObject t in toys){
            if (!placedToys.Contains(t))
                availableToys.Add(t);
        }

        CustomDraggableObject ret = null;
        ret = availableToys[Random.Range(0, availableToys.Count)];
        placedToys.Add(ret);
        return ret;
    }

    private ToyPlacer GetPlacer()
    {
        return placers.Where(p => p.isEmpty).First();
    }

    #endregion
}
