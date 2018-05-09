using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds; //Declaracion del array que contendra los objetos de fondo
    private float[] paralaxScales;  //La proporcion en la que se van a mover los fondos con respecto a la camara
    public float smoothing = 1;        //Que tan sensible sera el "parallaxing" (Siempre debe ser mayor a 0)

    private Transform cam;           //hace referencia al transform de la camara principal
    private Vector3 previousCamPos;  //Almacena la posicion de la camara en el frame anterior


    //Se llama antes del Start() es bueno para hacer referencias
    void Awake () {
        cam = Camera.main.transform;  //referencia a la camara principal
        
    }

    // Use this for initialization
    void Start () {
        previousCamPos = cam.position;  //Se guarda la posicion que tenia la camara en el frame anterior

        //Esto es para asignar la proporcion de parallax correspondiente a cada objeto de fondo
        paralaxScales = new float[backgrounds.Length]; 

        for (int i = 0; i < backgrounds.Length; i++) { 
            paralaxScales[i] = backgrounds[i].position.z * -1; //Se asigna en base a su posicion en el eje z
        }
		
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < backgrounds.Length; i++) {
            //El parallax es opuesto al movimiento de la camara
            float parallax = (previousCamPos.x - cam.position.x) * paralaxScales[i];

            //establece una posicion "target" en x que es la posicion actual + el parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //crear una posicion "target" que es la posicion actual del background junto con la posicion "target" de x
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //desvanecimiento entre la posicion actual y la poscion "target" usando "lerp"
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime); //Time.deltaTime convierte frames a segundos
        }
        //almacenar la posicion de la camra al final del frame
        previousCamPos = cam.position;
		
	}
}
