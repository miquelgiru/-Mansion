using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable, IDraggable
{
    public bool inverse = false;
    private float closedRot;
    private float openedRot;
    private float speed = 1;
    private float closeThreshold = 20;

    private void Start()
    {
        closedRot = transform.rotation.eulerAngles.y;
        openedRot = inverse ? closedRot + 85 : closedRot - 90;
    }


    public void OnDragEnd()
    {

        if (!inverse)
        {
            if (closedRot - transform.rotation.eulerAngles.y <= closeThreshold)
                transform.rotation = Quaternion.Euler(0, closedRot, 0);
        }
        else
        {
            if (Mathf.Abs(closedRot - transform.rotation.eulerAngles.y) <= closeThreshold)
                transform.rotation = Quaternion.Euler(0, closedRot, 0);
        }

    }

    public void OnDragRunning()
    {
        transform.Rotate(new Vector3(0, UnityEngine.Input.GetAxis("Mouse X") * speed, 0));

        float finalY = transform.rotation.eulerAngles.y;

        if (!inverse)
        {
            if (finalY < openedRot)
                finalY = openedRot;
            else if (finalY > closedRot)
            {
                if (finalY > 300)
                    finalY = openedRot;
                else
                    finalY = closedRot;
            }        }
        else
        {
            Debug.Log(finalY);
            if (finalY > openedRot)
            {
                if (finalY > 300 && openedRot < 300)
                    finalY = closedRot;
                else
                    finalY = openedRot;
            }
            else if (finalY < closedRot && finalY < openedRot)
            {
                if (closedRot - finalY > 100)
                    finalY = openedRot;
                else
                    finalY = closedRot;
            }
        }
       

        transform.rotation = Quaternion.Euler(0, finalY, 0);
    }

    public void OnDragStart()
    {
        
    }
}
