using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisableLightsOnStart : MonoBehaviour
{
    UnityEvent OnLightTurnedOn;

    public Transform noLights;
    public Transform letThereBeLightsBet;
    public Transform letThereBeLights;

    public List<Transform> mapLights;

    [SerializeField] private float mMapLightsFlashTime = 0.14f;
    [SerializeField] private int mMapLightsFlashCount = 4;


    [SerializeField] private float mTheInbetween = 0.2f;

    [SerializeField] private Material mMatWhite;
    [SerializeField] private Material mMatGreen;

    private void Start()
    {
        {
            StartCoroutine(help());

        }
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
