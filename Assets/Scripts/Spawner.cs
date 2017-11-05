using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject focusPoint;

    GameObject player;

    public GameObject[] ingredients;
    public float spawnTime = 0.3f;
    public float delayTime = 2f;
    public Transform[] spawnPoints;

    private System.Random rnd = new System.Random();
    private GameController gameController;

    List<GameObject> instantiated = new List<GameObject>();
    Coroutine spawnCoroutine;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void OnDisable()
    {
        StopSpawning();
    }

    void Update()
    {
        if (!player)
        {
            Cleanup();
            gameObject.SetActive(false);
        }
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
        Cleanup();
    }

    public void StartSpawning()
    {
        if(!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            spawnCoroutine = StartCoroutine(Spawn());
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    void Cleanup()
    {
        instantiated.ForEach(el => Destroy(el));
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            //pick random ingredient and location to spawn
            int randomIndex = Random.Range(0, ingredients.Length);
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            //Instantiate ingredient
            GameObject ingredient = Instantiate(ingredients[randomIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            ingredient.GetComponentInChildren<IngredientController>().SetFocusPoint(focusPoint);

            //Add to list of instantiated ingredients
            instantiated.Add(ingredient);


            //Destroy random ingredient
            int rInst = rnd.Next(instantiated.Count);
            if (rnd.Next(2) == 0)
            {
                Destroy(instantiated[rInst], delayTime);
                instantiated.RemoveAt(rInst);
                Debug.Log("Destroying");
            }
            if (instantiated.Count > 3)
            {
                Debug.Log("Too crowded! Destroying");
                for (int i = 0; i < instantiated.Count - 3; ++i)
                {
                    Destroy(instantiated[i]);
                }
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
