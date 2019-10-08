using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject m_goP1Score;
    [SerializeField] GameObject m_goP2Score;
    [SerializeField] GameObject m_goTimer;

    public float m_fInitialTimeSeconds = 180;
    private float m_fCurrentTime;
    private int m_nTimeToDisplay;

    public int m_nP1Score { get; set; } = 0;
    public int m_nP2Score { get; set; } = 0;

    void Start()
    {
        m_goP1Score.GetComponent<Text>().text = "Player 1: " + m_nP1Score.ToString();
        m_goP2Score.GetComponent<Text>().text = "Player 2: " + m_nP2Score.ToString();
        m_goTimer.GetComponent<Text>().text = m_fInitialTimeSeconds.ToString();

        m_fCurrentTime = m_fInitialTimeSeconds;
        m_nTimeToDisplay = (int)m_fInitialTimeSeconds;
    }

    // Updates all player's score counters
    public void UpdateScore()
    {
        m_goP1Score.GetComponent<Text>().text = "Player 1: " + m_nP1Score.ToString();
        m_goP2Score.GetComponent<Text>().text = "Player 2: " + m_nP2Score.ToString();
    }

    private void Update()
    {
        if (m_fCurrentTime < 0)
        {
            Debug.Log("TIME IS UP!");
        }
        else
        {
            m_fCurrentTime -= Time.deltaTime;
            m_nTimeToDisplay = Mathf.CeilToInt(m_fCurrentTime);
            m_goTimer.GetComponent<Text>().text = m_nTimeToDisplay.ToString();
        }
    }
}
