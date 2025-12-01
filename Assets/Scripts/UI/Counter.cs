using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Counter : MonoBehaviour
{
    public Text remainingEnemies;
    public Text totalEnemies;
    public GameObject winOverlay;

    private int enemies;

    void Start()
    {
        // get the number of enemy prefabs in the scene
        enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        remainingEnemies.text = enemies.ToString();
        totalEnemies.text = enemies.ToString();
    }

    // public function you can call from other scripts
    public void EnemyDown()
    {
        enemies -= 1;
        remainingEnemies.text = enemies.ToString();

        // win state
        if (enemies <= 0)
        {
            StartCoroutine(OpenWinOverlay());
        }
    }

    IEnumerator OpenWinOverlay()
    {
        winOverlay.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        
        yield return new WaitForSeconds(3f); // delay 3 seconds

        winOverlay.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
