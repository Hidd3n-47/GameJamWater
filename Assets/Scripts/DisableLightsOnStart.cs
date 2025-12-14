using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class DisableLightsOnStart : MonoBehaviour
{
    public UnityEvent OnLightTurnedOn;

    public Transform noLights;
    public Transform letThereBeLightsBet;
    public Transform letThereBeLights;

    public List<Transform> mapLights;

    [SerializeField] private float mMapLightsFlashTime = 0.14f;
    [SerializeField] private int mMapLightsFlashCount = 4;


    [SerializeField] private float mTheInbetween = 0.2f;

    [SerializeField] private Material mMatWhite;
    [SerializeField] private Material mMatGreen;

    [SerializeField] private bool mPlayAudio = true;

    [SerializeField] private AudioSource mRinging;
    [SerializeField] private AudioSource mVoiceOver;
    [SerializeField] private AudioSource mEndCall;

    private void Start()
    {
        {
            StartCoroutine(help());
        }
    }

    private void PlayVoiceLine()
    {
        if(mRinging.clip) mRinging.Play();
        if (mVoiceOver.clip) mVoiceOver.Play();
        if (mEndCall.clip) mEndCall.Play();
    }

    private IEnumerator help()
    {
        var f = letThereBeLights;
        foreach (var l in mapLights)
        {
            float timer = 0.0f;

            {
                var mesh = l.GetComponent<MeshRenderer>();
                var materials = mesh.materials;
                materials[1] = mMatGreen;
                mesh.materials = materials;

                OnLightTurnedOn?.Invoke();
            }

            while (timer < mMapLightsFlashTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        noLights.gameObject.SetActive(true);
        letThereBeLightsBet.gameObject.SetActive(true);

        if (mPlayAudio)
        {
            PlayVoiceLine();
        }

        float addedtime = mPlayAudio ? mRinging.clip.length + mVoiceOver.clip.length + mEndCall.clip.length : 0.0f;

        float asdfa = 0.0f;

        while (asdfa < mTheInbetween + addedtime)
        {
            asdfa += Time.deltaTime;
            yield return null;
        }

        Destroy(letThereBeLightsBet.gameObject);
        Destroy(noLights.gameObject);
        f.gameObject.SetActive(true);

    }


}
