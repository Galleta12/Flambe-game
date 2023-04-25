using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelectMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    
    public void PlayGroundBoss()
    {
        SceneManager.LoadScene(3);
    }
    
    
    public void PlayWindBoss()
    {
        SceneManager.LoadScene(4);
    }

    public void PlayWaterBoss()
    {
        SceneManager.LoadScene(5);
    }

  

    public void GoBack()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
