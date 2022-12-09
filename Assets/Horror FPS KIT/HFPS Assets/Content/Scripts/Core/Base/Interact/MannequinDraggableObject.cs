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
    }

    public override void OnRigidbodyDragHold()
    {

    }

    public override void OnRigidbodyRelease()
    {
        base.OnRigidbodyRelease();
        animator.SetBool("isCarried", false);
    }
}
