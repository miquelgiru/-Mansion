using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Definitions
{
   
    public static class LayerDefinitions
    {
        public static LayerMask Player = LayerMask.NameToLayer("PlayerBody");
        public static LayerMask Interactable = LayerMask.NameToLayer("Interact");
    }

    public static class RoomIds
    {
        public static string Room1 = "Room1";
        public static string Room2 = "Room2";
        public static string Room3 = "Room3";
        public static string Room4 = "Room4";
    }

}
