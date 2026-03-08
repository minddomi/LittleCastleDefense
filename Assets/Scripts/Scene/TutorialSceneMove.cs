using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScenesMove : MonoBehaviour
{
    public void TutorialScenesCtrl()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
