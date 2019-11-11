using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인터페이스
//public interface ICellPopupViewManagerDelegate
//{
//    void DeleteCell();
//}
public class CellPopupView : PopupViewManager
{
    //델리게이트
    public delegate void CellPopupViewManagerDelegate();
    public CellPopupViewManagerDelegate cellPopupViewManagerDelegate;

    //public ICellPopupViewManagerDelegate cellPopupViewManagerDelegate;

    public void OnOKButton()
    {
        cellPopupViewManagerDelegate.Invoke();
        Close();
    }
    public void OnNOButton()
    {
        Close();
    }
}
