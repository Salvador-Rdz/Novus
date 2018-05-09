using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

	public int rotationOffset = 90; 

	// Update is called once per frame
	void Update () {
		// Restamos la pocision del jugador de la pocision del mouse 
		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		difference.Normalize ();		// Normalizando el vector. Es decir que la suma del vector sera igual a 1 

		float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;	// Hallar el angulo en "grados"
		transform.rotation = Quaternion.Euler (0f, 0f, rotZ + rotationOffset);
	}
}