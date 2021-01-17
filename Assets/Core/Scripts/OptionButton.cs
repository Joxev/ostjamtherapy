using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public GameObject clickPrefab;
    public GameObject canvas;
    public TextMeshProUGUI buttonText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Animator>().SetBool("isHover", true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Animator>().SetBool("isHover", false);
    }
    public void onClicked()
    {
        TextMeshProUGUI t = Instantiate(clickPrefab, transform.position, transform.rotation, recursiveParent(transform)).GetComponent<TextMeshProUGUI>();
        t.text = buttonText.text.ToString();
    }

    public Transform recursiveParent(Transform currentTransform)
    {
        if(currentTransform.parent.GetComponent<CanvasGroup>() != null)
        {
            return currentTransform.parent;
        }
        else
        {
            return recursiveParent(currentTransform.parent);
        }
    }
}
