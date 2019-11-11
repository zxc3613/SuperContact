using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCell : MonoBehaviour
{
    Image image;
    Action<Sprite> onClickAction;

    public void SetImageCell(Sprite sprite, Action<Sprite> onClickAction)
    {
        this.GetComponent<Image>().sprite = sprite;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            onClickAction(this.GetComponent<Image>().sprite);
        });
    }
}
