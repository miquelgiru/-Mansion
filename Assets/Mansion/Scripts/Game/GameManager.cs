using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ToysManager toysManager;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
       toysManager = GetComponentInChildren<ToysManager>();
    }

    public void CallMethod(string method)
    {
        gameObject.SendMessage(method);
    }

    public void HideAllToys()
    {
        toysManager.HideToys(4);
    }
}
