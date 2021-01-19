using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform textPoint;
    public Transform focusPoint;

    public GameObject deathPose;


    private void OnEnable()
    {
        deathPose.SetActive(false);
    }
}
