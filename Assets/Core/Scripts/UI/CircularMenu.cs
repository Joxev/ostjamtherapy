using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenu : MonoBehaviour
{
    public List<MenuButton> buttons = new List<MenuButton>();
    private Vector2 MousePosition;
    private Vector2 fromVector2M = new Vector2(0.5f, 1.0f);
    private Vector2 centerCircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVector2M;

    public int menuItems;
    public int currentMenuItem;
    private int oldMenuItem;

    private void Start()
    {
        menuItems = buttons.Count;
        foreach(MenuButton button in buttons)
        {
            button.sceneImage.color = button.NormalColor;
        }
        currentMenuItem = 0;
        oldMenuItem = 0;
    }

    private void Update()
    {
        GetCurrentMenuItem();
        if(Input.GetButtonDown("Fire1"))
        {
            ButtonAction();
        }
    }

    public void GetCurrentMenuItem()
    {
        MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        toVector2M = new Vector2(MousePosition.x / Screen.width, MousePosition.y / Screen.height);

        float angle = (Mathf.Atan2(fromVector2M.y - centerCircle.y, fromVector2M.x - centerCircle.x) - Mathf.Atan2(toVector2M.y - centerCircle.y, toVector2M.x - centerCircle.x)) * Mathf.Rad2Deg;

        if (angle < 0) { angle += 360; }

        currentMenuItem = (int) (angle / (360 / menuItems));

        if(currentMenuItem != oldMenuItem)
        {
            buttons[oldMenuItem].sceneImage.color = buttons[oldMenuItem].NormalColor;
            oldMenuItem = currentMenuItem;
            buttons[currentMenuItem].sceneImage.color = buttons[currentMenuItem].HighlightedColor;
        }
    }

    public void ButtonAction()
    {
        buttons[currentMenuItem].sceneImage.color = buttons[currentMenuItem].PressedColor;
    }
}

[System.Serializable]
public class MenuButton
{
    public string name;
    public Image sceneImage;
    public Color NormalColor = Color.white;
    public Color HighlightedColor = Color.grey;
    public Color PressedColor = Color.gray;
}


