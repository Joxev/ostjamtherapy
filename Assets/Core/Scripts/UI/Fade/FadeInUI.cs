using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FadeInUI : MonoBehaviour
{
    public Image fadeImage;

    public int MenuSceneIndex = 1;

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }
    public IEnumerator FadeOut()
    { 
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
        while (fadeImage.color.a > 0f)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a - (Time.deltaTime / 2));
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }
}
