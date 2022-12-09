/*
 * DraggableObject.cs - by ThunderWire Studio
 * ver. 1.0
*/

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using WaterBuoyancy;
using WaterBuoyancy.Collections;

namespace HFPS.Systems
{
    /// <summary>
    /// Script defining Draggable Objects.
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class CustomDraggableObject : DraggableObject
    {
        [SerializeField] bool customRotationOnGrab = false;

        private void Start()
        {
            enableWaterBuoyancy = false;
            enableWaterFoam = false;
            calculateDensity = false;
        }


        public override void OnRigidbodyDragHold() {

            if (customRotationOnGrab)
                transform.LookAt(2 * Camera.main.transform.position - transform.position);
        }
    }   
}