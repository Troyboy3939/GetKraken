using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    private Animator m_Animator;
    private Vector3 m_v3OriginalPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_v3OriginalPosition = transform.position;
        m_Animator = GetComponent<Animator>();
        m_Animator.enabled = false;
    }

    public IEnumerator Shake()
    {
        yield return new WaitForSeconds(0.5f);
        bool b = true;
        while (b)
        {
            b = false;
            m_Animator.enabled = true;
            yield return new WaitForSeconds(2);
        }
        m_Animator.enabled = false;
        transform.position = m_v3OriginalPosition;
    }
}
