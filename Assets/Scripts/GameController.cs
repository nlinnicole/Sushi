using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static GameController instance = null;

    [SerializeField]
    int maxScore = 100;
    [SerializeField]
    float gameTime = 60f;
    [SerializeField]
    GameObject menu;
    [SerializeField]
    int buffScoreVariation = 2;
    [SerializeField]
    int ingredientScoreVariation = 5;

    // Game variables
    int score = 0;

    private bool _isPaused = false;
    private bool _isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("Start");
        StartCoroutine(StartLoadCoutdown());
    }

    void Update()
    {
        if (_isGameOver)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player)
            {
                Destroy(player);
            }

            GameObject sceneMenu = GameObject.FindWithTag("Menu");
            if (sceneMenu)
            {
                sceneMenu.SetActive(true);
            }
            else
            {
                Instantiate(menu, transform.position, transform.rotation);
            }
        }
    }

    public void LoadSceneByIndex(int sceneIndex = -1)
    {
        if (sceneIndex >= 0)
        {
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        }
    }

    public void LoadSceneByName(string sceneName = null)
    {
        if (sceneName != null)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }

    public void Play()
    {
        if (_isPaused)
        {
            TogglePause();
        }
        else
        {
            LoadSceneByIndex(1);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void TogglePause()
    {
        _isPaused = true;
    }

    public void UpdateScoreWithTag(string itemTag)
    {
        if (itemTag == null || itemTag.Length == 0)
        {
            return;
        }

        if (itemTag == "Ingredient")
        {
            UpdateScore(ingredientScoreVariation);
        }
        else if (itemTag.EndsWith("Buff"))
        {
            UpdateScore(buffScoreVariation);
        }
    }

    void UpdateScore(int variation)
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
