using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionButton : MonoBehaviour
{
    public GameObject clickPrefab;
    public GameObject canvas;
    public TextMeshProUGUI buttonText;

    public void onClicked()
    {
        TextMeshProUGUI t = Instantiate(clickPrefab, transform.position, transform.rotation, canvas.transform).GetComponent<TextMeshProUGUI>();
        t.text = buttonText.text.ToString();
    }
}
