using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    [SerializeField] private GameObject m_goFadeIn;
    [SerializeField] private GameObject m_goFadeOut;

    public IEnumerator FadeIn()
    {
        bool b = true;
        while (b)
        {
            m_goFadeIn.SetActive(true);
            m_goFadeIn.GetComponent<Animator>().Play("FadeIn");
            b = false;
            yield return new WaitForSeconds(1);
        }
        m_goFadeIn.SetActive(false);
    }

    public IEnumerator FadeOutToScene(string sceneName)
    {
        bool b = true;
        while (b)
        {
            m_goFadeOut.SetActive(true);
            m_goFadeOut.GetComponent<Animator>().Play("FadeOut");
            b = false;
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator FadeOutAndQuit()
    {
        bool b = true;
        while (b)
        {
            m_goFadeOut.SetActive(true);
            m_goFadeOut.GetComponent<Animator>().Play("FadeOut");
            b = false;
            yield return new WaitForSeconds(1);
        }
        Application.Quit();
    }
}
