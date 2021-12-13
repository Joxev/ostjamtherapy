using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndingManager : MonoBehaviour
{
    //Code written by Joxev (Jack)

    public GameObject playerDead, patientDead, finishGame;
    public int ending = 0;
    public Image flash;
    public CarryOver carryOver;
    public float waitTime;

    public int menuindex = 1;


    private void Start()
    {
        playerDead.SetActive(false);
        patientDead.SetActive(false);
        finishGame.SetActive(false);

        carryOver = FindObjectOfType<CarryOver>().GetComponent<CarryOver>();

        ending = carryOver.ending;

        StartCoroutine(flashDead());
    }

    IEnumerator flashDead()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/Player_Death");
        
        if(ending == 1)
        {
            playerDead.SetActive(true);
        }
        else if (ending == 2)
        {
            patientDead.SetActive(true);
        }
        else
        {
            finishGame.SetActive(true);
        }
        Destroy(carryOver.gameObject);

        yield return new WaitForSeconds(waitTime);

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
