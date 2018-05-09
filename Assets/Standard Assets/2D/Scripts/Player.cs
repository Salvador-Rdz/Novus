using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour {

    public int fallBoundary = -20; //Variable que determina cuando el jugador ha caido "fuera" del nivel (por default se toma como -20)

    [SerializeField]
	private StatusIndicator statusIndicator;

    private PlayerStats stats;


    void Start () {

        stats = PlayerStats.instance;

        stats.curHealth = stats.maxHealth;

		if(statusIndicator == null) {
			Debug.LogError ("No hay un indicador de status (StatusIndicator) referenciado en el jugador");
		} else {
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}

        GameMaster.gm.onToggeUpgradeMenu += OnUpgradeMenuToggle;

        InvokeRepeating("RegenHealth", 1f / stats.healthRegenRate, 1f / stats.healthRegenRate);

    }

    void OnUpgradeMenuToggle(bool active) {
        //Aqui se maneja todo lo que debe suceder cuando se activa el menu de mejoras
        GetComponent<Platformer2DUserControl>().enabled = !active;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if (_weapon != null)
            _weapon.enabled = !active;
    }

    void RegenHealth()
    {
        stats.curHealth += 1;
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    void Update() {
        //Si el jugador cae por debajo del limite establecido
        if (transform.position.y <= fallBoundary) {
            DamagePlayer (99999); //El jugador recibe daño suficiente para matarlo
        }
    }

      //Funcion en la que el jugador recive daño
    public void DamagePlayer(int damage) { 

        stats.curHealth -= damage; //El valor calculado se le resta a la "salud" del jugador
        
        //Si la vida del jugador es menor o igual a "0" entonces el jugador muere
        if(stats.curHealth <= 0) {
            GameMaster.KillPlayer(this); //Se llama la funcion KillPlayer del GameMaster para matar al jugador
        }
		statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    void OnDestroy() {
        GameMaster.gm.onToggeUpgradeMenu -= OnUpgradeMenuToggle;
    }
}