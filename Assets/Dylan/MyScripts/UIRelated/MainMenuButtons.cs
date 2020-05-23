﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [TextArea(1, 10)]
    [SerializeField] private string buttonTooltipText;
    [SerializeField] private TextMeshProUGUI buttonsTooltipText;
    [SerializeField] private Sprite selectedImage;
    [SerializeField] private Sprite deselectedImage;

    public void OnDeselect(BaseEventData eventData)
    {
        buttonsTooltipText.text = null;
        buttonsTooltipText.transform.gameObject.SetActive(false);
        GetComponent<Button>().image.sprite = deselectedImage;
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonsTooltipText.text = buttonTooltipText;
        buttonsTooltipText.transform.gameObject.SetActive(true);
        GetComponent<Button>().image.sprite = selectedImage;
    }

}
