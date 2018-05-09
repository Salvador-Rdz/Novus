using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public GameObject projectile;
    public Vector2 velocity;
    bool canShoot = true; 
    public Vector2 offset = new Vector2(0.4f, 0.1f);
    public float cooldown = 1f;
	


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        //Se presiona la tecla "z" para disparar, tambien comprueba que sea posible disparar (usando el bool canShoot)
		if (Input.GetKeyDown(KeyCode.Z) && canShoot) {
		
          //Instanciamos la creacion del proyectil, es relativo a la posicion del jugador y se multiplica por -1 si el jugador esta viendo hacia el otro lado
           GameObject go = (GameObject) Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.identity);

            //Esto le da velocidad de movimiento al proyectil
           go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);

           StartCoroutine(CanShoot()); //Se llama la subrutina CanShoot que es el cooldown

           GetComponent<Animator>().SetTrigger("shoot"); 

        }

    }

    //Subrutina para el "cooldown" luego de cada disparo
    IEnumerator  CanShoot() {
        canShoot = false;
        yield return new WaitForSeconds(cooldown); //Espera  el tiempo que se le asigne antes de disparar de nuevo (por default es 1 segundo)
        canShoot = true;
    }
}
