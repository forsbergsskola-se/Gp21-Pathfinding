using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //define method signature
    public delegate void SomeDelegate(); 
    //define event
    public static SomeDelegate onMyEvent; 

    public static void MyEvent()
    {
        if (onMyEvent != null)
            onMyEvent();
    }
}
