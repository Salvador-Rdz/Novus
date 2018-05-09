using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	
	public float fireRate = 0;
	public int Damage = 10;
	public LayerMask whatToHit;
	public Transform BulletTrailPrefab;
	public Transform HitPrefab;
	public Transform MuzzleFlashPrefab;
	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;
	
	//Esto es para hacer que la camara se sacuda, para hacer que el arma luzca "mas potente"
    public float camShakeAmt = 0.05f; //comenta este codigo, el void Start  y el de la linea 121 si no quieres que se sacuda la camara
	public float camShakeLength = 0.1f; //O coloca en 0 los valores de camShakeAmt y camShakeLength
	CameraShake camShake;

    public string weaponShootSound = "DefaultShot";
	
	
	float timeToFire = 0;
	Transform firePoint;

    AudioManager audioManager;
	
	
	//Esto tambien es para hacer que se sacuda la camara
	void Start () {
		
		camShake = GameMaster.gm.GetComponent<CameraShake>();
		if (camShake == null)
			Debug.LogError("No se encontro el script de 'CameraShake' en el objeto de GameMaster");
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("No se encontro un AudioManager");
        }

    } 

	// Use this for initialization
	void Awake () {
		firePoint = transform.Find ("FirePoint");
		if (firePoint == null) { //Si no existe un "Fire point"
			Debug.LogError ("No existe un 'FirePoint' porque????"); //Se manda un mensaje de error.
			
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(fireRate == 0) { //Si el "fireRate" es igual a 0
			if(Input.GetButtonDown ("Fire1")){ //Entonces dispara 1 vez por cada que se presione el boton izquierdo del mouse.
				Shoot();
			}
			
		} else {
			if(Input.GetButton ("Fire1") && Time.time > timeToFire){ //Si el firerate no es 0 
				timeToFire = Time.time + 1/fireRate; //Disparara 'Firerate' veces por segundo
				Shoot();
			}
			
		}
		
	}
	void Shoot () {
		//Revisa la posicion del mouse para saber hacia donde va a disparar
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		//Esto calcula la trajectoria de la "bala"
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
		
		Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition) * 100, Color.yellow); //Funcion de Debug que dibuja la trajectoria de la bala (el jugador no debe ver esto)
		if (hit.collider != null) {
			Debug.DrawLine (firePointPosition, hit.point, Color.red); 
			Enemy enemy = hit.collider.GetComponent<Enemy>();
			if (enemy != null) {
				enemy.DamageEnemy (Damage);
				//Debug.Log ("Golpeamos un " + hit.collider.name + " y causamos " + Damage + " unidades de daño."); 
			}
		}
		if (Time.time >= timeToSpawnEffect) { //Esto es para que no se acumulen demasiados objetos, por default solo puede haber 10 balas a la vez
			
			Vector3 hitPos;
			Vector3 hitNormal;
			
			if(hit.collider == null) {
				hitPos = (mousePosition - firePointPosition) * 30;
				hitNormal = new Vector3 (9999,9999,9999);
		    } else {
				hitPos = hit.point;
				hitNormal = hit.normal;
			}	
			
			Effect (hitPos, hitNormal);
			timeToSpawnEffect = Time.time + 1/effectSpawnRate;
		}
		
	}
	
	void Effect (Vector3 hitPos, Vector3 hitNormal) { //Funcion para dibujar la "bala"
		Transform trail = Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform; //Instanciamos el objeto de la "bala" y lo almacenamos en una variable
		LineRenderer lr = trail.GetComponent<LineRenderer>();
		
		if (lr != null) {
			//Aqui se asignaran las posiciones
			lr.SetPosition (0, firePoint.position);
			lr.SetPosition (1, hitPos);
			
		}
		Destroy (trail.gameObject, 0.02f);
		
		if(hitNormal != new Vector3 (9999,9999,9999)){
			Transform hitParticle = Instantiate (HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
			Destroy (hitParticle.gameObject, 1f);
			
		}
		
		
		
		Transform clone = Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform; //Instanciamos el destello del arma y lo almacenamos en una variable llamada "clone"
		clone.parent = firePoint; //Emparentamos la variable "clone" con la variable "firepoint"
		float size = Random.Range (0.6f, 0.9f); //El tamaño del destello del arma va a variar de manera aleatoria
		clone.localScale = new Vector3 (size, size, 0); //Le asignamos el tamaño aleatorio al destello 
		Destroy (clone.gameObject, 0.02f); //Destruye el objeto despues de un corto tiempo
		
		//Esto es para agitar la camara
		camShake.Shake (camShakeAmt, camShakeLength);

        //Esto reproduce el sonido del disparo
        audioManager.PlaySound(weaponShootSound);

	}
	
}
