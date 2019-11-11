using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public interface IAddphotoPopupViewManager
{
    void DidSelectAddPhoto(AddPhotoPopupViewManager addPhotoPopupViewManager);
}
public class AddPhotoPopupViewManager : PopupViewManager, IPhotoImage
{
    public IAddphotoPopupViewManager addphotoPopupDelegate;
    Sprite[] sprites;
    [SerializeField] Image imagePrefab;
    [SerializeField] RectTransform content;
    float imageHeight = 550;

    public List<Sprite> spritestList = new List<Sprite>();
    public List<PhotoImage> spritestLi = new List<PhotoImage>();

    protected override void Awake()
    {
        base.Awake();
        sprites = Resources.LoadAll<Sprite>("photo");
        
        Debug.Log(sprites);

        AddImage();
        AddContent();
    }
    public void AddImage()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            PhotoImage photoImage = Instantiate(imagePrefab, content).GetComponent<PhotoImage>();
            photoImage.GetComponent<Image>().sprite = sprites[i];
            photoImage.photoDelegates = this;
            spritestList.Add(sprites[i]);
            spritestLi.Add(photoImage);
        }
    }
    public void AddContent()
    {
        if (sprites.Length > 0)
        {
            content.sizeDelta = new Vector2(0, spritestList.Count * imageHeight);
        }
        else
        {
            content.sizeDelta = Vector2.zero;
        }
    }

    public void OnClickClose()
    {
        Close();
    }
    [HideInInspector] public int h;
    public void DidSelectPhoto(PhotoImage photoImage)
    {
        //AddPopupViewManager addPopupViewManager = GameObject.Find("Add Popup View(Clone)").GetComponent<AddPopupViewManager>();
        //int photoIndex = spritestList.IndexOf(photoImage);
        h = spritestLi.IndexOf(photoImage);
        //addPopupViewManager.profileImage.sprite = spritestList[photoIndex].GetComponent<Sprite>();

        addphotoPopupDelegate.DidSelectAddPhoto(this);
        Close();

    }
}
