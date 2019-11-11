using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ICell
{
    void DidSelectCell(Cell cell);
    void DidSelectDeleteCell(Cell cell);
}

public class Cell : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] Button deleteButton;
    Button cellButton;
    public ICell cellDelegate;

    public string Title
    {
        get
        {
            return this.title.text;
        }
        set
        {
            // title에 대한 유효성 체크
            this.title.text = value;
        }
    }
    private void Start()
    {
        //셀의 삭제버튼 안보이게 함
        cellButton = GetComponent<Button>();
        this.ActiveDelete = false;
        
    }

    //셀의 삭제버튼 안보이게 하는 프러퍼티
    public bool ActiveDelete
    {
        get { return deleteButton.gameObject.activeSelf; }
        set { deleteButton.gameObject.SetActive(value); 
        
            if(value)
            {
                cellButton.interactable = false;
            }
            else
            {
                cellButton.interactable = true;
            }
        }
    }
    public void OnClick()
    {
        //해당 셀 정보 보기
        cellDelegate.DidSelectCell(this);
    }

    public void OnClickDelete()
    {
        //해당 셀 지우기
        cellDelegate.DidSelectDeleteCell(this);
    }

    //public void ButtonOn()
    //{
    //    cellButton.interactable = true;
    //}
    //public void ButtonOff()
    //{
    //    cellButton.interactable = false;
    //}
}
