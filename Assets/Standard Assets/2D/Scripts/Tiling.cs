using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))] //Este script siempre comprueba si esta adjunto a un sprite

public class Tiling : MonoBehaviour {

    public int offSetX = 2; //variable de la diferencia del final del "foreground" con respecto a la camara

    //Estos dos boolean son para comprobar si se tendran que hacer los calculos o no 
    //el "buddy" se refiere a crear otra instancia del "foreground" fuera de camara para dar la impresion de que es infinito
    public bool hasArightBuddy = false;
    public bool hasALeftbuddy = false;

    public bool reverseScale = false; //Se usa si el objeto no es un "tile"

    private float spriteWidth = 0f; // el ancho de nuestro elemento o textura
    private Camera cam;
    private Transform myTransform;

    void Awake() {
        cam = Camera.main;
        myTransform = transform;   
    }

    // Use this for initialization
    void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>(); //Aqui se manda a llamar al "Sprite Renderer"
        spriteWidth = sRenderer.sprite.bounds.size.x; //Esto checa el tamaño del sprite que se esta usando
	}
	
	// Update is called once per frame
	void Update () {
        //Este if es para checar si necesitamos hacer un "buddy" o no
        if (hasALeftbuddy == false || hasArightBuddy == false) {

            //calcular el alcance de la camara
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            //calcular la posicion en "x" donde la camara puede ver la orilla del sprite
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            //Checamos si es posible ver la orilla del Sprite y llamamos a MakeNewBuddy en caso de que si
            if (cam.transform.position.x >= edgeVisiblePositionRight - offSetX && hasArightBuddy == false) {

                MakeNewBuddy(1);
                hasArightBuddy = true;

            } else if (cam.transform.position.x <= edgeVisiblePositionLeft + offSetX && hasALeftbuddy == false) {

                MakeNewBuddy(-1);
                hasALeftbuddy = true;

            }
        }
		
	}
    void MakeNewBuddy (int rightOrLeft) {
        //Esto es para calcular la posicion del "buddy" que crearemos
        Vector3 newPostion = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        //Instanciando nuestro nuevo "buddy" y lo almacenamos en una variable
        Transform newBuddy = Instantiate(myTransform,newPostion,myTransform.rotation) as Transform;

        //Si el objeto no es "tilable" entonces invertimos su eje x para que no se vea tan mal
        if (reverseScale == true) {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z); 
        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftbuddy = true;
        }
        else {
            newBuddy.GetComponent<Tiling>().hasArightBuddy = true;
        }

    }
}
