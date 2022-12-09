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

        [Header("Place Object")]
        [SerializeField] bool canBePlaced = false;
        [SerializeField] Transform[] placeholders = null;
        [SerializeField] Vector3 placedPos;
        [SerializeField] Vector3 placedRot;

        bool hasBeenGrabbed = false;
        [SerializeField] Collider itemCollider;
        [SerializeField] Rigidbody itemRigidbody;

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

        public override void OnRigidbodyRelease()
        {
            base.OnRigidbodyRelease();
            hasBeenGrabbed = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!isGrabbed && canBePlaced && hasBeenGrabbed)
            {

                foreach(Transform t in placeholders)
                {
                    if (collision.transform == t)
                    {
                        hasBeenGrabbed = false;
                        transform.position = placedPos;
                        transform.rotation = Quaternion.Euler(placedRot);
                        itemCollider.enabled = false;
                        itemRigidbody.isKinematic = true;
                    }
                }             
            }
        }
    }   
}