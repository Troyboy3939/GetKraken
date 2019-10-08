using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    private static Blackboard instance = null;
    [SerializeField] GameObject tentacle;
    [SerializeField] GameObject plane;
    [SerializeField] GameObject canvas;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public static Blackboard GetInstance()
    {
        return instance;
    }

    public GameObject GetTentacle()
    {
        return tentacle;
    }

    public GameObject GetPlane()
    {
        return plane;
    }

    public GameObject GetChestWithID(int id)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Chest");

        foreach (GameObject go in gos)
        {
            if (go.GetComponent<ChestController>().m_nPlayerID == id)
            {
                return go;
            }
        }

        Debug.Log("No chest found.");
        return null;
    }

    public GameObject GetCanvas()
    {
        return canvas;
    }
}
