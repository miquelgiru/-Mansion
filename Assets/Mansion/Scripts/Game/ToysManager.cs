using HFPS.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToysManager : MonoBehaviour
{
    [SerializeField] CustomDraggableObject[] toys;

    private List<CustomDraggableObject> placedToys;

    private void Start()
    {
        
    }

    public void ToyPlaced(CustomDraggableObject toy)
    {
        if (!placedToys.Contains(toy))
            placedToys.Add(toy);
    }
}
