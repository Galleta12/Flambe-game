using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{ 
    [SerializeField] private GameObject menu;

   
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        Time.timeScale = Time.timeScale >= 1f ? 0f : 1f;
        menu.SetActive(!menu.activeSelf);
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
