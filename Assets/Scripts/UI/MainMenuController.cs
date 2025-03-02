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
        view.ShopButton.onClick.AddListener(OnShopClicked);

        view.settingsButton.onClick.AddListener(OnSettingsClicked);
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

    void OnFriendClicked()
    {
        Debug.Log("동료창 열기");
    }
    void OnShopClicked()
    {
        Debug.Log("상점 열기");
    }
    void OnGuildClicked()
    {
        Debug.Log("길드 열기");
    }

}
