using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    GameObject[] m_aNeighbours;
    [SerializeField] float m_fRadius = 10.0f;
    int m_nNeighbourCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_aNeighbours = GameObject.FindGameObjectsWithTag("Fish");
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   private Vector3 Separation()
    {
        Vector3 v3Result = new Vector3(0,0,0);

        for(int i = 0; i < m_aNeighbours.Length; i++)
        {
            if (m_aNeighbours[i] != gameObject) // if the neighbour is not yourself
            {
                if (m_aNeighbours[i] != null)
                {
                    v3Result += m_aNeighbours[i].transform.position - transform.position;
                }
            }
        }
        v3Result /= m_aNeighbours.Length;
        v3Result *= -1;


        return v3Result.normalized;
    }

    private Vector3 Alignment()
    {
        Vector3 v3Result = new Vector3(0, 0, 0);

        for(int i = 0; i < m_aNeighbours.Length; i++)
        {

            if (m_aNeighbours[i] != gameObject) // if the neighbour is not yourself
            {
                if (m_aNeighbours[i] != null)
                {
                    if ((m_aNeighbours[i].transform.position - transform.position).sqrMagnitude < m_fRadius * m_fRadius)
                    {
                        Rigidbody rb = m_aNeighbours[i].GetComponent<Rigidbody>();
                        v3Result += rb.velocity;
                        m_nNeighbourCount++;
                    }
                }
            }
        }



        return v3Result.normalized;
    }

    private Vector3 Cohesion()
    {
        return new Vector3();
    }

   
}
