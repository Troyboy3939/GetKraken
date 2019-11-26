using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CursorState : MonoBehaviour
{
    [SerializeField] private Pointer m_Pointer = null;
    [SerializeField] private UIController m_UIController = null;
    [SerializeField] private float m_fMouseSpeed = 0;
    [SerializeField] private Texture m_tCursorImage = null;
    [SerializeField] private bool m_bIsCursorVisible = false;
    [SerializeField] private GameObject m_ResumeButton = null;
    [SerializeField] private GameObject m_QuitButton = null;
    private Vector2 m_v2CursorPosition;
    
    void Start()
    {
        m_v2CursorPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    private void Update()
    {
        if (XCI.IsPluggedIn(1) && XCI.GetButtonDown(XboxButton.A, XboxController.First))
        {
            Click(m_v2CursorPosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Click(Input.mousePosition);
        }
    }

    private void OnGUI()
    {
        if (m_bIsCursorVisible)
        {
            // Allow a joystick to move the mouse cursor if a controller is connected
            if (XCI.IsPluggedIn(1))
            {
                Cursor.visible = false;

                float h = (m_fMouseSpeed * XCI.GetAxis(XboxAxis.LeftStickX, XboxController.All));
                float v = (m_fMouseSpeed * XCI.GetAxis(XboxAxis.LeftStickY, XboxController.All));

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
    }

    public void ShowCursor()
    {
        m_bIsCursorVisible = true;
    }

    public void HideCursor()
    {
        m_bIsCursorVisible = false;
    }

    private void Click(Vector2 pos)
    {
        // As a rule of thumb, use raycasts in the Update method, not FixedUpdate. Otherwise you might get some unresponsive behaviour.
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, 500))
        {
            if (m_Pointer != null)
            {
                if (hit.transform.gameObject.tag == "MenuStart" && !m_Pointer.m_bControlsScreen)
                {
                    m_Pointer.m_bClicked = true;
                    m_Pointer.HideUI();
                    m_Pointer.m_BackIndicator.SetActive(true);
                }
                else if (hit.transform.gameObject.tag == "MenuControls")
                {
                    m_Pointer.m_ControlsImage.SetActive(true);
                    m_Pointer.m_bControlsScreen = true;
                    m_Pointer.HideUI();
                    m_Pointer.m_BackIndicator.SetActive(true);
                }
                else if (hit.transform.gameObject.tag == "MenuCredits")
                {
                    StartCoroutine(m_Pointer.fc.FadeOutToScene("Credits"));
                }
                else if (hit.transform.gameObject.tag == "MenuQuit")
                {
                    StartCoroutine(m_Pointer.fc.FadeOutAndQuit());
                }
            }
        }

        if (m_UIController != null)
        {
            Rect resumeButton = RectTransformToScreenSpace(m_ResumeButton.GetComponent<RectTransform>());
            Rect quitButton = RectTransformToScreenSpace(m_QuitButton.GetComponent<RectTransform>());

            // If resume button is clicked with controller cursor
            if (resumeButton.Contains(pos))  // probably should be converted to GUI space or whatever
            {
                m_UIController.ResumeGame();
            }

            // If quit button is clicked with controller cursor
            if (quitButton.Contains(pos))
            {
                m_UIController.QuitGame();
            }
        }
    }

    private Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect((Vector2)transform.position - (size * 0.5f), size);
    }
}
