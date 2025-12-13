using UnityEngine;
using System.Linq;

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

    private TownLightMaterials mMaterials;

    void Awake()
    {
        if (!mLightTransform)
        {
            mLightTransform = gameObject.transform.parent;
        }

        mMaterials = GameObject.Find("TownLightMaterials").GetComponent<TownLightMaterials>();
    }

    private void Update()
    {
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


    }
}
