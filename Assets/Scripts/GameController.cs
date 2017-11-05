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
    GameObject menuCamera;
    [SerializeField]
    GameObject levelCamera;
    [SerializeField]
    GameObject ingredientSpawner;
    [SerializeField]
    int buffScoreVariation = 2;
    [SerializeField]
    int ingredientScoreVariation = 5;

    // Game variables
    int score = 0;

    private bool _isPaused = false;
    private bool _isGameOver = false;
    private GameObject _player;

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
        _isPaused = true;
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (_isGameOver)
        {
            if (_player && _player.activeInHierarchy)
            {
                ingredientSpawner.SetActive(false);
                _player.SetActive(false);
            }

            if (!_isPaused)
            {
                TogglePause();
            }
        }
        else if (Input.GetKey(KeyCode.Escape) && !_isPaused)
        {
            TogglePause();
        }
    }

    public void Play()
    {
        TogglePause(_isPaused);
        if (!_isPaused)
        {
            score = 0;
            StartCoroutine(StartLoadCoutdown());
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

    public void TogglePause(bool toggleCamera = false)
    {
        _isPaused = !_isPaused;
        menu.SetActive(_isPaused);

        if (toggleCamera)
        {
            Debug.Log("Toggle camera");

            menuCamera.SetActive(_isPaused);
            levelCamera.SetActive(!_isPaused);
        }

        Spawner spawner = ingredientSpawner.GetComponent<Spawner>();

        if (spawner)
        {
            if (_isPaused)
            {
                spawner.StopSpawning();
            }
            else
            {
                spawner.StartSpawning();
            }
        }
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
        Spawner spawner = ingredientSpawner.GetComponent<Spawner>();

        yield return new WaitForSeconds(3f);

        if (spawner)
        {
            spawner.SetPlayer(_player);
            spawner.StartSpawning();
        }

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
