using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{

    //[SerializeField] float m_fLerpStart = 0.0f;
    float m_fT = 0.0f;
    bool m_bSwitch = false;
    [SerializeField] float m_fSpeed = 0.005f;
    [SerializeField] float m_fStartY = 0.0f;
    [SerializeField] float m_fPositionSinModifier1 = 0.0f;
    [SerializeField] float m_fPositionSinModifier2 = 0.0f;
    [SerializeField] float m_RotationStart = 82.0f;
    [SerializeField] float m_RotationEnd = 101.0f;
    float Lerp(float fStart, float fEnd, float fT)
    {
        return fStart * (1 - fT) + fEnd * fT;
    }

    Vector3 startPos;
    float occilationSpeed = 2.0f;
    float displacementScale = 0.50f;
    float rngOffest;
    float rngOffestrolling;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rngOffest = Random.value;
        rngOffestrolling = Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = new Vector3(startPos.x, startPos.y + Mathf.Sin(Time.time * (occilationSpeed + rngOffest)) * displacementScale, startPos.z);
        transform.localEulerAngles = new Vector3(0, Mathf.Sin(Time.time + rngOffestrolling) * 5.0f, 90 + Mathf.Sin(Time.time * (occilationSpeed + rngOffestrolling)) * 10);
    }
}
