using System.Collections;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField]
    private float colorTransitionTime;
    [SerializeField]
    private Color[] colors;
    private int currentColor;

    SpriteRenderer spriteRenderer;

    void Start ()
    {
        Events.OnScreenShakeBegin += OnScreenShakeBegin;
        currentColor = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    void OnScreenShakeBegin()
    {
        StartCoroutine(FadeToColor(colors[currentColor++]));
    }

    IEnumerator FadeToColor(Color colorDestination)
    {
        Color colorSource = spriteRenderer.color;
        float time = 0;
        while (time < colorTransitionTime)
        {
            var color = Color.Lerp(colorSource, colorDestination, time / colorTransitionTime);
            spriteRenderer.color = color;
            yield return null;
            time += Time.deltaTime;
        }
        spriteRenderer.color = colorDestination;
    }
}
