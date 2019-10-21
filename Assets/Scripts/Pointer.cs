using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class Pointer : MonoBehaviour
{
    bool m_bClicked = false;
    float m_fT = 0.0f;
    [SerializeField] float m_fMoveSpeed = 0.01f;
    [SerializeField] float m_fDisplacement = 10.0f;
    // Start is called before the first frame update
    private void Update()
    {
        // Start the game if "Start" clicked or A button pressed
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition),ray.direction * 5000,Color.red,199);
            if (Physics.Raycast(ray, out hit,500))
            {
                if (hit.transform.gameObject.tag == "MenuStart")
                {
                    SceneManager.LoadScene("OfficialBuild");
                   // m_bClicked = true;
                }
            }
        }
        else if (XCI.GetButtonDown(XboxButton.A))
        {
            SceneManager.LoadScene("OfficialBuild");
        }

        if(m_bClicked)
        {
            Vector3 v3Pos = transform.position;
            m_fT += m_fMoveSpeed;
            if(!(m_fT > 1))
            {
                transform.position = Vector3.Lerp(v3Pos, new Vector3(v3Pos.x + m_fDisplacement, v3Pos.y, v3Pos.z), m_fT);
            }
           
        }
    }
}
