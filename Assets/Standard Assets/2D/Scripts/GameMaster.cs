using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;
	
	[SerializeField]
	private int maxLives = 3;
	private static int _remainingLives;
	public static int RemainingLives {
		
		get { return _remainingLives; }
		
	}
	

    void Awake () {
        //Si no se tiene un "game master"
        if (gm == null) {
            //busca el objeto que tenga la etiqueta "GM" y lo convierte en el "game master"
            gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;
	//public Transform spawnPrefab;
	public string spawnSoundName;
	
	
	public CameraShake cameraShake;
	
	[SerializeField]
	private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggeUpgradeMenu;

    private AudioManager audioManager;
	
	void Start () {
		if (cameraShake == null) {
			Debug.LogError ("No hay un cameraShake referenciado en el GameMaster");
		}
		_remainingLives = maxLives; //Esto se usa para "reiniciar" el contador de vidas, una vez que el juego se reinicie luego de un "Game Over"
		
		audioManager = AudioManager.instance;
		if(audioManager == null) {
			Debug.LogError("No se encontro un AudioManager en la escena!!!!! Panico");
		}
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            ToggleUpgradeMenu();
        }
    }

    private void ToggleUpgradeMenu() {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        onToggeUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }

    public void EndGame () {
		
		Debug.Log("Game Over");
		gameOverUI.SetActive(true);
		
	}

    public IEnumerator RespawnPlayer() {
		//Llama al sonido de Respawn
		audioManager.PlaySound(spawnSoundName);
        
		//Espera un cierto tiempo antes de reaparecer al jugador (por default son 2 segundos)
        yield return new WaitForSeconds(spawnDelay);
        
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation); //Instanciamos de nuevo al jugador para "revivirlo"
        //Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation); //Particulas para efecto de reaparicion (aun no implementado)
    }

    public static void KillPlayer(Player player) {
        //Destruye el objeto de jugador
        Destroy(player.gameObject);
		_remainingLives -= 1;
		
		if(_remainingLives <= 0) {
			//Se acaba el juego
			gm.EndGame();
				
		} else {
			//llama la subrutina "RespawnPlayer" para revivirlo luego de morir
			gm.StartCoroutine(gm.RespawnPlayer());
		}
		
        
    }
	
	public static void KillEnemy(Enemy enemy) {
		gm._killEnemy(enemy);
	}
	
	public void _killEnemy (Enemy _enemy) {

        //Efecto de sonido
        audioManager.PlaySound(_enemy.deathSoundName);

        
        //Agregamos efecto de particulas
        Transform _clone = Instantiate (_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
		Destroy(_clone.gameObject, 5f);

        //Hacemos temblar la camara
        cameraShake.Shake (_enemy.shakeAmt, _enemy.shakeLength);
		Destroy(_enemy.gameObject);
	}
}
