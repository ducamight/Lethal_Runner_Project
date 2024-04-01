using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int coin;
    private void Awake() {
        Instance = this;
    }

    public void RestartLevel() => SceneManager.LoadScene(0);
}
