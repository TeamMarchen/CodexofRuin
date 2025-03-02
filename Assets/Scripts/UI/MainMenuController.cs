using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    private MainMenuModel model;
    private MainMenuView view;

    void Start()
    {
        model = new MainMenuModel();
        view = FindObjectOfType<MainMenuView>();

        // Model과 View 연결
        model.OnUserNameChanged += view.SetUserName;

        // 초기 값 설정
        model.UserName = "Codex of Ruin";
        UIManager.Instance.Gold = 1000;
        UIManager.Instance.Cash = 500;

        // 버튼 이벤트 설정
        view.playButton.onClick.AddListener(OnPlayClicked);
        view.friendButton.onClick.AddListener(OnFriendClicked);
        view.InventButton.onClick.AddListener(OnInventClicked);
        view.MissionButton.onClick.AddListener(OnMissionClicked);
        view.AchiveButton.onClick.AddListener(OnAchiveClicked);
        view.ShopButton.onClick.AddListener(OnShopClicked);
        view.PackageButton.onClick.AddListener(OnPackageClicked);

        view.settingsButton.onClick.AddListener(OnSettingsClicked);
        view.mailButton.onClick.AddListener(OnMailClicked);
        view.newsButton.onClick.AddListener(OnNewsClicked);

        view.myRoomButton.onClick.AddListener(OnMyRoomClicked);
        view.GuildButton.onClick.AddListener(OnGuildClicked);
    }

    void OnPlayClicked()
    {
        Debug.Log("게임 시작!");
        SceneManager.LoadScene("Prolog01");
    }

    void OnSettingsClicked()
    {
        Debug.Log("설정 화면 열기");
    }

    void OnMailClicked()
    {
        Debug.Log("우편함 열기");
    }
    void OnFriendClicked()
    {
        Debug.Log("동료창 열기");
    }
    void OnInventClicked()
    {
        Debug.Log("가방 열기");
    }
    void OnMissionClicked()
    {
        Debug.Log("미션 열기");
    }
    void OnAchiveClicked()
    {
        Debug.Log("업적 열기");
    }
    void OnShopClicked()
    {
        Debug.Log("상점 열기");
    }
    void OnPackageClicked()
    {
        Debug.Log("패키지 열기");
    }

    void OnNewsClicked()
    {
        Debug.Log("알림 열기");
    }
    void OnMyRoomClicked()
    {
        Debug.Log("마이룸 열기");
    }
    void OnGuildClicked()
    {
        Debug.Log("길드 열기");
    }

}
