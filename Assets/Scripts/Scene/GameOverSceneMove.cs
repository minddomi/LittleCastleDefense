using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScenesMove : MonoBehaviour
{
    public void GameOverScenesCtrl()
    {
        SceneManager.LoadScene("GameOver");
    }
}