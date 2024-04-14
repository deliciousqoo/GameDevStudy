using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    [SerializeField]
    private Sprite onButton;
    [SerializeField]
    private Sprite offButton;

    private Button button;

    private bool check;

    private void Start()
    {
        button = GetComponent<Button>();
        button.image.sprite = onButton;
    }

    public void ButtonImageChange()
    {
        if (!check)
        {
            button.image.sprite = offButton;
            check = true;
        }
        else
        {
            button.image.sprite = onButton;
            check = false;
        }
    }
}
