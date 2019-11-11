using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhotoImage
{
    void DidSelectPhoto(PhotoImage photoImage);
}

public class PhotoImage : MonoBehaviour
{

    public IPhotoImage photoDelegates;

    public void OnClickPhoto()
    {
        photoDelegates.DidSelectPhoto(this);
    }
}
