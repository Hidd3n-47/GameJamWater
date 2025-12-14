using System;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class Town : MonoBehaviour
{
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
    }

    private void Update()
    {
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
        }
        else if (mPercentage > mRedPercent)
        {
            var materials = mLightTransform.GetComponent<MeshRenderer>().materials;
            materials[1] = mMaterials.yellow;
            mLightTransform.GetComponent<MeshRenderer>().materials = materials;
        }
        else
        {
            var materials = mLightTransform.GetComponent<MeshRenderer>().materials;
            materials[1] = mMaterials.red;
            mLightTransform.GetComponent<MeshRenderer>().materials = materials;
        }

        mPercentage = Math.Max(mPercentage, 0.0f);

        mText.text = "[" + (int)mPercentage + "%]";
    }
}
