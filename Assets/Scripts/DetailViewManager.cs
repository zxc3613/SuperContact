﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SCIputField
{
    public static void SetImmutable(this InputField inputField, bool editMode)
    {
        if (editMode)
        {
            inputField.transform.Find("Text").GetComponent<Text>().color = Color.black;
        }
        else
        {
            inputField.transform.Find("Text").GetComponent<Text>().color = Color.white;
        }
        inputField.transform.Find("Placeholder").gameObject.SetActive(editMode);
        inputField.GetComponent<InputField>().interactable = editMode;
        inputField.GetComponent<Image>().enabled = editMode;
    }
}
public class DetailViewManager : ViewManager
{
    [HideInInspector] public Animator animator;
    [SerializeField] InputField nameInputField;
    [SerializeField] InputField phoneNumberInputField;
    [SerializeField] InputField emailInputField;
    [SerializeField] Button saveButtom;
    [SerializeField] GameObject thirdViewPrefab;

    public delegate void DetailViewManagerSaveDelegate(Contact contact);
    public DetailViewManagerSaveDelegate saveDelegate;

    public Contact? contact;
    bool editMode = true;

    void ToggleEditMode(bool updateInputField = false)
    {
        editMode = !editMode;

        //저장버튼
        saveButtom.gameObject.SetActive(editMode);

        nameInputField.SetImmutable(editMode);
        phoneNumberInputField.SetImmutable(editMode);
        emailInputField.SetImmutable(editMode);

        if (editMode)
        {
            rightNavgationViewButton.SetTitle("취소");
        }
        else
        {
            rightNavgationViewButton.SetTitle("편집");

            if (contact.HasValue && updateInputField == false)
            {
                //데이터 화면 출력
                Contact contactValue = contact.Value;
                nameInputField.text = contactValue.name;
                phoneNumberInputField.text = contactValue.phoneNumber;
                emailInputField.text = contactValue.email;
            }
        }
    }
    private void Awake()
    {
        // Title 지정
        title = "상세화면";
        // Add 버튼 지정
        rightNavgationViewButton = Instantiate(buttonPrefab).GetComponent<SCButton>();
        rightNavgationViewButton.SetTitle("편집");
        rightNavgationViewButton.SetOnClickAction(() =>
        {
            ToggleEditMode();
        });
    }
    private void Start()
    {
        ToggleEditMode();
    }

    private void OnDestroy()
    {
        Destroy(rightNavgationViewButton.gameObject);
    }

    public void Save()
    {
        //사용자가 입력한 정보를 바탕으로 Contact 객체 만들고 만들어진 Contact 객체를 ScrollView에게 전달하면 끝
        //편집모드를 해제
        Contact newContact = new Contact();
        newContact.name = nameInputField.text;
        newContact.phoneNumber = phoneNumberInputField.text;
        newContact.email = emailInputField.text;


        saveDelegate?.Invoke(newContact);

        ToggleEditMode(true);
    }
    //3번쨰 화면
    public void Next()
    {
        ThirdViewManager thirdViewManager = Instantiate(thirdViewPrefab).GetComponent<ThirdViewManager>();
        mainManager.PresentViewManager(thirdViewManager);
    }
}