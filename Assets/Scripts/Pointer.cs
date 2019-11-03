﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    [SerializeField] Vector3 m_v3InterpolationEnd = new Vector3(0,0,0);
    [SerializeField] GameObject m_Bezier1 = null;
    [SerializeField] GameObject m_Bezier2 = null;


    bool m_bFirstControllerConnected = false;
    bool m_bFirstPlayerConnected = false;
    bool m_bSecondPlayerConnected = false;
    bool m_bThirdPlayerConnected = false;
    bool m_bFourthPlayerConnected = false;

    bool[] m_bControllersConnected = new bool[5] {true,false,false,false,false};
    [SerializeField] GameObject[] m_Players = new GameObject[4];

    [SerializeField] float m_fMouseSpeed = 5f;
    private Vector2 m_v2CursorPosition;
    [SerializeField] Texture m_tCursorImage;

    [SerializeField] Collider m_cBarrelCollider;
    Vector3 m_Rot; 
    [SerializeField] Vector3 m_RotationEnd;

    [SerializeField] float m_fLerpSpeedScale = 4;
    Vector3 m_v3Pos;
    bool m_bDone = false;
    private void Start()
    {
        // Disable the mouse cursor and use a software cursor
        //Cursor.visible = false;
        m_v2CursorPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        m_Rot = transform.rotation.eulerAngles;
        
    }

    private void OnGUI()
    {
        // Allow a joystick to move the mouse cursor if a controller is connected
        if (XCI.IsPluggedIn(1) && !m_bClicked)
        {
            Cursor.visible = false;

            float h = (m_fMouseSpeed * XCI.GetAxis(XboxAxis.LeftStickX)) * Time.deltaTime;
            float v = (m_fMouseSpeed * XCI.GetAxis(XboxAxis.LeftStickY)) * Time.deltaTime;

            m_v2CursorPosition.x += h;
            m_v2CursorPosition.y += v;

            if (m_tCursorImage != null)
            {
                GUI.DrawTexture(new Rect(m_v2CursorPosition.x, Screen.height - m_v2CursorPosition.y, 32, 32), m_tCursorImage);
            }
        }
        else
        {
            Cursor.visible = true;
        }
    }

    private void FixedUpdate()
    {
        if (XCI.IsPluggedIn(1) && !m_bClicked)
        {
            if (XCI.GetButtonDown(XboxButton.A) && !m_bClicked)
            {
                Click(m_v2CursorPosition);
            }
        }
    }

    private void Update()
    {
        // Start the game if "Start" clicked or A button pressed
        if (Input.GetMouseButtonUp(0))
        {
            Click(Input.mousePosition);
        }

        if(m_bClicked)
        {
            if (!m_bDone)
            {
               m_v3Pos = transform.position;
                m_bDone = true;
            }
            
            m_fT += m_fMoveSpeed * Time.deltaTime;
            //Debug.Log(m_fT);
            if((m_fT < 1))
            {
                Vector3 rot = Vector3.Lerp(m_Rot, m_RotationEnd, m_fT * m_fLerpSpeedScale * 1.5f);
                transform.position = Bezier(m_v3Pos, m_v3InterpolationEnd,m_Bezier1.transform.position,m_Bezier2.transform.position,m_fT * m_fLerpSpeedScale);
                transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
            }
            else
            {
                Calibration c = GameObject.FindGameObjectWithTag("Calibration").GetComponent<Calibration>();

                // TODO: Make it possible to calibrate keyboard for a specific player

                //If player 1 presses A
                if (m_bFirstControllerConnected && (XCI.GetButtonDown(XboxButton.A, XboxController.First) || Input.GetKeyDown(KeyCode.Space)))
                {
                    //and if a second player has been connected
                    if (m_bSecondPlayerConnected)
                    {
                        //if a third has
                        if (m_bThirdPlayerConnected)
                        {
                            //if fourth player connected
                            if (m_bFourthPlayerConnected)
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

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (m_bFirstPlayerConnected)
                    {
                        if (m_bSecondPlayerConnected)
                        {
                            if (m_bThirdPlayerConnected)
                            {

                            }
                            else
                            {

                            }
                        }
                    }
                }

                for (int i = 1; i < 5; i++)
                {
                    //Get whether the a button is presed for each controller
                    if (XCI.GetButtonUp(XboxButton.A, (XboxController)i))
                    {
                        if ((XboxController)i == XboxController.First)
                        {
                            m_bFirstControllerConnected = true;
                        }

                        if (!m_bFirstPlayerConnected)
                        {
                            if (!m_bControllersConnected[i])
                            {
                                c.SetOrangeID(i);
                                m_bFirstPlayerConnected = true;
                                m_bControllersConnected[i] = true;
                                m_Players[0].GetComponent<MainMenuDiverController>().Go();
                            }
                         }
                        else if (!m_bSecondPlayerConnected)
                        {
                            if (!m_bControllersConnected[i])
                            {
                                c.SetGreenID(i);
                                m_bSecondPlayerConnected = true;
                                m_bControllersConnected[i] = true;
                                m_Players[1].GetComponent<MainMenuDiverController>().Go();
                            }
                        
                        }
                        else if (!m_bThirdPlayerConnected)
                        {
                            if (!m_bControllersConnected[i])
                            {
                               c.SetBlueID(i);
                               m_bThirdPlayerConnected = true;
                               m_bControllersConnected[i] = true;
                               m_Players[2].GetComponent<MainMenuDiverController>().Go();
                            }
                        
                        }
                        else if (!m_bFourthPlayerConnected)
                        {
                            if (!m_bControllersConnected[i])
                            {
                                c.SetYellowID(i);
                                m_bFourthPlayerConnected = true;
                                m_bControllersConnected[i] = true;
                                m_Players[3].GetComponent<MainMenuDiverController>().Go();
                            }
                        }
                    }
                }
            }
        }
    }

    private void Click(Vector2 pos)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, 500))
        {
            if (hit.transform.gameObject.tag == "MenuStart")
            {
                m_bClicked = true;
                //SceneManager.LoadScene("OfficialBuild 15 x 21");
            }
        }
    }

    private Vector3 Bezier(Vector3 v3Start, Vector3 v3End, Vector3 v3Control1, Vector3 v3Control2, float fT)
    {
        Vector3 L1 = Vector3.Lerp(v3Start,v3Control1,fT);
        Vector3 L2 = Vector3.Lerp(v3Control1, v3Control2, fT);
        Vector3 L3 = Vector3.Lerp(v3Control2, v3End, fT);
        Vector3 L4 = Vector3.Lerp(L1,L2,fT);
        Vector3 L5 = Vector3.Lerp(L2, L3, fT);

        return (Vector3.Lerp(L4,L5,fT));



    }

    
}


