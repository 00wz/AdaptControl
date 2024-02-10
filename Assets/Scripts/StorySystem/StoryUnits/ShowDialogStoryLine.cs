using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows the message and waits for the user's reply
/// </summary>
[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/ShowDialogStoryLine")]
public class ShowDialogStoryLine : ScriptableObject, IStoryLine
{
    [SerializeField]
    private string DialogHeader;
    [SerializeField]
    [TextArea(3, 5)]
    private string DialogBody;

    public event Action OnEndStoryLine;

    public void BeginStoryLine()
    {
        GameRoot.Instance.DialogView.ShowDialog(DialogHeader, DialogBody, OnEndStoryLine.Invoke);
    }
}
