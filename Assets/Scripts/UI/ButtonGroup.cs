using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField]
    private RectTransform ButtonsRoot;

    private const string BUTTON_ASSET_PATH = "ButtonAsset";

    public void AddButton(Sprite icon, Action onClick)
    {
        var image = GameObject.Instantiate<Image>(Resources.Load<Image>(BUTTON_ASSET_PATH), ButtonsRoot);
        image.sprite = icon;
        var button = image.GetOrAddComponent<Button>();
        button.onClick.AddListener(new UnityAction(onClick));
    }
}
