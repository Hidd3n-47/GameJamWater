using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public float duration = 0.1f;

    public RectTransform topRect;
    public RectTransform botRect;
    public Image top;
    public Image bot;
    public Canvas canv;

    private float halfHeight;

    private void Start()
    {
        halfHeight = canv.pixelRect.height;
    }

    public void BlinkFunc()
    {
        StartCoroutine(BlinkCo());
    }

    IEnumerator BlinkCo()
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            top.rectTransform.sizeDelta = new Vector2(top.rectTransform.sizeDelta.x, math.lerp(0.0f, halfHeight, timer / duration));
            bot.rectTransform.sizeDelta = new Vector2(bot.rectTransform.sizeDelta.x, math.lerp(0.0f, halfHeight, timer / duration));

            timer += Time.deltaTime;
            yield return null;
        }

        top.rectTransform.sizeDelta = new Vector2(top.rectTransform.sizeDelta.x, halfHeight);
        bot.rectTransform.sizeDelta = new Vector2(bot.rectTransform.sizeDelta.x, halfHeight);

        timer = 0.0f;

        while (timer < duration)
        {
            top.rectTransform.sizeDelta = new Vector2(top.rectTransform.sizeDelta.x, math.lerp(halfHeight, 0.0f,  timer / duration));
            bot.rectTransform.sizeDelta = new Vector2(bot.rectTransform.sizeDelta.x, math.lerp(halfHeight, 0.0f,  timer / duration));

            timer += Time.deltaTime;
            yield return null;
        }

        top.rectTransform.sizeDelta = new Vector2(top.rectTransform.sizeDelta.x,0.0f);
        bot.rectTransform.sizeDelta = new Vector2(bot.rectTransform.sizeDelta.x,0.0f);
    }
}
