using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{


    public virtual void OnExit()
    {
    }

    public virtual void OnFocus()
    {
    }

    public virtual void OnSelect()
    {
    }
}
