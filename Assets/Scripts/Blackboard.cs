using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // This is required since the blackboard is only created once as a singleton, and you need
    // to find the chests every time the scene is loaded. It's not a default Unity method, see Awake.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
