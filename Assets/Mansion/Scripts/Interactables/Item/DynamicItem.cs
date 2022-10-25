using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicItem : Interactable
{
    private Vector3 originScale;

    public Vector3 offsetPos;
    public Vector3 offsetScale;
    public bool ShowIcon;
    public Rigidbody rigidbody;

    private void Start()
    {
        originScale = transform.localScale;
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnFocus()
    {
        base.OnFocus();
    }

    public override void OnSelect()
    {
        base.OnSelect();
       //PlayerController.Instance.GrabItem(this);
    }

    public Vector3 GetOriginTransformScale()
    {
        return originScale;
    }
}
