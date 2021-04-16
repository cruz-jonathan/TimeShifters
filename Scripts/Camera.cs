using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public static Camera instance;

    void Awake()
    {
        instance = this;    //creates camera instance
    }
}
