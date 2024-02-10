using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonView : MonoBehaviour
{
    [SerializeField]
    private Text ButtonText;
    [SerializeField]
    private Button _button;

    private void Awake()
    {
        //_button = GetComponent<Button>();
    }

    public void SetUp(Action onClick, string text)
    {
        _button.onClick.AddListener(new UnityEngine.Events.UnityAction(onClick));
        ButtonText.text = text;
    }
}
