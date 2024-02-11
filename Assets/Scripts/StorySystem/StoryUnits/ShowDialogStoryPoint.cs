using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows the message and waits for the user's reply
/// </summary>
[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/ShowDialogStoryPoint")]
public class ShowDialogStoryPoint : ScriptableObject, IStoryPoint
{
    [SerializeField]
    private string DialogHeader;
    [SerializeField]
    [TextArea(3, 5)]
    private string DialogBody;

    public void BeginStoryPoint()
    {
        GameRoot.Instance.DialogView.ShowDialog(DialogHeader, DialogBody);
    }
}
