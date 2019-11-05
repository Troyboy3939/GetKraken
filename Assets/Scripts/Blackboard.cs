using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blackboard : MonoBehaviour
{
    private static Blackboard instance = null;

    GameObject tentacle;
    GameObject plane;
    GameObject canvas;
    GameObject[] m_Chests;
    Node[,] m_Nodes;
    FloorGrid m_Grid;
    [SerializeField] float m_fTentacleVerticalPercentage;
    [SerializeField] Material m_OrangeMaterial = null;
    [SerializeField] Material m_BlueMaterial = null;
    [SerializeField] Material m_YellowMaterial = null;
    [SerializeField] Material m_GreenMaterial = null;


    [SerializeField] Material m_OrangeChestMaterial = null;
    [SerializeField] Material m_BlueChestMaterial = null;
    [SerializeField] Material m_YellowChestMaterial = null;
    [SerializeField] Material m_GreenChestMaterial = null;

    void Awake()
    {
        //Debug.Log("Creating a blackboard now");
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

    private void Update()
    {
     
    }

    // This is required since the blackboard is only created once as a singleton, and you need
    // to find these objects every time the scene is loaded. It's not a default Unity method, see Awake.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        tentacle = GameObject.Find("Tentacle");
        plane = GameObject.Find("Plane");
        canvas = GameObject.Find("/UI/Canvas");
        m_Chests = GameObject.FindGameObjectsWithTag("Chest");
    }

    public Material GetBlueMat()
    {
        return m_BlueMaterial;
    }
    public Material GetGreenMat()
    {
        return m_GreenMaterial;
    }

    public Material GetYellowMat()
    {
        return m_YellowMaterial;
    }

    public Material GetOrangeMat()
    {
        return m_OrangeMaterial;
    }

    public Material GetBlueChestMat()
    {
        return m_BlueChestMaterial;
    }
    public Material GetGreenChestMat()
    {
        return m_GreenChestMaterial;
    }

    public Material GetYellowChestMat()
    {
        return m_YellowMaterial;
    }

    public Material GetOrangeChestMat()
    {
        return m_OrangeChestMaterial;
    }






    public static Blackboard GetInstance()
    {
        return instance;
    }

    public int NodeSize(int nDimension)
    {
        return m_Nodes.GetLength(nDimension);
    }


    public GameObject GetTentacle()
    {
        return tentacle;
    }

    public GameObject GetPlane()
    {
        return plane;
    }

    public float GetTentacleVerticalPercentage()
    {
        return m_fTentacleVerticalPercentage;
    }

    public void SetTentacleVerticalPercentage(float fPercentage)
    {
        m_fTentacleVerticalPercentage = fPercentage;
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

    public void SetNodes(ref Node[,] nodes)
    {
        m_Nodes = nodes;
    }

    public Node GetNode(Vector2 vec)
    {
        if (vec.x <= m_Nodes.GetLength(0))
        {
            if (vec.x <= m_Nodes.GetLength(1))
            {
                return m_Nodes[Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.y)];
            }
        }
        //Debug.Log("Blackboard, GetNode  out of bounds! X: " + vec.x + ", Y: " + vec.y);
        return null;
    }

    public Node GetNode(int x, int y)
    {
        return m_Nodes[x, y];
    }

    public FloorGrid GetGrid()
    {
        return m_Grid;
    }

    public void SetGrid(FloorGrid grid)
    {
        m_Grid = grid;
    }


}
