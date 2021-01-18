using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenu : MonoBehaviour
{
    public Transform textPointHolder;

    float value = 1;

    public bool isHover = false;

    public bool hasClicked = false;

    public float targetRotation;

    public GameObject carrotUi;

    private void Start()
    {
        carrotUi.SetActive(false);
    }

    private void Update()
    {
        if (hasClicked)
        {
            if (Mathf.Abs(textPointHolder.transform.localEulerAngles.z - targetRotation) > 0.5f)
            {
                if(textPointHolder.transform.localEulerAngles.z < targetRotation)
                {
                    value += 0.3f;
                }
                else
                {
                    value -= 0.3f;
                }
            }
            else
            {
                carrotUi.SetActive(true);
            }
        }
        else if(!isHover)
        {
            if (value > 360)
            {
                value = 0;
            }
            else
            {
                value += 0.03f;
            }
        }
        else
        {
            value += Mathf.Sin(Time.time * 2) * 0.003f;
        }
        textPointHolder.transform.rotation = Quaternion.Euler(textPointHolder.transform.rotation.x, textPointHolder.transform.rotation.y, value);
    }

    public void onClick(float targetRot)
    {
        if(!hasClicked)
        {
            targetRotation = targetRot;
            hasClicked = true;
        }
    }
}

