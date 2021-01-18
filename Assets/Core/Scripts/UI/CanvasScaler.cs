using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScaler : MonoBehaviour
{
    float defaultCameraSize;

    private void Start()
    {
        defaultCameraSize = Camera.main.orthographicSize;
    }

    private void Update()
    {
        float change = Camera.main.orthographicSize / defaultCameraSize;

        float canvasScale = 1 / change;

        transform.localScale = new Vector3(canvasScale, canvasScale, canvasScale);
    }
}
