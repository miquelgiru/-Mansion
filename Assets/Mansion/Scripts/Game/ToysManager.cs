using HFPS.Systems;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] ToyDraggableObject[] toys;
    private List<ToyDraggableObject> placedToys = new List<ToyDraggableObject>();


    #region Toy Placing

    public void ToyPlaced(ToyDraggableObject toy)
    {
        if (!placedToys.Contains(toy))
            placedToys.Add(toy);
    }

    [ContextMenu(nameof(HideToy))]
    public void HideToy()
    {
        if(toys.Length > placedToys.Count)
        {
            var toHide = GetRandomToy();
            RoomManager.Instance.PlaceItemInRoom(RoomManager.Instance.GetRandomRoomIDWithoutPlayer(), toHide);
        }
    }

    private ToyDraggableObject GetRandomToy()
    {
        List<ToyDraggableObject> availableToys = new List<ToyDraggableObject>();
        foreach(ToyDraggableObject t in toys){
            if (!placedToys.Contains(t))
                availableToys.Add(t);
        }

        ToyDraggableObject ret = null;
        ret = availableToys[Random.Range(0, availableToys.Count)];
      
        return ret;
    }

    #endregion
}
