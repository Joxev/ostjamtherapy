using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class BackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int SceneIndex;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
    }

    public void onClick()
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
