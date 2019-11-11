using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AddPhotoPopupViewManager : PopupViewManager
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] GameObject baseImageCell;


    public Action<Sprite> didSelectImage;
    protected override void Awake() {
        base.Awake();

        Sprite[] sprites = SpriteManager.Load();
        MakeImageCell(sprites);

        //baseImageCell 비활성화
        baseImageCell.SetActive(false);
    }

    private void MakeImageCell(Sprite[] sprites)
    {
        //float cellHeight = (셀 Y 사이즈 + spacing.y) * (이미지 수 /3) + Top padding + botton
        float cellHright = (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y) * 
            (sprites.Length / gridLayoutGroup.constraintCount) + 
            gridLayoutGroup.padding.top + 
            gridLayoutGroup.padding.bottom;
        scrollRect.content.sizeDelta = new Vector2(0, cellHright);
        foreach (Sprite sprite in sprites)
        {
            ImageCell imageCell = Instantiate(baseImageCell, scrollRect.content).GetComponent<ImageCell>();
            imageCell.SetImageCell(sprite, (selectsprite) =>
            {
                didSelectImage?.Invoke(selectsprite);
                Close();
            });
        }
    }

    public void OnClickClose()
    {
        Close();
    }
}
