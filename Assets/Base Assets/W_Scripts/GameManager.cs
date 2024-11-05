using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public InputController InputController { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        InputController = GetComponentInChildren<InputController>();
    }

    public void NextScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex + 1);
    }
}
