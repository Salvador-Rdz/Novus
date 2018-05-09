using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

	[System.Serializable]
	//Estas son las stats del enemigo, la "salud", entre otras cosas
    public class EnemyStats { 
        public int maxHealth = 100;
		
		private int _curHealth;
		public int curHealth {
			
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp (value, 0, maxHealth); }
			
		}
		public int damage = 40;
		
		public void Init () {
			
			curHealth = maxHealth;
		}
    }

    //Instanciamos la clase EnemyStats, guardandolo en la variable "stats"
    public EnemyStats stats = new EnemyStats();
	
	public Transform deathParticles;
	
	public float shakeAmt = 0.1f;
	public float shakeLength = 0.1f;

    public string deathSoundName = "Explosion";
	
	[Header("Opcional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;
	
	
	void Start () {
		
		stats.Init();
		
		if(statusIndicator != null) {
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
			
		}

        GameMaster.gm.onToggeUpgradeMenu += OnUpgradeMenuToggle;

		if (deathParticles == null) {
			Debug.LogError ("No hay partiuclas de muerte (deathParticles) referenciadas en el enemigo");
		}
	}

    void OnUpgradeMenuToggle(bool active) {
        //Aqui se maneja todo lo que debe suceder cuando se activa el menu de mejoras
        GetComponent<EnemyAI>().enabled = !active;
    }


    //Funcion en la que el enemigo recive daño
    public void DamageEnemy(int damage) { 

        stats.curHealth -= damage; //El valor calculado se le resta a la "salud" del enemigo
        
        //Si la vida del enemigo es menor o igual a "0" entonces el enemigo muere
        if(stats.curHealth <= 0) {
            GameMaster.KillEnemy(this); //Se llama la funcion KillEnemy del GameMaster para destruir al enemigo
        }
		if(statusIndicator != null) {
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
			
		}
    }
	
	void OnCollisionEnter2D (Collision2D _colInfo) {
		Player _player = _colInfo.collider.GetComponent<Player>();
		
		if(_player != null) {
			_player.DamagePlayer(stats.damage);
			DamageEnemy(9999);
		}
	}

    void OnDestroy() {
        GameMaster.gm.onToggeUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
