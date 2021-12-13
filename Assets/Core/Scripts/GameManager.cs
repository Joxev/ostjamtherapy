using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMOD;
using FMODUnity;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Patient> patients = new List<Patient>();

    public Character Therapist;

    public GameObject Camera;

    public int EndingSceneIndex = 5;

    public CarryOver carryOver;

    int currentPatient = 0;
    int suicideCount = 0;

    [Header("Effects")]
    public GameObject normalPostProcessing;
    public GameObject deathPostProcessing;

    public GameObject Background;
    public GameObject FlowerPot;

    public Image transitionUI;

    public float fadeInTime = 0.1f;

    private void Start()
    {
        transitionUI.enabled = false;
        foreach(Patient p in patients)
        {
            if(p.gameObject.activeSelf)
            {
                p.gameObject.SetActive(false);
            }
        }
        deathPostProcessing.SetActive(false);
        normalPostProcessing.SetActive(true);

        BubbleDialogueUI.instance.dialogueRunner.AddCommandHandler("PatientSuccess", patientSuccess);
        BubbleDialogueUI.instance.dialogueRunner.AddCommandHandler("PatientSuicide", patientSuicide);
        BubbleDialogueUI.instance.dialogueRunner.AddCommandHandler("PlayerDeath", playerDeath);

        initalizePatient();
    }
    public void initalizePatient()
    {
        if(patients[currentPatient] == null)
        {
            carryOver.ending = 0;
            SceneManager.LoadScene(EndingSceneIndex);
        }
        else
        {
            patients[currentPatient].gameObject.SetActive(true);
            patients[currentPatient].initalizePatient();
        }
    }
    Coroutine nextP;
    public void nextPatient()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Sadness", 0);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Anger", 0);
        nextP = StartCoroutine(nextPatientWait());
    }
    public void patientSuccess(string[] info)
    {
        patientEnd();
        nextPatient();
    }
    public void patientSuicide(string[] info)
    {
        carryOver.ending = 2;
        Background.GetComponent<Animator>().SetBool("Blood", true);
        FlowerPot.GetComponent<Animator>().SetBool("dead", true);
        patients[currentPatient].GetComponent<Character>().deathPose.SetActive(true);
        patients[currentPatient].GetComponent<SpriteRenderer>().enabled = false;
        patientEnd();
        StartCoroutine(deathWait());
    }
    public void playerDeath(string[] info)
    {
        death();
        Background.GetComponent<Animator>().SetBool("Blood", true);
        FlowerPot.GetComponent<Animator>().SetBool("dead", true);
        Therapist.GetComponent<Character>().deathPose.SetActive(true);
        Therapist.GetComponent<SpriteRenderer>().enabled = false;
        patientEnd();
        deathPostProcessing.SetActive(true);
        normalPostProcessing.SetActive(false);
    }
    public void death()
    {
        StartCoroutine(playerDeathWait());
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Death", 1);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Game/Player_Death");
    }

    IEnumerator playerDeathWait()
    {
        carryOver.ending = 1;
        yield return new WaitForSeconds(3f);
        transitionUI.enabled = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(EndingSceneIndex);
    }
    public void patientEnd()
    {
        patients[currentPatient].GetComponent<Animator>().SetBool("talking", false);
        BubbleDialogueUI.instance.text.GetComponent<TextMeshProUGUI>().text = "";
        BubbleDialogueUI.instance.dialogueRunner.Stop();
    }
    IEnumerator deathWait()
    {
        deathPostProcessing.SetActive(true);
        normalPostProcessing.SetActive(false);
        yield return new WaitForSeconds(3f);

        suicideCount++;
        
        if(suicideCount > 1)
        {
            SceneManager.LoadScene(EndingSceneIndex);
        }
        else
        {
            nextPatient();
            deathPostProcessing.SetActive(false);
            normalPostProcessing.SetActive(true);
        }
    }
    IEnumerator nextPatientWait()
    {
        transitionUI.enabled = true;
        yield return new WaitForSeconds(3f);
        if (!(currentPatient >= patients.Count))
        {
            Background.GetComponent<Animator>().SetBool("Blood", false);
            FlowerPot.GetComponent<Animator>().SetBool("dead", false);
        }

        patients[currentPatient].gameObject.SetActive(false);
        currentPatient++;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Progress", currentPatient);

        if(currentPatient >= patients.Count)
        {
            print("End");
            StartCoroutine(playerDeathWait());
            StopCoroutine(nextP);
        }

        initalizePatient();

        yield return new WaitForSeconds(1f);

        while (transitionUI.GetComponent<CanvasGroup>().alpha > 0)
        {
            transitionUI.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
            yield return null;
        }
        transitionUI.enabled = false;

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Death", 0);
    }
}
