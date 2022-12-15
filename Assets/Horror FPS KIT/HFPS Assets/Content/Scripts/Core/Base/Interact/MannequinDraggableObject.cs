using HFPS.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinDraggableObject : CustomDraggableObject
{

    [Header("Animations")]
    [SerializeField] Animator animator;

    public override void OnRigidbodyDrag()
    {
        base.OnRigidbodyDrag();
        animator.SetBool("isCarried", true);
        transform.GetChild(0).localRotation = Quaternion.Euler(0, -90, 0);
    }

    public override void OnRigidbodyDragHold()
    {
        transform.LookAt(2 * Camera.main.transform.position - transform.position);
    }

    public override void OnRigidbodyRelease()
    {
        base.OnRigidbodyRelease();
        animator.SetBool("isCarried", false);
        transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
    }
}
