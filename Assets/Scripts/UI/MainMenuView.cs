using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuView : MonoBehaviour
{
    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI cashText;

    //바텀 UI
    public Button playButton; //스토리 시작 버튼
    public Button friendButton; //동료 버튼
    public Button InventButton;//가방 버튼
    public Button MissionButton; //미션 버튼
    public Button AchiveButton; //업적 버튼
    public Button ShopButton; //상점 버튼
    public Button PackageButton; //패키지 버튼
    //오른쪽 상단 UI
    public Button settingsButton; //설정 버튼
    public Button mailButton; //메일 버튼
    public Button newsButton; //알림 버튼
    //왼쪽 상단 UI
    public Button myRoomButton; //마이룸 버튼
    public Button GuildButton; //길드 버튼
    public void SetUserName(string userName)
    {
        userNameText.text = userName;
    }
  
}
