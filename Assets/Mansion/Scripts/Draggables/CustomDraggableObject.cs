/*
 * DraggableObject.cs - by ThunderWire Studio
 * ver. 1.0
*/

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        [SerializeField] protected Transform[] exclusiveplaceHolders = null;
        [SerializeField] protected Vector3 placedPos;
        [SerializeField] protected Vector3 placedRot;

        protected bool hasBeenGrabbed = false;
        [SerializeField] protected Collider itemCollider;
        [SerializeField] protected Rigidbody itemRigidbody;

        public UnityEvent OnDragEnter;
        public UnityEvent OnDragExit;

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
            itemRigidbody.freezeRotation = false;

            if (canBePlaced)
            {
                ToysManager.Instance.TryPlaceToy(this);
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if(!isGrabbed && canBePlaced && hasBeenGrabbed)
            {

                foreach(Transform t in exclusiveplaceHolders)
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