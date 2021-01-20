using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Patient> patients = new List<Patient>();

    public Character Therapist;

    public GameObject Camera;

    int currentPatient = 0;
    int suicideCount = 0;

    [Header("Effects")]
    public GameObject normalPostProcessing;
    public GameObject deathPostProcessing;

    public GameObject Background;

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
        patients[currentPatient].gameObject.SetActive(true);
        patients[currentPatient].initalizePatient();
    }
    Coroutine nextP;
    public void nextPatient()
    {
        if(currentPatient >= patients.Count)
        {
            //Game Over
            return;
        }
        nextP = StartCoroutine(nextPatientWait());
    }
    public void patientSuccess(string[] info)
    {
        patientEnd();
        nextPatient();
    }
    public void patientSuicide(string[] info)
    {
        Background.GetComponent<Animator>().SetBool("Blood", true);
        patients[currentPatient].GetComponent<Character>().deathPose.SetActive(true);
        patients[currentPatient].GetComponent<SpriteRenderer>().enabled = false;
        patientEnd();
        StartCoroutine(deathWait());
    }
    public void playerDeath(string[] info)
    {
        Background.GetComponent<Animator>().SetBool("Blood", true);
        Therapist.GetComponent<Character>().deathPose.SetActive(true);
        Therapist.GetComponent<SpriteRenderer>().enabled = false;
        patientEnd();
        deathPostProcessing.SetActive(true);
        normalPostProcessing.SetActive(false);
        //GameOver
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
        yield return new WaitForSeconds(5f);

        suicideCount++;
        
        if(suicideCount >= 2)
        {
            //Game Over
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
        yield return new WaitForSeconds(1f);
        Background.GetComponent<Animator>().SetBool("Blood", false);

        patients[currentPatient].gameObject.SetActive(false);
        currentPatient++;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Progress", currentPatient);

        initalizePatient();

        yield return new WaitForSeconds(1f);

        while (transitionUI.GetComponent<CanvasGroup>().alpha > 0)
        {
            transitionUI.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
            yield return null;
        }
        transitionUI.enabled = false;
    }
}
