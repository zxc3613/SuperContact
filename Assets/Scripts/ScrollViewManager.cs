using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewManager : ViewManager, ICell //ICellPopupViewManagerDelegate
{
    [SerializeField] GameObject cellPrefab;
    [SerializeField] GameObject addPopupViewPrefab;
    [SerializeField] GameObject detailViewPrefab;
    [SerializeField] GameObject cellPopupViewPrefab;

    [SerializeField] RectTransform content;

    List<Cell> cellList = new List<Cell>();

    float cellHeight = 240f;
    Contacts? contacts;

    //Cell 편집 버튼 관련 변수
    bool isEditable = false;

    private void Awake() 
    {
        // Title 지정
        title = "수퍼연락처";

        // Add 버튼 지정
        rightNavgationViewButton = Instantiate(buttonPrefab).GetComponent<SCButton>();
        rightNavgationViewButton.SetTitle("추가");
        rightNavgationViewButton.SetOnClickAction(() =>
        {
            // AddPopupViewManager를 표시하는 동작 구현
            AddPopupViewManager addPopupViewManager =
                Instantiate(addPopupViewPrefab, mainManager.transform).GetComponent<AddPopupViewManager>();

            // 새로운 연락처를 추가했을때 할 일
            addPopupViewManager.addContactCallback = (contact) =>
            {
                AddContact(contact);
                AddCell(contact, contacts.Value.contactList.Count);

                ClearCell();
                LoadData();
            };

            // AddPopupViewManager 열기
            addPopupViewManager.Open();
        });

        leftNavgationViewButton = Instantiate(buttonPrefab).GetComponent<SCButton>();
        leftNavgationViewButton.SetTitle((isEditable) ? "완료" : "편집");
        leftNavgationViewButton.SetOnClickAction(() =>
        {
            isEditable = !isEditable;

            if (isEditable)
            {
                leftNavgationViewButton.SetTitle("완료");
                foreach (Cell cell in cellList)
                {
                    cell.ActiveDelete = true;
                    rightNavgationViewButton.gameObject.SetActive(false);
                }
            }
            else
            {
                leftNavgationViewButton.SetTitle("편집");
                foreach (Cell cell in cellList)
                {
                    cell.ActiveDelete = false;
                    rightNavgationViewButton.gameObject.SetActive(true);
                }
            }
        });
    }
    //셀 모두 지우기
    void ClearCell()
    {
        foreach (Cell cell in cellList)
        {
            Destroy(cell.gameObject);
        }
        cellList.RemoveRange(0, cellList.Count);
    }
    private void Start() 
    {
        contacts = FileManager<Contacts>.Load(Constant.kFileName);
        LoadData();
    }

    // TODO: Contacts에 있는 정보를 Cell로 만들어서 추가
    void LoadData()
    {
        if (contacts.HasValue)
        {
            Contacts contactsValue = contacts.Value;

            contactsValue.contactList.Sort();
            for (int i = 0; i < contactsValue.contactList.Count; i++)
            {
                AddCell(contactsValue.contactList[i], i);
            }
        }
    }

    // Contact 정보로 Cell 객체를 만들어서 content에 추가하는 함수
    void AddCell(Contact contact, int index)
    {
        Cell cell = Instantiate(cellPrefab, content).GetComponent<Cell>();
        cell.Title = contact.name;
        cell.cellDelegate = this;
        cellList.Add(cell);

        // Content의 높이 재조정
        AdjustContent();
    }

    // Contacts에 Contact 추가
    void AddContact(Contact contact)
    {
        if (contacts.HasValue)
        {
            Contacts contactsValue = contacts.Value;
            contactsValue.contactList.Add(contact);
        }
        else
        {
            List<Contact> contactsList = new List<Contact>();
            contactsList.Add(contact);

            contacts = new Contacts(contactsList);
        }
    }

    // Content의 높이 재조정
    void AdjustContent()
    {
        if (contacts.HasValue)
        {
            Contacts contactsValue = contacts.Value;
            content.sizeDelta = new Vector2(0, contactsValue.contactList.Count * cellHeight);
        }
        else
        {
            content.sizeDelta = Vector2.zero;
        }
    }

    void OnApplicationQuit()
    {
        if (contacts.HasValue)
            FileManager<Contacts>.Save(contacts.Value, Constant.kFileName);
    }

    // Cell이 터치 되었을때 호출하는 함수
    public void DidSelectCell(Cell cell)
    {
        //어떤 Cell이 선택되었는지 확인 후 Cell과 관련된 Detail 화면 표시
        if (contacts.HasValue)
        {
            int cellIndex = cellList.IndexOf(cell);

            DetailViewManager detailViewManager
                = Instantiate(detailViewPrefab).GetComponent<DetailViewManager>();

            Contact selectedContact = contacts.Value.contactList[cellIndex];
            detailViewManager.contact = selectedContact;

            detailViewManager.saveDelegate = (newContact) =>
            {
                //전체 연락처 정보를 가지고 있는 contacts 변수에 있는 예전 정보를 newContact 정보로 바꿔줘야함.
                //contacts.contacts << newContact를 추가해야함.

                contacts.Value.contactList[cellIndex] = newContact;
                cell.Title = newContact.name;
            };
            mainManager.PresentViewManager(detailViewManager);
        }
    }
    //Cell의 버튼이 터치 되었을때 호출하는 함수
    public void DidSelectDeleteCell(Cell cell)
    {
        //Cell Object 삭제 <- 눈이 보이는 항목
        //Contacts에 있는 contactList에서 contact 정보를 삭제 <- 데이터

        //팝업창 띄우기
        if (contacts.HasValue)
        {
            CellPopupView cellPopupView = Instantiate(cellPopupViewPrefab, mainManager.transform).GetComponent<CellPopupView>();
            //삭제할때 쓰는 코드  델리게이트 방식
            cellPopupView.cellPopupViewManagerDelegate = () =>
            {
                int cellIndex = cellList.IndexOf(cell);

                List<Contact> contactList = contacts.Value.contactList;
                contactList.RemoveAt(cellIndex);
                cellList.RemoveAt(cellIndex);
                Destroy(cell.gameObject);

                AdjustContent();
            };

            cellPopupView.Open();
        }
    }

}



