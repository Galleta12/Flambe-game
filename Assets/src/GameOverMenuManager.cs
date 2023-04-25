using TMPro;
using UnityEngine;

public class GameOverMenuManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;

    public void DisplayText(string text)
    {
        textField.SetText(text);
        gameObject.SetActive(true);
    }
}
