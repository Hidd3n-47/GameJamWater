using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Button startButton; 
    public Button quitButton;
    public GameObject cam;
    public float camStartRot = 80.118f;
    public float camEndRot = 78.663f;
    public float duration = 5f;


    private void Start()
    {
        StartCoroutine(Camera());
    }

    public void StartGame()
    {

        SceneManager.LoadSceneAsync("SampleScene");

    }
    public void QuitGame()
    {

        Application.Quit();

    }

    IEnumerator Camera()
    {
        float timer = 0f;
        while (timer < duration)
        {

            timer+= Time.deltaTime;
            Vector3 Ang = cam.transform.eulerAngles;

            Ang.x = Mathf.Lerp(camStartRot, camEndRot, timer / duration); 
            cam.transform.eulerAngles = Ang;

            yield return null;
        }

        timer = 0f;

        while (timer < duration)
        {

            timer += Time.deltaTime;
            Vector3 Ang = cam.transform.eulerAngles;

            Ang.x = Mathf.Lerp(camEndRot, camStartRot, timer / duration);
            cam.transform.eulerAngles = Ang;

            yield return null;
        }

        StartCoroutine(Camera());
    }

}
