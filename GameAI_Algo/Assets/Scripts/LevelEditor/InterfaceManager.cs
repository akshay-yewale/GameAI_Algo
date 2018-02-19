using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{

    public bool isMouseOverUIElement;
    private static InterfaceManager instance = null;
    
    void Awake()
    {
        instance = this;

    }

    private void Start()
    {
       
    }
    public static InterfaceManager GetInstance()
    {
        return instance;
    }

    public void OnMouseEnter()
    {
        isMouseOverUIElement = true;
    }

    public void OnMouseExit()
    {
        isMouseOverUIElement = false;
    }
}
