using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour
{
    public GameObject pauseUI;

    private bool mPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mPaused)
            {
                Time.timeScale = 1.0f;
                pauseUI.SetActive(false);
            }
            else
            {
                Time.timeScale = 0.0f;
                pauseUI.SetActive(true);
            }

            mPaused = !mPaused;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
