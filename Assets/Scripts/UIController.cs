using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject m_goP1Score;
    [SerializeField] GameObject m_goP2Score;
    [SerializeField] GameObject m_goP3Score;
    [SerializeField] GameObject m_goP4Score;
    [SerializeField] GameObject m_goTimer;
    [SerializeField] GameObject m_goStartAndEndDisplay;

    public float m_fInitialTimeSeconds = 180;
    private float m_fCurrentTime;
    private int m_nTimeToDisplay;

    [HideInInspector] public bool m_bGameEnded = true;

    public int m_nP1Score { get; set; } = 0;
    public int m_nP2Score { get; set; } = 0;
    public int m_nP3Score { get; set; } = 0;
    public int m_nP4Score { get; set; } = 0;

    void Start()
    {
        m_bGameEnded = true;
        UpdateScore();
        m_goTimer.GetComponent<Text>().text = m_fInitialTimeSeconds.ToString();

        m_fCurrentTime = m_fInitialTimeSeconds;
        m_nTimeToDisplay = (int)m_fInitialTimeSeconds;

        StartCoroutine(StartGameCountdown(3));
    }

    // Updates all player's score counters
    public void UpdateScore()
    {
        if (!m_bGameEnded)
        {
            m_goP1Score.GetComponent<Text>().text = "Player 1: " + m_nP1Score.ToString();
            m_goP2Score.GetComponent<Text>().text = "Player 2: " + m_nP2Score.ToString();
            m_goP3Score.GetComponent<Text>().text = "Player 3: " + m_nP3Score.ToString();
            m_goP4Score.GetComponent<Text>().text = "Player 4: " + m_nP4Score.ToString();
        }
    }

    IEnumerator StartGameCountdown(int seconds)
    {
        int count = seconds;

        while (count > -1)
        {
            switch (count)
            {
                case 3:
                case 2:
                case 1:
                    m_goStartAndEndDisplay.GetComponent<Text>().text = count.ToString();
                    break;
                case 0:
                    m_goStartAndEndDisplay.GetComponent<Text>().text = "Get Kraken!";
                    StartGame();
                    break;
                default:
                    Debug.LogError("The start countdown timer is greater than 3.");
                    break;
            }

            yield return new WaitForSeconds(1);
            count--;
        }

        m_goStartAndEndDisplay.GetComponent<Text>().text = "";
    }

    private void StartGame()
    {
        m_bGameEnded = false;
    }

    private IEnumerator ShowFinalScore(List<int> winningIDs)
    {
        bool coroutineDone = false;

        while (!coroutineDone)
        {
            if (winningIDs.Count == 1)
            {
                m_goStartAndEndDisplay.GetComponent<Text>().text = "Player " + winningIDs[0] + " Wins!";
            }
            else
            {
                m_goStartAndEndDisplay.GetComponent<Text>().text = "It's a tie!";
            }
            
            coroutineDone = true;
            yield return new WaitForSeconds(3);
        }

        SceneManager.LoadScene("MainMenu");
    }

    private void EndGame()
    {
        m_bGameEnded = true;
        List<int> winningIDs = FindWinners();
        StartCoroutine(ShowFinalScore(winningIDs));
    }

    // Who has the highest score? Allow ties to happen
    private List<int> FindWinners()
    {
        // I'll be using the index + 1 to get the player IDs
        // Not an optimal way of doing this, but the IDs should never change
        List<int> playerScores = new List<int>();
        playerScores.Add(m_nP1Score);
        playerScores.Add(m_nP2Score);
        playerScores.Add(m_nP3Score);
        playerScores.Add(m_nP4Score);

        // Get the highest score
        int winningScore = 0;
        int winningID = 0;
        for (int i = 0; i < playerScores.Count; i++)
        {
            if (playerScores[i] > winningScore)
            {
                winningScore = playerScores[i];
                winningID = i + 1;
            }
        }

        // Check if there's more than one winner
        List<int> duplicates = new List<int>();
        foreach (int score in playerScores)
        {
            if (score == winningScore) duplicates.Add(score);
        }

        List<int> winningIDs = new List<int>();
        if (duplicates.Count > 1)
        {
            winningIDs = duplicates;
        }
        else
        {
            winningIDs.Add(winningID);
        }

        return winningIDs;
    }

    private void Update()
    {
        // If game has ended, load the scene
        if (m_fCurrentTime < 0 && !m_bGameEnded)
        {
            EndGame();
        }
        // Otherwise, count down the timer
        else if (m_fCurrentTime > 0 && !m_bGameEnded)
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
