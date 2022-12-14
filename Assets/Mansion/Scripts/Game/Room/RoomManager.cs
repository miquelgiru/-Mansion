using HFPS.Systems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get { return m_intance; } }
    private static RoomManager m_intance;

    private Room playerRoom;
    [SerializeField] private List<Room> rooms = new List<Room>();

    private void Awake()
    {
        if (m_intance == null)
            m_intance = this;
    }

    #region Room Management
    public void SetPlayerRoom(Room room = null)
    {
        playerRoom = room;
    }

    public bool IsPlayerInRoom(string roomID) => playerRoom.GetRoomID() == roomID;

    public Room GetRoom(string id)
    {
        return (from r in rooms where r.GetRoomID() == id select r).First();    //Warning Comprovar que funciona
    }

    public void PlaceItemInRoom(string roomID, CustomDraggableObject item)
    {
        Room room = GetRoom(roomID);
        room.PlaceItem(item);
    }

    #endregion
}
