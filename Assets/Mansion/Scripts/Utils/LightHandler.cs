using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHandler : MonoBehaviour
{

    [SerializeField] Material switchedOnMaterial;
    [SerializeField] Material switchedOfMaterial;
    [SerializeField] MeshRenderer[] renderers;
    [SerializeField] Light[] lights;


    public void SwitchLight(bool on)
    {

        Material mat = on ? switchedOnMaterial : switchedOfMaterial;

        foreach(var r in renderers)
        {
            r.material = mat;
        } 

        foreach(var l in lights)
        {
            l.gameObject.SetActive(on);
            l.enabled = on;
        }
    }
}
