using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInMenu : MonoBehaviour
{
    [HideInInspector] public CircularMenu cm;

    public Image fadeImage;

    public bool fadeOut;

    public bool onStartFadeIn = true;

    public int MenuSceneIndex = 1;

    private void Start()
    {
        if (!fadeOut)
        {
            fadeImage.gameObject.SetActive(false);
        }
        else
        {
            fadeImage.gameObject.SetActive(true);
        }
        if (onStartFadeIn)
        {
            StartCoroutine(FadeIn());
        }
    }
    public IEnumerator FadeIn()
    {
        if(!fadeOut)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
            while (fadeImage.color.a < 1.0f)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a + (Time.deltaTime / 2));
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            cm.StartGameAfterFade();
        }
        else
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
}
