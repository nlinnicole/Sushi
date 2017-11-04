using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject[] ingredients;
    public float spawnTime = 0.3f;
    public float delayTime = 2f;
    public Transform[] spawnPoints;

    private System.Random rnd = new System.Random();

    List<GameObject> instantiated = new List<GameObject>();

    void Start() {
            InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn() {
        Debug.Log("Spawning");
        //pick random ingredient and location to spawn
        int randomIndex = Random.Range(0, ingredients.Length);
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        //Instantiate ingredient
        GameObject ingredient = Instantiate(ingredients[randomIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

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
        if (instantiated.Count > 3) {
            Debug.Log("Too crowded! Destroying");
            for (int i = 0; i < instantiated.Count - 3; ++i) {
                Destroy(instantiated[i]);
            }
        }
        

    }
}
