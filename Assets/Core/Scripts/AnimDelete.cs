using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDelete : MonoBehaviour
{
    Vector3 worldSpace;

    private void Start()
    {
        worldSpace = Camera.main.ScreenToWorldPoint(transform.position);
    }

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(worldSpace);
    }

    public void delete()
    {
        Destroy(gameObject);
    }
}
