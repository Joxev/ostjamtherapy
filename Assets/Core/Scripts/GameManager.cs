using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Patient> patients = new List<Patient>();

    int currentPatient = 0;
    int suicideCount = 0;

    [Header("Effects")]
    public GameObject normalPostProcessing;
    public GameObject deathPostProcessing;

    private void Start()
    {
        foreach(Patient p in patients)
        {
            if(p.gameObject.activeSelf)
            {
                p.gameObject.SetActive(false);
            }
        }
        deathPostProcessing.SetActive(false);
        normalPostProcessing.SetActive(true);

        BubbleDialogueUI.instance.dialogueRunner.AddCommandHandler("PatientSuicide", patientSuicide);
        BubbleDialogueUI.instance.dialogueRunner.AddCommandHandler("PlayerDeath", playerDeath);
        BubbleDialogueUI.instance.dialogueRunner.AddCommandHandler("PatientSuccess", patientSuccess);

        initalizePatient();
    }
    public void initalizePatient()
    {
        patients[currentPatient].gameObject.SetActive(true);
        patients[currentPatient].initalizePatient();
    }

    public void nextPatient()
    {
        if(currentPatient >= patients.Count)
        {
            //Game Over
            return;
        }
        patients[currentPatient].gameObject.SetActive(false);
        currentPatient++;
        SoundManager.instance.GameplaySound.setParameterByName("Progress", currentPatient);

        initalizePatient();
    }
    public void patientSuccess(string[] info)
    {

    }
    public void patientSuicide(string[] info)
    {
        StartCoroutine(deathWait());
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

    public void playerDeath(string[] info)
    {
        deathPostProcessing.SetActive(true);
        normalPostProcessing.SetActive(false);
        //GameOver
    }


}
