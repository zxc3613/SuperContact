using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PopupViewManager : MonoBehaviour
{
    //public enum AnimationType { TYPE1, TYPE2 };
    //public AnimationType animagtionType;
    Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    // 팝업 나타날때 호출할 함수
    public void Open(/*AnimationType animationType=AnimationType.TYPE1*/)
    {
        //this.animagtionType = animationType;
        gameObject.SetActive(true);
        //switch (this.animagtionType)
        //{
        //    case AnimationType.TYPE1:
        //        animator.SetTrigger("open");
        //        break;
        //    case AnimationType.TYPE2:
        //        animator.SetTrigger("open2");
        //        break;
        //}
        animator.SetTrigger("open");
    }

    protected void Close()
    {
        animator.SetTrigger("close");
    }

    public void SetDisablePanel()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
