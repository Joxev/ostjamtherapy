using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInText : MonoBehaviour
{
    public TMP_Text fadeText;
    public Image fadeImage;

    public int MenuSceneIndex = 1;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }
    public IEnumerator FadeIn()
    {
        fadeText.color = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b, 0);
        while (fadeText.color.a < 1.0f)
        {
            fadeText.color = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b, fadeText.color.a + (Time.deltaTime / 6));
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        while (fadeImage.color.a < 1.0f)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a + (Time.deltaTime / 2));
            yield return null;
        }
        SoundManager.instance.GameplaySound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(MenuSceneIndex);
    }
}
