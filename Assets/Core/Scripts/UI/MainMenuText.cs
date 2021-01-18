using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuText : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public CircularMenu CM;
    public Transform point;
    bool clicked = false;

    public float topRotation;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CM.isHover = true;
        GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CM.isHover = false;
        if (!clicked) { GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal; }
    }

    public void OnClick()
    {
        clicked = true;
        CM.onClick(topRotation);
    }

    private void Update()
    {
        transform.position = point.position;
    }
}
