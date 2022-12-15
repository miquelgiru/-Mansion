using HFPS.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyDraggableObject : CustomDraggableObject
{

    protected override void OnCollisionEnter(Collision collision)
    {
        if (!isGrabbed  && hasBeenGrabbed)
        {
            foreach (Transform t in exclusiveplaceHolders)
            {
                if (collision.transform == t)
                {
                    hasBeenGrabbed = false;
                    transform.position = placedPos;
                    transform.rotation = Quaternion.Euler(placedRot);
                    itemCollider.enabled = false;
                    itemRigidbody.isKinematic = true;
                    ToysManager.Instance.ToyPlaced(this);
                }
            }
        }
    }
}
