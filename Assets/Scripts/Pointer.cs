using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XboxCtrlrInput;


public class Pointer : MonoBehaviour
{
    public bool m_bClicked = false;
    float m_fT = 0.0f;
    [SerializeField] float m_fMoveSpeed = 0.01f;
    [SerializeField] float m_fDisplacement = 10.0f;
    [SerializeField] string m_sz4PlayerLevel = "";
    [SerializeField] string m_sz3PlayerLevel = "";
    [SerializeField] string m_sz2PlayerLevel = "";
    [SerializeField] Vector3 m_v3InterpolationEnd = new Vector3(0, 0, 0);
    [SerializeField] GameObject m_Bezier1 = null;
    [SerializeField] GameObject m_Bezier2 = null;


    [SerializeField] GameObject m_Logo = null;
    [SerializeField] GameObject m_Start = null;
    [SerializeField] GameObject m_Controls = null;
    [SerializeField] GameObject m_Credits = null;
    [SerializeField] GameObject m_Quit = null;
    public GameObject m_ControlsImage = null;
    [SerializeField] GameObject m_AToConnect = null;
    [SerializeField] GameObject m_AToStart = null;
    public GameObject m_BackIndicator = null;

    bool m_bFirstControllerConnected = false;
    bool m_bFirstPlayerConnected = false;
    bool m_bSecondPlayerConnected = false;
    bool m_bThirdPlayerConnected = false;
    bool m_bFourthPlayerConnected = false;

    bool[] m_bControllersConnected = new bool[5] { true, false, false, false, false };
    [SerializeField] GameObject[] m_Players = new GameObject[4];

    [SerializeField] Collider m_cBarrelCollider;
    Vector3 m_Rot;
    [SerializeField] Vector3 m_RotationEnd;

    [SerializeField] float m_fLerpSpeedScale = 4;
    Vector3 m_v3Pos;
    bool m_bDone = false;

    [SerializeField] private float m_fLoadingDuration = 5;

    public bool m_bControlsScreen = false;

    [HideInInspector] public FadeController fc;

    [SerializeField] private bool m_bStartWithSpace = false;


    public GameObject ControlPanel;

    private void Start()
    {
        fc = GetComponent<FadeController>();

        // Disable the mouse cursor and use a software cursor
        //Cursor.visible = false;
        m_Rot = transform.rotation.eulerAngles;

        if (!m_ControlsImage) Debug.LogError("Pointer.cs: The Controls Image object is not in the correct field in the inspector.");

        if (m_ControlsImage.activeSelf) m_ControlsImage.SetActive(false);

        StartCoroutine(fc.FadeIn());
    }

    private void Update()
    {
        if (m_bClicked)
        {
            // "Go Back" by restarting the scene
            if (XCI.GetButtonDown(XboxButton.B, XboxController.All) || Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(fc.FadeOutToScene(SceneManager.GetActiveScene().name));
            }

            if (!m_bDone)
            {
                m_v3Pos = transform.position;
                m_bDone = true;
            }

            m_fT += m_fMoveSpeed * Time.deltaTime;
            if ((m_fT < 0.7))
            {
                Vector3 rot = Vector3.Lerp(m_Rot, m_RotationEnd, m_fT * m_fLerpSpeedScale * 1.3f);
                transform.position = Bezier(m_v3Pos, m_v3InterpolationEnd, m_Bezier1.transform.position, m_Bezier2.transform.position, m_fT * m_fLerpSpeedScale);
                transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
            }
            else
            {
                Calibration c = GameObject.FindGameObjectWithTag("Calibration").GetComponent<Calibration>();

                if (!m_AToConnect.activeSelf && !m_bFirstControllerConnected) m_AToConnect.SetActive(true);
                
                // Let people without controllers test the game
                if (Input.GetKeyDown(KeyCode.Space) && m_bStartWithSpace)
                {
                    StartCoroutine(fc.FadeOutToScene(m_sz4PlayerLevel));
                }
                else if (Input.GetKeyDown(KeyCode.Space) && !m_bStartWithSpace)
                {
                    Debug.Log("If you want to start the game with a keyboard, make sure 'Start With Space' is checked on the Pointer script component.");
                }

                //If player 1 presses A
                if (m_bFirstControllerConnected && XCI.GetButtonDown(XboxButton.A, XboxController.First))
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
                                StartCoroutine(ShowLoadingScreen(m_fLoadingDuration, m_sz4PlayerLevel));
                            }
                            else //fourth isn't connect, load three player level
                            {
                                StartCoroutine(ShowLoadingScreen(m_fLoadingDuration, m_sz3PlayerLevel));
                            }
                        }
                        else //if third hasn't connect load two player level
                        {
                            StartCoroutine(ShowLoadingScreen(m_fLoadingDuration, m_sz2PlayerLevel));
                        }
                    }
                }

                for (int i = 1; i < 5; i++)
                {
                    //Get whether the a button is presed for each controller
                    if (XCI.GetButtonDown(XboxButton.A, (XboxController)i))
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
                                m_AToStart.SetActive(true);
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

        if (XCI.GetButtonDown(XboxButton.B, XboxController.All) || Input.GetKeyDown(KeyCode.B))
        {
            if (m_bControlsScreen)
            {
                ShowUI();
                m_bControlsScreen = false;
                m_ControlsImage.SetActive(false);
                m_BackIndicator.SetActive(false);
            }
        }
    }

    public void HideUI()
    {
        if (m_Logo != null) m_Logo.SetActive(false);
        if (m_Start != null) m_Start.SetActive(false);
        if (m_Controls != null) m_Controls.SetActive(false);
        if (m_Credits != null) m_Credits.SetActive(false);
        if (m_Quit != null) m_Quit.SetActive(false);
    }

    public void ShowUI()
    {
        if (m_Logo != null) m_Logo.SetActive(true);
        if (m_Start != null) m_Start.SetActive(true);
        if (m_Controls != null) m_Controls.SetActive(true);
        if (m_Credits != null) m_Credits.SetActive(true);
        if (m_Quit != null) m_Quit.SetActive(true);
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

    private IEnumerator ShowLoadingScreen(float duration, string sceneName)
    {
        bool b = true;
        while (b)
        {
            b = false;
            m_AToConnect.SetActive(false);
            m_AToStart.SetActive(false);
            m_ControlsImage.SetActive(true);
            m_BackIndicator.SetActive(false);
            yield return new WaitForSeconds(duration);
        }
        StartCoroutine(fc.FadeOutToScene(sceneName));
    }


}


