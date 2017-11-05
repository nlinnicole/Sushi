using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour {
	// Look at the camera
    public void SetCamera(GameObject camera)
    {
        transform.LookAt(camera.transform.position, -Vector3.up);
    }
}
