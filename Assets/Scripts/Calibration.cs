using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
    int m_nOrangeID = 1;
    int m_nGreenID = 2;
    int m_nBlueID = 3;
    int m_nYellowID = 4;

    public int GetOrangeID()
    {
        return m_nOrangeID;
    }

    public int GetGreenID()
    {
        return m_nGreenID;
    }

    public int GetBlueID()
    {
        return m_nBlueID;
    }

    public int GetYellowID()
    {
        return m_nYellowID;
    }

    public void SetOrangeID(int nID)
    {
        m_nOrangeID = nID;
    }
    public void SetGreenID(int nID)
    {
        m_nGreenID = nID;
    }
    public void SetBlueID(int nID)
    {
        m_nBlueID = nID;
    }
    public void SetYellowID(int nID)
    {
        m_nYellowID = nID;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
