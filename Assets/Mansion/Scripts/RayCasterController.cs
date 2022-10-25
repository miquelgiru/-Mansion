using System.Collections;
using System.Collections.Generic;
using ThunderWire.Input;
using UnityEngine;

public class RayCasterController : MonoBehaviour
{
    private Transform origin;
    private LayerMask layer;

    private Transform currentTransform;
    private IInteractable currentInteractable;
    private IDraggable currentDraggable;

    public bool EnableRaycaster = true;

    // Start is called before the first frame update
    void Start()
    {
        origin = Camera.main.transform;
        layer = 1 << LayerMask.NameToLayer("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if (EnableRaycaster)
        {
            if(currentDraggable == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(origin.position, origin.forward, out hit, 100, layer))
                {
                    if (hit.transform != currentTransform)
                    {
                        if (currentInteractable != null)
                            currentInteractable.OnExit();

                        currentTransform = hit.transform;
                        currentInteractable = hit.transform.GetComponent<IInteractable>();
                        currentInteractable.OnFocus();    
                    }

                    if (InputHandler.ReadButton("Fire"))
                    {
                        currentInteractable.OnSelect();
                        var drag = hit.transform.GetComponent<IDraggable>();
                        if (drag != null)
                            currentDraggable = drag;
                    }
                }
                else
                {
                    if (currentTransform != null)
                    {
                        currentInteractable.OnExit();
                        currentInteractable = null;
                        currentTransform = null;
                    }
                }
            }
            else
            {
                if (InputHandler.ReadButton("Fire"))
                    currentDraggable?.OnDragStart();

                if (InputHandler.ReadButton("Fire"))
                    currentDraggable.OnDragRunning();

                if (InputHandler.ReadButton("Fire"))
                {
                    currentDraggable.OnDragEnd();
                    currentDraggable = null;
                    currentInteractable.OnExit();
                    currentInteractable = null;
                    currentTransform = null;
                }
            }
        } 
        
    }
}
