using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Intro,
    Playing,
    Dead
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameState state = GameState.Intro;

    public float playStartTime;
    public int lives = 3;
    public bool isInvincible = false;

    [Header("References")]
    
    public GameObject introUI;
    public GameObject deadUI;
    public GameObject enemySpawner;
    public GameObject foodSpawner;
    public GameObject goldSpawner;
    
    public PlayerController playerScript;
    
    public TMP_Text scoreText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        introUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.Playing)
        {
            scoreText.text = "Score: " + Mathf.FloorToInt(CalculateScore());
        }
        else if (state == GameState.Dead)
        {
            scoreText.text = "High Score: " + GetHighScore();
        }
        
        if (state == GameState.Intro && Input.GetKeyDown(KeyCode.Space))
        {
            state = GameState.Playing;
            introUI.SetActive(false);
            enemySpawner.SetActive(true);
            foodSpawner.SetActive(true);
            goldSpawner.SetActive(true);
            playStartTime = Time.time;
        }

        if (state == GameState.Playing && lives == 0)
        {
            playerScript.KillPlayer();
            enemySpawner.SetActive(false);
            foodSpawner.SetActive(false);
            goldSpawner.SetActive(false);
            deadUI.SetActive(true);
            SaveHighScore();
            state = GameState.Dead;
        }

        if (state == GameState.Dead && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("main");
        }
    }

    float CalculateScore()
    {
        return Time.time - playStartTime;
    }

    void SaveHighScore()
    {
        int score = Mathf.FloorToInt(CalculateScore());
        int currentHighScore = PlayerPrefs.GetInt("HighScore");
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    public float CalculateGameSpeed()
    {
        if (state != GameState.Playing)
        {
            return 5f;
        }

        float speed = 8f + (0.5f * Mathf.Floor(CalculateScore() / 10f));
        return Mathf.Min(speed, 20f);
    }
}
