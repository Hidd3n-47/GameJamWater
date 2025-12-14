using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class IModule : MonoBehaviour
{
    public Transform mLightTransform;
    private OnPassedOrFailedVariables mPassedOrFailedVariables;

    public UnityEvent OnPassedEventHandler;
    public UnityEvent OnFailedEventHandler;
    public UnityEvent<int, bool> OnPuzzleCompletedEventHandler;

    private Light mWhiteLight;
    private Light mGreenLight;
    private Light mRedLight;

    private Light mCurrentActiveLight;

    private int mModuleId;
    public int ModuleId
    {
        get => mModuleId;
        set
        {
            mModuleId = value;
        }
    }

    private Blink mBlink;

    public void Register(GameManager manager, int moduleId)
    {
        mModuleId = moduleId;
        OnPuzzleCompletedEventHandler.AddListener(manager.OnModuleCompleted);
    }

    private void Awake()
    {
        OnPassedEventHandler.AddListener(Passed);
        OnFailedEventHandler.AddListener(Failed);

        mPassedOrFailedVariables = GameObject.Find("OnPassedOrFailedVariables").GetComponent<OnPassedOrFailedVariables>();

        GetComponent<AudioSource>().playOnAwake = false;

        Light[] lights = GetComponentsInChildren<Light>(includeInactive: true);
        mWhiteLight = lights.Where(x => x.name == "White Light").ToArray()[0];
        mGreenLight = lights.Where(x => x.name == "Green Light").ToArray()[0];
        mRedLight = lights.Where(x => x.name == "Red Light").ToArray()[0];

        mBlink = GameObject.Find("Blink").GetComponent<Blink>();

        mCurrentActiveLight = mWhiteLight;
    }

    private void OnDestroy()
    {
        OnFailedEventHandler.RemoveListener(Failed);
        OnPassedEventHandler.RemoveListener(Passed);
    }

    private void Passed()
    {
        // In case the light is still flashing from failing.
        StopAllCoroutines();

        if (mPassedOrFailedVariables.passedAudioClip)
        {
            AudioSource source = GetComponent<AudioSource>();
            source.resource = mPassedOrFailedVariables.passedAudioClip;

            source.Play();
        }

        ChangeActiveLight(mGreenLight);
        mLightTransform.GetComponent<MeshRenderer>().material = mPassedOrFailedVariables.passedMaterial;

        DisableOnComplete();

        mBlink.BlinkFunc();

        OnPuzzleCompletedEventHandler?.Invoke(mModuleId, true);

        OnPassed();
    }

    private void Failed()
    {
        AudioSource source = GetComponent<AudioSource>();
        if (!(source.isPlaying && source.resource == mPassedOrFailedVariables.failedAudioClip))
        {
            source.resource = mPassedOrFailedVariables.failedAudioClip;
            source.Play();
        }

        source.Play();

        StartCoroutine(LightAnimation());

        OnPuzzleCompletedEventHandler?.Invoke(mModuleId, false);

        OnFailed();
    }

    private IEnumerator LightAnimation()
    {
        MeshRenderer meshRenderer = mLightTransform.GetComponent<MeshRenderer>();

        for (int i = 0; i < mPassedOrFailedVariables.numberOfFlashWhenFailed; i++)
        {
            float timer = 0.0f;

            meshRenderer.material = mPassedOrFailedVariables.failedMaterial;
            ChangeActiveLight(mRedLight);
            while (timer < mPassedOrFailedVariables.timeFailedFlash)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            timer = 0.0f;

            meshRenderer.material = mPassedOrFailedVariables.standardMaterial;
            ChangeActiveLight(mWhiteLight);
            while (timer < mPassedOrFailedVariables.timeBetweenFlash)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        ChangeActiveLight(mWhiteLight);
        meshRenderer.material = mPassedOrFailedVariables.standardMaterial;
    }

    private void ChangeActiveLight(Light light)
    {
        mCurrentActiveLight.gameObject.SetActive(false);
        light.gameObject.SetActive(true);

        mCurrentActiveLight = light;
    }

    protected virtual void OnPassed()
    {
        Debug.Log("Passed module!");
    }

    protected virtual void OnFailed()
    {
        Debug.Log("Failed module!");
    }

    protected virtual void DisableOnComplete()
    {
        gameObject.GetComponent<IModule>().enabled = false;
    }
}
