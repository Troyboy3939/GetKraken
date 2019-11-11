using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    private FadeController fc;

    private void Start()
    {
        fc = GetComponent<FadeController>();
        StartCoroutine(fc.FadeIn());
    }

    void Update()
    {
        if (XCI.GetButtonDown(XboxButton.B, XboxController.All))
        {
            StartCoroutine(fc.FadeOutToScene("MainMenu"));
        }
    }
}
