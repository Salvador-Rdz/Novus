using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

//El objeto al que le queremos poner este script debe contar con Rigidbody2D y con un script de seeker
[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour {

	public Transform target;
    public float direction = 1;
    //Cuantas veces por segundo se actualiza el camino de la IA
    public float updateRate = 2f;
	
	private Seeker seeker;
	private Rigidbody2D rb;
	
	//El camino calculado
	public Path path;
	
	//La velocidad por segundo de la IA
	public float speed = 300f;
	public ForceMode2D fMode;
	
	[HideInInspector] //No queremos que esto aparezca en el inspector
	public bool pathIsEnded = false;
	
	private bool searchingForPlayer = false;
	
	//la distancia maxima para que la IA se mueva de un punto a otro
	public float nextWaypointDistance = 3f;
	
	//El punto hacia el que se mueve actualmente
	private int currentWaypoint = 0;
	
	void Start () {
		
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		
		if(target == null) { //Si no se encuentra un jugador
				
				if(!searchingForPlayer) { //Si no esta actualmente buscando al jugador
					searchingForPlayer = true;
					StartCoroutine(SearchForPlayer());
				}
				return;
		}
		
		//Inicia un nuevo camino hacia el objetivo (target) y devuelve el resultado al metodo OnPathComplete
		seeker.StartPath (transform.position, target.position, OnPathComplete);
		
		StartCoroutine (UpdatePath());
		
	}
	
	IEnumerator SearchForPlayer () { //Funcion de busqueda del jugador
		
		//Buscamos al objecto que tenga la etiqueta "Player"
		GameObject sResult = GameObject.FindGameObjectWithTag ("Player");
		
		if(sResult == null) { //Si el resultado de la busqueda fue nulo
			
			yield return new WaitForSeconds (0.5f); //Espera 0.5 segundos y despues busca de nuevo
			StartCoroutine (SearchForPlayer());
		
		} else { //Si se logro encontrar al jugador
			
			target = sResult.transform; //El "target" de la IA se vuelve el resultado de la busqueda
			searchingForPlayer = false; //Y dejamos de buscar al jugador
			StartCoroutine (UpdatePath());
			
			yield return false;
			
		}
	}
	
	IEnumerator UpdatePath () {
		if(target == null) { //Si no se encuentra un jugador
				
				if(!searchingForPlayer) { //Si no esta actualmente buscando al jugador
					searchingForPlayer = true;
					StartCoroutine(SearchForPlayer());
				}
				yield return false;
		}
		
		seeker.StartPath (transform.position, target.position, OnPathComplete);
		
		yield return new WaitForSeconds (1f/updateRate);
		StartCoroutine (UpdatePath());
		
	}
	
	public void OnPathComplete (Path p) {
		
		//Debug.Log ("Tenemos un camino, ¿contiene errores?" + p.error);
		if(!p.error) {
			path = p;
			currentWaypoint = 0;
			
		}
		
		
	}
	
	void FixedUpdate () {
		
		if(target == null) { //Si no se encuentra un jugador
				
				if(!searchingForPlayer) { //Si no esta actualmente buscando al jugador
					searchingForPlayer = true;
					StartCoroutine(SearchForPlayer());
				}
				return;
		}
		
		//TODO: hacer que el enemigo este siempre viendo hacia el jugador
		
		if(path == null)
			return;
		
		//Si el punto (waypoint) actual es mayor o igual al ultimo punto (waypoint) quiere decir que hemos llegado al final del camino
		if(currentWaypoint >= path.vectorPath.Count) {
			
			//Si ya se habia indicado el fin del camino entonces solo retorna
			if(pathIsEnded)
				return;
			//Si no se habia indicado que se llego al final del camino
			//Debug.Log ("Se ha llegado al final del camino."); //Lo marcamos en el debug
			pathIsEnded = true; //Se indica que hemos llegado al final del camino y luego retorna
			return;
		}
		pathIsEnded = false; //Si el condicional anterior no se cumple quiere decir que aun no llegamos al final del camino
		
		//Calculamos la direccion del siguiente punto (waypoint)
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		
		dir *= speed * Time.fixedDeltaTime;
		
		//Esto hace que se mueva la IA
		rb.AddForce(dir, fMode); 
		
		float dist = Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]);
		
		if(dist < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
        if (target.position.x > transform.position.x)
        {
            //face right
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (target.position.x < transform.position.x)
        {
            //face left
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
	}
    void Flip()
    {
        print("flipping!");
        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;

    }
}
