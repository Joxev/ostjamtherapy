using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryOver : MonoBehaviour
{
    public int ending;

    private void Start() {
        DontDestroyOnLoad(this.gameObject);
    }
}
