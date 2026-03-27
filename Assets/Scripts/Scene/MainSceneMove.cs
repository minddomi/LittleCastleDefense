using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScenesMove : MonoBehaviour
{
    public void MainScenesCtrl()
    {
        SceneManager.LoadScene("Main");
    }
}