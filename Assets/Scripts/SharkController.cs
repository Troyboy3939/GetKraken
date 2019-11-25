using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkController : MonoBehaviour
{
    [SerializeField] float m_fMaxSpeed = 5.0f;
    Rigidbody m_rb;
    GameObject[] m_aFishes;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_aFishes = GameObject.FindGameObjectsWithTag("Fish");
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 v3Pos = new Vector3(0, 0, 0);
        
        for(int i = 0; i < m_aFishes.Length; i++)
        {
            v3Pos += m_aFishes[i].transform.position;
        }

        v3Pos /= m_aFishes.Length;
       

        if (m_rb.velocity.magnitude > 1)
        {
            transform.forward = m_rb.velocity.normalized;
        }

        if (m_rb != null)
        {
            //Seek toward the averaged position
            m_rb.AddForce(Seek(m_aFishes[0].transform.position) * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    private Vector3 Seek(Vector3 v3Pos)
    {
        Vector3 v3Desired = new Vector3(0, 0, 0);

        v3Desired = v3Pos - transform.position;
        v3Desired.Normalize();
        v3Desired *= m_fMaxSpeed;


        return v3Desired - m_rb.velocity;
    }

}