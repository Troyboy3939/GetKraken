using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class FishController : MonoBehaviour
{
    GameObject[] m_aNeighbours;
    [SerializeField] float m_fRadius = 0.1f;
    [SerializeField] float m_fMaxSpeed = 5.0f;
    Rigidbody m_rb;
    Vector3 m_v3Start;

    [Range(0,15)]
    [SerializeField] float m_fAlignmentMod = 0.0f;

    [Range(0, 15)]
    [SerializeField] float m_fCohesionMod = 0.0f;

    [Range(0, 15)]
    [SerializeField] float m_fSeperationMod = 0.0f;


    [Range(0, 15)]
    [SerializeField] float m_fSharkSeperationMod = 0.0f;

    [Range(0, 15)]
    [SerializeField] float m_fSeekMod = 0.0f;
    GameObject m_Shark;
    int m_nCurrentWaypoint = 0;
    List<GameObject> m_Waypoints = null;
    [SerializeField] float m_fWaypointDistance = 5;
    // Start is called before the first frame update
    void Start()
    {
        m_aNeighbours = GameObject.FindGameObjectsWithTag("Fish");
        m_rb = GetComponent<Rigidbody>();
        m_v3Start = transform.position;

        m_Shark = GameObject.FindGameObjectWithTag("Shark");
        m_Waypoints = Blackboard.GetInstance().GetWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        //If you are close enough
        if(Vector3.Distance(transform.position,m_Waypoints[m_nCurrentWaypoint].transform.position) < m_fWaypointDistance)
        {
            m_nCurrentWaypoint++;
        }
        if(m_nCurrentWaypoint > m_Waypoints.Count)
        {
            m_nCurrentWaypoint = 0;
        }
       
       
        Vector3 v3Acceleration = (m_fAlignmentMod * Alignment()) + (m_fCohesionMod * Cohesion()) + (m_fSeperationMod * Separation()) + (m_fSeekMod * Seek(m_Waypoints[m_nCurrentWaypoint].transform.position) + (SharkSeparation() * m_fSharkSeperationMod) );
        //transform.localRotation = Quaternion.LookRotation(transform.forward,Vector3.up);
        



        if (m_rb != null)
        {
            
            m_rb.AddForce(v3Acceleration * Time.deltaTime,ForceMode.VelocityChange);
        }

        if(m_rb.velocity.magnitude > 1)
            transform.forward = m_rb.velocity.normalized;
    }
    private Vector3 Seek(Vector3 v3Pos)
    {
        Vector3 v3Desired = new Vector3(0,0,0);

        v3Desired = v3Pos - transform.position;
        v3Desired.Normalize();
        v3Desired *= m_fMaxSpeed;

        
        return v3Desired - m_rb.velocity;
    }
   private Vector3 Separation()
    {
        Vector3 v3Result = new Vector3(0, 0, 0);

        for (int i = 0; i < m_aNeighbours.Length; i++) // for every neighbour
        {
            if (m_aNeighbours[i] != gameObject) // if the neighbour is not yourself
            {
                if (m_aNeighbours[i] != null) // and the neighbour is valid
                {
                    if (Vector3.Distance(m_aNeighbours[i].transform.position,transform.position) < m_fRadius / 2 ) //and the neighbour is within the neighbour radius 
                    {
                       
                            Vector3 v = m_aNeighbours[i].transform.position - transform.position; 
                            v3Result -= v / v.magnitude;
                        
                    }
                }
            }
        }
      
        return v3Result;
    
    }
    private Vector3 SharkSeparation()
    {
        Vector3 v3Result = new Vector3(0, 0, 0);

        GameObject s = GameObject.FindGameObjectWithTag("Shark");
        if (s != null)
        {
            Vector3 v = s.transform.position - transform.position;
            v3Result -= v.normalized;
        }

        return v3Result;
    }


private Vector3 Alignment()
    {
        Vector3 v3Result = new Vector3(0, 0, 0);
        int nNeighbourCount = 0;
        for (int i = 0; i < m_aNeighbours.Length; i++)
        {

            if (m_aNeighbours[i] != gameObject) //if the neighbour is not yourself
            {
                if (m_aNeighbours[i] != null)
                {
                    if ((m_aNeighbours[i].transform.position - transform.position).sqrMagnitude < m_fRadius * m_fRadius)
                    {
                        Rigidbody rb = m_aNeighbours[i].GetComponent<Rigidbody>();
                        v3Result += rb.velocity;
                        nNeighbourCount++;
                    }
                }
            }

            if(nNeighbourCount > 0)
            {
                v3Result /= nNeighbourCount;
            }
        }



        return v3Result - m_rb.velocity;
    }

    private Vector3 Cohesion()
    {
        Vector3 v3CentreOfMass = new Vector3(0, 0, 0);
        Vector3 v3SteeringForce = new Vector3(0, 0, 0);

        int nNeighbourCount = 0;
        for (int i = 0; i < m_aNeighbours.Length; i++)
        {
           if(m_aNeighbours[i] != null && m_aNeighbours[i] != this && Vector3.Distance(m_aNeighbours[i].transform.position,transform.position) < m_fRadius)
           {
                v3CentreOfMass += m_aNeighbours[i].transform.position;
                nNeighbourCount++;
           }
        }
       
        if(nNeighbourCount > 0)
        {
            v3CentreOfMass /= nNeighbourCount;
            v3SteeringForce = Seek(v3CentreOfMass);
        }
       


        return Seek(v3SteeringForce);
    }

   
}
