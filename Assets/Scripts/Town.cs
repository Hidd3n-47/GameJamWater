using System;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Town : MonoBehaviour
{
    public UnityEvent BulbLevelChanged;

    public string Password;

    float mPercentage = 100.0f;

    [SerializeField]
    float mDecayRate = 3.0f;

    [SerializeField]
    Transform mLightTransform;

    [SerializeField]
    float mYellowPercent = 75.0f;
    [SerializeField]
    float mRedPercent = 25.0f;

    [SerializeField]
    private bool mDisabled;
    [SerializeField]
    private Material mDisabledTexture;

    private TownLightMaterials mMaterials;

    private TextMeshProUGUI mText;

    private int mLightBulbLevel = 1;

    [SerializeField]
    GameManager mGameManager;

    public bool CanFix => mPercentage <= mYellowPercent;

    public void IncreasePercentage(float addition)
    {
        mPercentage = Math.Min(mPercentage + addition, 100.0f);
    }

    public void DecreasePercentage(float decrease)
    {
        mPercentage -= decrease;
    }

    void Awake()
    {
        if (!mLightTransform)
        {
            mLightTransform = gameObject.transform.parent;
        }

        if (mDisabled)
        {
            var materials = mLightTransform.GetComponent<MeshRenderer>().materials;
            materials[1] = mDisabledTexture;
            mLightTransform.GetComponent<MeshRenderer>().materials = materials;

            mLightTransform.GetChild(0).gameObject.SetActive(false);

            return;
        }

        mMaterials = GameObject.Find("TownLightMaterials").GetComponent<TownLightMaterials>();

        mText = GetComponentsInChildren<TextMeshProUGUI>().First(x => !x.name.Contains("TMP"));

        Password = String.Empty;
        for (int i = 0; i < 4; ++i)
        {
            Password += Random.Range(0, 9);
        }

        GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name.Contains("TMP")).text = Password;

        BulbLevelChanged.AddListener(() => { GetComponent<AudioSource>().Play(); });

        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        int bulbLevel;

        if (mDisabled)
        {
            return;
        }

        mPercentage -= mDecayRate * Time.deltaTime;

        if (mPercentage > mYellowPercent)
        {
            var materials = mLightTransform.GetComponent<MeshRenderer>().materials;
            materials[1] = mMaterials.green;
            mLightTransform.GetComponent<MeshRenderer>().materials = materials;
            bulbLevel = 1;
        }
        else if (mPercentage > mRedPercent)
        {
            var materials = mLightTransform.GetComponent<MeshRenderer>().materials;
            materials[1] = mMaterials.yellow;
            mLightTransform.GetComponent<MeshRenderer>().materials = materials;
            bulbLevel = 2;
        }
        else
        {
            var materials = mLightTransform.GetComponent<MeshRenderer>().materials;
            materials[1] = mMaterials.red;
            mLightTransform.GetComponent<MeshRenderer>().materials = materials;
            bulbLevel = 3;

            if (mPercentage <= 0.0f)
            {
                mGameManager.OnGameFailed();
            }
        }

        mPercentage = Math.Max(mPercentage, 0.0f);

        mText.text = "[" + (int)mPercentage + "%]";

        if (bulbLevel > mLightBulbLevel)
        {
            BulbLevelChanged?.Invoke();
        }

        mLightBulbLevel = bulbLevel;
    }
}
