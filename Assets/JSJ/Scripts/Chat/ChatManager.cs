﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ChatManager : MonoBehaviourPun
{
    public static ChatManager instance;

    // Chat View
    public GameObject chatView;

    // ChatItem Prdfab
    public GameObject chatItemFactory;

    // Content 의 Transform
    public RectTransform trContent;

    // ChatView 의 Transform
    public RectTransform trChatView;

    // Input Field
    public TMP_InputField inputChat;

    // 채팅이 추가되기 전의 Content 의 H(높이) 값을 가지고 있는 변수
    float prevContentH;

    // 채팅 중인지
    bool isChatting = false;

    // 닉네임 색상
    Color nickNameColor;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 닉네임 색상을 랜덤하게 설정
        nickNameColor = Random.ColorHSV();

        // inputChat 이 선택될 때 호출되는 함수 등록
        inputChat.onSelect.AddListener(OnSelect);
        // inputChat 의 내용이 변경될 때 호출되는 함수 등록
        inputChat.onValueChanged.AddListener(OnValueChanged);
        // inputChat 엔터를 쳤을 때 호출되는 함수 등록
        inputChat.onSubmit.AddListener(OnSubmit);
        // inputChat 이 포커스를 잃었을 때 호출되는 함수 등록
        inputChat.onDeselect.AddListener(OnDeselect);

        // chatView 비활성화
        chatView.SetActive(false);
    }

    void Update()
    {
        
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnSelect ]
    // inputChat 이 선택되었을 때 호출되는 함수
    void OnSelect(string s)
    {
        // chatView 활성화
        chatView.SetActive(true);
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnValueChanged ]
    // 채팅 내용을 입력 중일 때
    void OnValueChanged(string s)
    {
        isChatting = true;
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnSubmit ]
    // Enter를 쳤을 때    
    void OnSubmit(string s)
    {
        // 만약에 s 의 길이가 0 이면 함수를 나가자.
        if (s.Length == 0) return;

        // 채팅 내용을 NickName : 채팅 내용
        // "<collor=#ffffff> 원하는 내용 </color>"
        string nick = "<color=#" + ColorUtility.ToHtmlStringRGB(nickNameColor) + ">" + PhotonNetwork.NickName + "</color>";
        string chat = nick + " : " + s;

        // AddChat RPC 함수 호출
        photonView.RPC(nameof(AddChat), RpcTarget.All, chat);

        if (photonView.IsMine)
        {
            // 강제로 inputChat 을 활성화 하자
            inputChat.ActivateInputField();
        }
    }

    // 채팅 추가 함수
    [PunRPC]
    void AddChat(string chat)
    {
        // 새로운 채팅이 추가되기 전의 Content 의 H 값을 저장
        prevContentH = trContent.sizeDelta.y;

        // ChatItem 하나 만들자 (부모를 ChatView 의 Content 로 하자)
        GameObject go = Instantiate(chatItemFactory, trContent);
        // ChatItem 컴포넌트 가져오자.
        ChatItem chatItem = go.GetComponent<ChatItem>();
        // 가져온 컴포넌트의 SetText 함수 실행
        chatItem.SetText(chat);
        // 가져온 컴포넌트의 onAutoScroll 변수에 AutoScrollBottom 을 설정
        chatItem.onAutoScroll = AutoScrollBottom;
        // inputChat 에 있는 내용을 초기화
        inputChat.text = "";
    }

    // 채팅 추가 되었을 때 맨밑으로 Content 위치를 옮기는 함수
    public void AutoScrollBottom()
    {
        // chatView 의 H 보다 content 의 H 값이 크다면 (스크롤이 가능한 상태라면)
        if(trContent.sizeDelta.y > trChatView.sizeDelta.y)
        {
            //trChatView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

            // 이전 바닥에 닿아있었다면
            if (prevContentH - trChatView.sizeDelta.y <= trContent.anchoredPosition.y)
            {
                // content 의 y값을 재설정한다.
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - trChatView.sizeDelta.y);
            }
        }
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnDeselect ]
    // inputChat 이 선택을 잃었을 때 호출되는 함수
    void OnDeselect(string s)
    {
        // chatView 비활성화
        chatView.SetActive(false);

        isChatting = false;
    }


    // isChatting 상태를 반환하는 함수
    public bool IsChatting()
    {
        return isChatting;
    }
}
