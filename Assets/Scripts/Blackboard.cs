using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    private static Blackboard instance = null;
    [SerializeField] GameObject tentacle;
    [SerializeField] GameObject plane;
    [SerializeField] GameObject canvas;
    GameObject[] m_Chests;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        m_Chests = GameObject.FindGameObjectsWithTag("Chest");
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
        foreach (GameObject go in m_Chests)
        {
            if (go.GetComponent<ChestController>().m_nPlayerID == id)
            {
                return go;
            }
        }

        return null;
    }

    public GameObject GetChest(int nIndex)
    {
        return m_Chests[nIndex];
    }

    public int GetChestCount()
    {
        return m_Chests.Length;
    }

    public GameObject GetCanvas()
    {
        return canvas;
    }
}
