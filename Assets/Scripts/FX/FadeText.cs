using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    [SerializeField]
    float fadeTime;
    Text text;
    float targetAlpha;
	void Awake()
    {
        text = GetComponent<Text>();
        targetAlpha = text.color.a;
        Color noAlpha = text.color;
        noAlpha.a = 0;
        text.color = noAlpha;
        Events.OnGameOver += OnGameOver;
	}

    private void OnDestroy()
    {
        Events.OnGameOver -= OnGameOver;
    }

    void OnGameOver()
    {
        StartCoroutine(FadeInText());
    }

    IEnumerator FadeInText()
    {
        Color c = text.color;
        float time = 0;
        while (time < fadeTime)
        {
            float alpha = Mathf.Lerp(0, targetAlpha, time / fadeTime);
            c.a = alpha;
            text.color = c;
            yield return null;
            time += Time.deltaTime;
        }
        c.a = targetAlpha;
        text.color = c;
    }
}