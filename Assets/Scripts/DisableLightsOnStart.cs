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

    private void Start()
    {
        {
            StartCoroutine(help());

        }
    }

    private void PlayVoiceLine()
    {
        GetComponent<AudioSource>().Play();
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

        float asdfa = 0.0f;

        while (asdfa < mTheInbetween)
        {
            asdfa += Time.deltaTime;
            yield return null;
        }

        Destroy(letThereBeLightsBet.gameObject);
        Destroy(noLights.gameObject);
        f.gameObject.SetActive(true);

    }


}
