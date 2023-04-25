using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject levelSelectMenu;
    
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToLevelSelectMenu()
    {
        levelSelectMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
