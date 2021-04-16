using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftBehavior : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Forest" || other.tag == "Lava" || other.tag == "World")
        {
            LevelManager.instance.canShift = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Forest" || other.tag == "Lava" || other.tag == "World")
        {
            LevelManager.instance.canShift = false;
        }
    }
    
    //Bug occurs here
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Forest" || other.tag == "Lava" || other.tag == "World")
        {
            LevelManager.instance.canShift = true;
        }
    }

    
}
