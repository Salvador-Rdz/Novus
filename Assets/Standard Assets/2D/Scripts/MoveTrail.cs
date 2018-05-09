using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour {
	
	public int moveSpeed = 230;
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed); //Esto hace que se mueva el prefab es decir la "bala"
		Destroy (gameObject, 1); //El objeto se autodestruye luego de un segundo, para que no se llene la jerarquia
	}
}
