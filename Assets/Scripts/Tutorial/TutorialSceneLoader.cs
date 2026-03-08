using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSceneLoader : MonoBehaviour
{
    public void GoToMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}