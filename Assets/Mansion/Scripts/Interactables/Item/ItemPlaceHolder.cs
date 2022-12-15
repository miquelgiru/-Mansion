
using HFPS.Systems;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ItemPlaceHolder
{
    public string name;
    public Transform Holder;
    public Vector3 Rotation;
    public bool empty;

    public UnityAction OnItemRemoved;
}
