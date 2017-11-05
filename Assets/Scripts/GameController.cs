using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameController();
            }

            return _instance;
        }
    }

    static GameController _instance = null;

    [SerializeField]
    int maxScore = 100;
    [SerializeField]
    float gameTime = 60f;

    // Game variables
    int score = 0;

    private bool _isPaused = false;
    private bool _isGameOver = false;

    void Awake()
    {
        if (_instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.anyKey)
        {
            // TODO : handle menu with arrows + ENTER
            if (_isPaused)
            {

            }
            else if (_isGameOver)
            {

            }
        }
    }

    public void LoadScene()
    {

    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void UpdateScore(int variation)
    {
        score = Mathf.RoundToInt(Mathf.Clamp(score + variation, 0f, maxScore));
    }

    IEnumerator StartLoadCoutdown()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(StartLevelCoutdown());
    }

    IEnumerator StartLevelCoutdown()
    {
        while (_isPaused)
        {
            yield return null;
        }

        yield return new WaitForSeconds(gameTime);
        _isGameOver = true;
    }
}
