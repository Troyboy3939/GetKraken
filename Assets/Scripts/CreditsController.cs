using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    void Update()
    {
        if (XCI.GetButtonDown(XboxButton.B, XboxController.All))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
