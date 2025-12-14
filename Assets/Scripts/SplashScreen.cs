using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{

    public void Test()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
