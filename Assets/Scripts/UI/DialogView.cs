using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogView : MonoBehaviour
{
    [SerializeField]
    private Text Head;
    [SerializeField]
    private Text Body;
    [SerializeField]
    private RectTransform ButtonRoot;
    [SerializeField]
    private ButtonView ButtonPrefab;

    private List<ButtonView> _buttons = new();
    private const string DEFAULT_BUTTON_TEXT = "Ok";

    /// <summary>
    /// Shows a message box
    /// </summary>
    /// <param name="headerMessage">message header</param>
    /// <param name="bodyMessage">message</param>
    /// <param name="onOKPressed">event triggered when the user presses the OK button</param>
    public void ShowDialog(string headerMessage, string bodyMessage, Action onOKPressed)
    {
        Head.text = headerMessage;
        Body.text = bodyMessage;
        ClearButtons();
        SetUpButton(DEFAULT_BUTTON_TEXT, onOKPressed);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Shows a message box
    /// </summary>
    /// <param name="headerMessage">message header</param>
    /// <param name="bodyMessage">message</param>
    /// <param name="answers">optionally. creates buttons labeled "answer" that 
    /// trigger an onSelecting event when pressed</param>
    public void ShowDialog(string headerMessage, string bodyMessage,
        params (string answer, Action onSelecting)[] answers)
    {
        Head.text = headerMessage;
        Body.text = bodyMessage;
        ClearButtons();
        SetUpButtons(answers);
        gameObject.SetActive(true);
    }

    private void SetUpButtons((string answer, Action onSelecting)[] answers)
    {
        if(answers.Length == 0)
        {
            SetUpButton(DEFAULT_BUTTON_TEXT);
        }
        foreach((string answer, Action onSelecting) in answers)
        {
            SetUpButton(answer, onSelecting);
        }
    }

    ///creates a button labeled text that closes the dialog box.
    ///optionally, you can add an event when you press
    private void SetUpButton(string text, Action onClick = null)
    {
        var button = Instantiate<ButtonView>(ButtonPrefab, ButtonRoot);
        Action onClickAction = onClick == null ?
            () => this.gameObject.SetActive(false) :
            () => { this.gameObject.SetActive(false); onClick.Invoke(); };
        button.SetUp(onClickAction, text);
        _buttons.Add(button);
    }

    private void ClearButtons()
    {
        foreach(var batton in _buttons)
        {
            Destroy(batton.gameObject);
        }
        _buttons.Clear();
    }
}
