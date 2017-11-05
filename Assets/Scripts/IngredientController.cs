using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour {
	// Look at the camera
    public void SetFocusPoint(GameObject focusPoint)
    {
        transform.LookAt(focusPoint.transform.position, Vector3.up);
    }
}
