using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Node class. Node represent a tile
public class Node
{
    public Vector3 m_v3Position;
    public StateMachine m_StateMachine;
    GameObject m_BasePlane;
    GameObject m_Plane;


   

    public Node(float fX, float fY, float fZ,  GameObject Plane)
    {

        m_v3Position.x = fX;
        m_v3Position.y = fY;
        m_v3Position.z = fZ;
        m_BasePlane = Plane;
        m_Plane = GameObject.Instantiate(m_BasePlane, m_v3Position, new Quaternion(0, 0, 0, 0));
        m_StateMachine = new StateMachine(m_v3Position, ref m_Plane);
    }

    public Node(Vector3 v3Position, GameObject Plane)
    {
        m_v3Position = v3Position;
        m_BasePlane = Plane;
        m_Plane = GameObject.Instantiate(m_BasePlane, m_v3Position, new Quaternion(0, 0, 0, 0));
        m_StateMachine = new StateMachine(m_v3Position, ref m_Plane);

    }
    public GameObject GetPlane()
    {
        return m_Plane;
    }
    public Vector3 GetPosition()
    {
        return m_v3Position;
    }
    public StateMachine GetStateMachine()
    {
        return m_StateMachine;
    }

    public bool GetHasTentacle()
    {
        return m_StateMachine.GetFloorState().GetHasTentacle();
    }

    public bool GetHasChest()
    {
        return m_StateMachine.GetFloorState().GetHasChest();
    }

    public void SetHasTentacle(bool b)
    {
        m_StateMachine.GetFloorState().SetHasTentacle(b);
    }

    public void SetHasChest(bool b)
    {
        m_StateMachine.GetFloorState().SetHasChest(b);
    }

    private void Start()
    {

    }

    private void Update()
    {
        m_StateMachine.Update();
    }

    public void ChangeState()
    {
        m_StateMachine.ChangeState();
    }

    public StateMachine.ESTATE GetState()
    {
        return m_StateMachine.GetState();
    }
}