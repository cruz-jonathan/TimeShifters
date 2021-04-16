using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject worldOne;             //main tilemap related to world 1
    public GameObject[] worldOneObjects;    //all objects related to world 1

    public GameObject worldTwo;             //main tilemap related to world 2
    public GameObject[] worldTwoObjects;    //all objects related to world 2

    public bool canShift = true;

    public TilemapRenderer[] worldOneTilemap;
    public TilemapRenderer[] worldTwoTilemap;

    public GameObject[] worldOneColliders;
    public GameObject[] worldTwoColliders;

    private bool worldOneView = true;
    private bool worldTwoView = false;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        worldOneObjects = GameObject.FindGameObjectsWithTag("Past");

        worldTwoObjects = GameObject.FindGameObjectsWithTag("Future");

        //~~Despawn Objects from World 2~~//
        for (int i = 0; i < worldTwoObjects.Length; i++)
        {
            worldTwoObjects[i].SetActive(false);
        }

    }

    //flips the world based on the value given
    //worldActive : world currently active
    //worldDeactive : world currently deactive
    public void flipWorld(TilemapRenderer[] worldActive, TilemapRenderer[] worldDeactive)
    {
        //flip the worlds
        for (int i = 0; i < 3; i++)
        {
            worldActive[i].enabled = false;
            worldDeactive[i].enabled = true;
        }

    }

    //Flips the Colliders to triggers
    //worldActive: world currently active
    //worldDeactive: world currently deactive
    public void flipColliders(GameObject[] worldActive, GameObject[] worldDeactive)
    {
        for (int i = 0; i < 2; i++)
        {
            worldActive[i].GetComponent<CompositeCollider2D>().isTrigger = true;
            worldDeactive[i].GetComponent<CompositeCollider2D>().isTrigger = false;
        }
    }

    //Peek at the world
    public void peekWorld()
    {
        if (worldOneView)
        {
            /*
            worldOneTilemap.enabled = false;    //set world 1 deactive
            worldTwoTilemap.enabled = true;     //set world 2 active
            */

            flipWorld(worldOneTilemap, worldTwoTilemap);

            worldTwo.transform.position = worldOne.transform.position;   //move it up

            //~~Despawn Objects from World 1~~//
            for (int i = 0; i < worldOneObjects.Length; i++)
            {
                worldOneObjects[i].SetActive(false);
            }

            //~~Respawn Objects from World 2~~//
            for (int i = 0; i < worldTwoObjects.Length; i++)
            {
                worldTwoObjects[i].SetActive(true);
                worldTwoObjects[i].GetComponent<Collider2D>().isTrigger = true; //turn into trigger to prevent crashing into each other
                worldTwoObjects[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }
        else if (worldTwoView)
        {
            /*
           worldOneTilemap.enabled = true;    //set world 1 active
           worldTwoTilemap.enabled = false;     //set world 2 deactive
           */

            flipWorld(worldTwoTilemap, worldOneTilemap);

            worldOne.transform.position = worldTwo.transform.position;

            //~~Respawn Objects from World 1~~//
            for (int i = 0; i < worldOneObjects.Length; i++)
            {
                worldOneObjects[i].SetActive(true);
                worldOneObjects[i].GetComponent<Collider2D>().isTrigger = true; //turn into trigger to prevent crashing into each other
                worldOneObjects[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }

            //~~Despawn Objects from World 2~~//
            for (int i = 0; i < worldTwoObjects.Length; i++)
            {
                worldTwoObjects[i].SetActive(false);
            }

        }
    }

    public void unPeekWorld()
    {
        if (worldOneView)
        {
            /*
            worldOneTilemap.enabled = true; //set world 1 active
            worldTwoTilemap.enabled = false;    //set world 2 active
            */

            flipWorld(worldTwoTilemap, worldOneTilemap);

            worldTwo.transform.position = worldTwo.transform.position - new Vector3(0, 0.2f, 0);

            //~~Respawn Objects from World 1~~//
            for (int i = 0; i < worldOneObjects.Length; i++)
            {
                worldOneObjects[i].SetActive(true);
            }

            //~~Despawn Objects from World 2~~//
            for (int i = 0; i < worldTwoObjects.Length; i++)
            {
                worldTwoObjects[i].SetActive(false);
                worldTwoObjects[i].GetComponent<Collider2D>().isTrigger = false;
                worldTwoObjects[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }

        }
        else if (worldTwoView)
        {
            /*
            worldOneTilemap.enabled = false;    //set world 1 deactive
            worldTwoTilemap.enabled = true;     //set world 2 active
            */

            flipWorld(worldOneTilemap, worldTwoTilemap);

            worldOne.transform.position = worldOne.transform.position - new Vector3(0, 0.2f, 0);

            //~~Respawn Objects from World 1~~//
            for (int i = 0; i < worldOneObjects.Length; i++)
            {
                worldOneObjects[i].SetActive(false);
                worldOneObjects[i].GetComponent<Collider2D>().isTrigger = false;
                worldOneObjects[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            }

            //~~Despawn Objects from World 2~~//
            for (int i = 0; i < worldTwoObjects.Length; i++)
            {
                worldTwoObjects[i].SetActive(true);
            }
        }
    }

    public void switchWorld()
    {
        if (canShift)
        {
            //If World One is currently active
            if (worldOneView)
            {
                flipWorld(worldOneTilemap, worldTwoTilemap);
                flipColliders(worldOneColliders, worldTwoColliders);

                //~~~~~~~~World 1 Stuff~~~~~~~~~~~~~~//
                //Set World 1 to be inactive
                //worldOneTilemap.enabled = false;
                //Move World 1 down to make sure player isn't stepping on it
                worldOne.transform.position = worldOne.transform.position - new Vector3(0, 0.2f, 0);
                //Turn into a trigger
                //worldOne.GetComponent<CompositeCollider2D>().isTrigger = true;

                //~~Despawn Objects from World 1~~//
                for (int i = 0; i < worldOneObjects.Length; i++)
                {
                    worldOneObjects[i].SetActive(false);
                }




                //~~~~~~~~~World 2 Stuff~~~~~~~~~~~~~~~//
                //Set World 2 to be active
                //worldTwoTilemap.enabled = true;
                //Move World 2 back into position
                worldTwo.transform.position = worldTwo.transform.position + new Vector3(0, 0.2f, 0);

                //Turn into a collider
                //worldTwo.GetComponent<CompositeCollider2D>().isTrigger = false;

                //~~Respawn Objects from World 2~~//
                for (int i = 0; i < worldTwoObjects.Length; i++)
                {
                    worldTwoObjects[i].SetActive(true);
                }




                worldOneView = false;
                worldTwoView = true;
            }
            //If world two is currently active
            else if (worldTwoView)
            {
                flipWorld(worldTwoTilemap, worldOneTilemap);
                flipColliders(worldTwoColliders, worldOneColliders);

                //~~~~~~World 1 Stuff~~~~~~~//
                //Set World 1 to be active
                //worldOneTilemap.enabled = true;
                //Move back into position
                worldOne.transform.position = worldOne.transform.position + new Vector3(0, 0.2f, 0);

                //Turn into a collider
                //worldOne.GetComponent<CompositeCollider2D>().isTrigger = false;

                //~~Respawn Objects from World 1~~//
                for (int i = 0; i < worldOneObjects.Length; i++)
                {
                    worldOneObjects[i].SetActive(true);
                }




                //~~~~~~World 2 Stuff~~~~~~//
                //Set World 2 to be inactive
                //worldTwoTilemap.enabled = false;
                //Move World 2 down to make sure player isn't stepping on it
                worldTwo.transform.position = worldTwo.transform.position - new Vector3(0, 0.2f, 0);
                //Turn World 2 into a trigger
                //worldTwo.GetComponent<CompositeCollider2D>().isTrigger = true;

                //~~Despawn Objects from World 2~~//
                for (int i = 0; i < worldTwoObjects.Length; i++)
                {
                    worldTwoObjects[i].SetActive(false);
                }

                worldOneView = true;
                worldTwoView = false;
            }
        }
        else
        {
            Debug.Log("Can't Shift Now");
        }
    }
}
