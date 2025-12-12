using UnityEngine;
public class OnPassedOrFailedVariables : MonoBehaviour
{
    [Header("Light materials")]
    public Material standardMaterial;
    public Material passedMaterial;
    public Material failedMaterial;

    [Header("Flashing Light Variables")]
    public float timeFailedFlash = 0.2f;
    public float timeBetweenFlash = 0.3f;
    public int numberOfFlashWhenFailed = 3;

    [Header("Audio")]
    public AudioClip passedAudioClip;
    public AudioClip failedAudioClip;

}
