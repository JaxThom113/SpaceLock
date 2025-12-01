using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text text;
    public GameObject loseOverlay;

    private int health = 100;
    private int enemyDamage = 5;

    void Start()
    {
        slider.maxValue = health;
        text.text = health.ToString();
    }

    // public functions so you can call them from other scripts
    public void DamagePlayer()
    {
        health -= enemyDamage;
        slider.value = health;
        text.text = health.ToString();
            
        // lose state
        if (health <= 0)
        {
            StartCoroutine(OpenLoseOverlay());
        }
    }

    IEnumerator OpenLoseOverlay()
    {
        loseOverlay.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        yield return new WaitForSeconds(3f); // delay 3 seconds

        loseOverlay.SetActive(false);
        SceneManager.LoadScene(1);
    }
}
