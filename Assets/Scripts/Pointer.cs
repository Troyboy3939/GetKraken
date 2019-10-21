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
    [SerializeField] string m_sz4PlayerLevel = "";
    [SerializeField] string m_sz3PlayerLevel = "";
    [SerializeField] string m_sz2PlayerLevel = "";
    bool m_bFirstPlayerConnected = false;
    bool m_bSecondPlayerConnected = false;
    bool m_bThirdPlayerConnected = false;
    bool m_bFourthPlayerConnected = false;
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
                   // SceneManager.LoadScene("OfficialBuild");
                    m_bClicked = true;
                }
            }
        }
        else if (XCI.GetButtonDown(XboxButton.A))
        {
            // SceneManager.LoadScene("OfficialBuild");
            m_bClicked = true;
        }

        if(m_bClicked)
        {
            Vector3 v3Pos = transform.position;
            m_fT += m_fMoveSpeed;
            if(!(m_fT > 1))
            {
                transform.position = Vector3.Lerp(v3Pos, new Vector3(v3Pos.x + m_fDisplacement, v3Pos.y, v3Pos.z), m_fT);

                

                        Calibration c = GetComponentInParent<Calibration>();
                for(int i = 1; i < 5; i++)
                {
                    //Get whether the a button is presed for each controller
                    if (XCI.GetButton(XboxButton.A, (XboxController)i))
                    {
                        
                        if(!m_bFirstPlayerConnected)
                        {
                            c.SetBlueID(i);
                            m_bFirstPlayerConnected = true;
                        }
                        else if(!m_bSecondPlayerConnected)
                        {
                            c.SetGreenID(i);
                            m_bSecondPlayerConnected = true;
                        }
                        else if(!m_bThirdPlayerConnected)
                        {
                            c.SetOrangeID(i);
                            m_bThirdPlayerConnected = true;
                        }
                        else if(!m_bFourthPlayerConnected)
                        {
                            c.SetYelllowID(i);
                            m_bFourthPlayerConnected = true;
                        }


                        
                    }
                }

                //If player 1 presses A
                if (XCI.GetButton(XboxButton.A, XboxController.First))
                {
                    //and if a second player has been connected
                    if(m_bSecondPlayerConnected)
                    {
                        //if a third has
                        if(m_bThirdPlayerConnected)
                        {
                            //if fourth player connected
                            if(m_bFourthPlayerConnected)
                            {
                                //Load 4 player level
                                SceneManager.LoadScene(m_sz4PlayerLevel);
                            }
                            else //fourth isn't connect, load three player level
                            {
                                SceneManager.LoadScene(m_sz3PlayerLevel);
                            }
                        }
                        else //if third hasn't connect load two player level
                        {
                            SceneManager.LoadScene(m_sz2PlayerLevel);
                        }
                    }
                }





            }
           
        }
    }
}
