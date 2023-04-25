using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Health playerHealth;
    [SerializeField]
    private Health enemyHealth;

    [SerializeField]
    private GameOverMenuManager gameOverMenu;

    private void Start()
    {
        playerHealth.OnDeath += OnPlayerDeath;
        enemyHealth.OnDeath += OnEnemyDeath;
    }

    private void OnPlayerDeath()
    {
        StartCoroutine(SwitchScenes("Game Over", 0));
    }

    private void OnEnemyDeath()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(
            nextSceneIndex < SceneManager.sceneCountInBuildSettings
            ? SwitchScenes("Level Complete", nextSceneIndex)
            : SwitchScenes("Game Complete", 0)
            );
    }

    private IEnumerator SwitchScenes(string displayText, int nextScene)
    {
        yield return new WaitForSeconds(2);
        gameOverMenu.DisplayText(displayText);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(nextScene);
    }
}
