using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject m_goP1Score;
    [SerializeField] GameObject m_goP2Score;
    [SerializeField] GameObject m_goP3Score;
    [SerializeField] GameObject m_goP4Score;
    [SerializeField] GameObject m_goTimer;

    public float m_fInitialTimeSeconds = 180;
    private float m_fCurrentTime;
    private int m_nTimeToDisplay;
    [HideInInspector] public bool m_bGameEnded = false;

    public int m_nP1Score { get; set; } = 0;
    public int m_nP2Score { get; set; } = 0;
    public int m_nP3Score { get; set; } = 0;
    public int m_nP4Score { get; set; } = 0;

    void Start()
    {
        UpdateScore();
        m_goTimer.GetComponent<Text>().text = m_fInitialTimeSeconds.ToString();

        m_fCurrentTime = m_fInitialTimeSeconds;
        m_nTimeToDisplay = (int)m_fInitialTimeSeconds;
    }

    // Updates all player's score counters
    public void UpdateScore()
    {
        m_goP1Score.GetComponent<Text>().text = "Player 1: " + m_nP1Score.ToString();
        m_goP2Score.GetComponent<Text>().text = "Player 2: " + m_nP2Score.ToString();
        m_goP3Score.GetComponent<Text>().text = "Player 3: " + m_nP3Score.ToString();
        m_goP4Score.GetComponent<Text>().text = "Player 4: " + m_nP4Score.ToString();
    }

    private void Update()
    {
        if (m_fCurrentTime < 0 && !m_bGameEnded)
        {
            m_bGameEnded = true;
            SceneManager.LoadScene("TitleScreen01");
        }
        else if (m_fCurrentTime > 0)
        {
            m_fCurrentTime -= Time.deltaTime;
            m_nTimeToDisplay = Mathf.CeilToInt(m_fCurrentTime);
            m_goTimer.GetComponent<Text>().text = m_nTimeToDisplay.ToString();
        }
    }

    // trigger game over thing where the kraken kills each of the players one by one
    // from last place to second, first place player stays alive

    // add each player to a list, sorted by score
    // remove the winning player from the list
    // loop over the list every second (Time.deltaTime), kraken will kill the next player in the list

    // What should happen when players are tied? What happens if the winning player is killed just
    // before the time runs out?

    // consider using a coroutine for this
}
