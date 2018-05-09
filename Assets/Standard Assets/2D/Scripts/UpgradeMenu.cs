//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeMenu : MonoBehaviour {

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private float healthMultiplier = 1.3f;

    [SerializeField]
    private float movementSpeedMultiplier = 1.3f;

    private PlayerStats stats;
    public Text question1;
    public Text question2;

    public string[] questions = { "" };
    public string[] answers = {""};
    void OnEnable () {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues () {
        healthText.text = "Salud: " + stats.maxHealth.ToString();
        speedText.text = "Velocidad actual: " + stats.movementSpeed.ToString();
    }

    public void UpgradeHealth() {
        stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);
        UpdateValues();
    }

    public void UpgradeSpeed()
    {
        stats.movementSpeed = Mathf.Round(stats.movementSpeed * movementSpeedMultiplier);
        UpdateValues();
    }

    public void WrongAnswer() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
