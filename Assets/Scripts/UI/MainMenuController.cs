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

        // Model�� View ����
        model.OnUserNameChanged += view.SetUserName;

        // �ʱ� �� ����
        model.UserName = "Codex of Ruin";
        UIManager.Instance.Gold = 1000;
        UIManager.Instance.Cash = 500;

        // ��ư �̺�Ʈ ����
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
        Debug.Log("���� ����!");
        SceneManager.LoadScene("Prolog01");
    }

    void OnSettingsClicked()
    {
        Debug.Log("���� ȭ�� ����");
    }

    void OnMailClicked()
    {
        Debug.Log("������ ����");
    }
    void OnFriendClicked()
    {
        Debug.Log("����â ����");
    }
    void OnInventClicked()
    {
        Debug.Log("���� ����");
    }
    void OnMissionClicked()
    {
        Debug.Log("�̼� ����");
    }
    void OnAchiveClicked()
    {
        Debug.Log("���� ����");
    }
    void OnShopClicked()
    {
        Debug.Log("���� ����");
    }
    void OnPackageClicked()
    {
        Debug.Log("��Ű�� ����");
    }

    void OnNewsClicked()
    {
        Debug.Log("�˸� ����");
    }
    void OnMyRoomClicked()
    {
        Debug.Log("���̷� ����");
    }
    void OnGuildClicked()
    {
        Debug.Log("��� ����");
    }

}
