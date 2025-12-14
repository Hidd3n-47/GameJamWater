using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour
{
    public GameObject pauseUI;

    private bool mPaused = false;

    public GameObject mainMusic;
    public GameObject ticking;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mPaused)
            {
                Time.timeScale = 1.0f;
                pauseUI.SetActive(false);

                mainMusic.SetActive(false);
                ticking.GetComponents<AudioSource>()[1].Pause();
            }
            else
            {
                Time.timeScale = 0.0f;
                pauseUI.SetActive(true);

                mainMusic.SetActive(true);
                ticking.GetComponents<AudioSource>()[1].Play();
            }

            mPaused = !mPaused;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
