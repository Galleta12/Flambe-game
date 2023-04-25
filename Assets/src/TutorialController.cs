using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private List<Transform> _children;
    private int _currentChildIndex;

    private bool _aPressed;
    private bool _dPressed;
    private bool _spacePressed;
    private bool _shiftPressed;
    private bool _leftMousePressed;
    private bool _rightMousePressed;

    private void Start()
    {
        _children = new List<Transform>();
        for (var i = 0; i < transform.childCount; i++)
        {
            _children.Add(transform.GetChild(i));
        }

        _children[_currentChildIndex].gameObject.SetActive(true);
    }

    private void Update()
    {
        var pos = player.position;
        pos.y += 0.5f;
        transform.position = pos;

        if (Input.GetKey(KeyCode.A)) _aPressed = true;
        if (Input.GetKey(KeyCode.D)) _dPressed = true;
        if (Input.GetKey(KeyCode.Space)) _spacePressed = true;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) _shiftPressed = true;
        if (Input.GetMouseButton(0)) _leftMousePressed = true;
        if (Input.GetMouseButton(1)) _rightMousePressed = true;

        switch (_currentChildIndex)
        {
            case 0: 
                if (_aPressed && _dPressed) NextPrompt(); 
                break;
            case 1:
                if (_spacePressed) NextPrompt();
                break;
            case 2:
                if (_shiftPressed) NextPrompt();
                break;
            case 3:
                if (_leftMousePressed) NextPrompt();
                break;
            case 4:
                if (_rightMousePressed) _children[_currentChildIndex].gameObject.SetActive(false);
                break;
        }
    }

    private void NextPrompt()
    {
        _children[_currentChildIndex].gameObject.SetActive(false);
        _children[++_currentChildIndex].gameObject.SetActive(true);
    }
}
