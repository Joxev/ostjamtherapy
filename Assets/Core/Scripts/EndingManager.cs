using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndingManager : MonoBehaviour
{
    //Code written by Joxev (Jack)

    public GameObject dead1;
    public GameObject dead2;
    public GameObject dead3;
    public Image flash;

    public int menuindex = 1;


    private void Start()
    {
        dead1.SetActive(false);
        dead2.SetActive(false);
        dead3.SetActive(false);

        StartCoroutine(flashDead());
    }

    IEnumerator flashDead()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/Player_Death");
        dead1.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        dead1.SetActive(false);
        flash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flash.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.35f);
        dead2.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        dead2.SetActive(false);
        flash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flash.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.45f);
        dead3.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        StartCoroutine( FadeIn());
        yield return new WaitForSeconds(3f);

        SoundManager.instance.GameplaySound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(menuindex);
    }

    public IEnumerator FadeIn()
    {
        flash.gameObject.SetActive(true);
        flash.color = new Color(flash.color.r, flash.color.g, flash.color.b, 0);
        while (flash.color.a < 1.0f)
        {
            flash.color = new Color(flash.color.r, flash.color.g, flash.color.b, flash.color.a + (Time.deltaTime / 2));
            yield return null;
        }
    }
}
