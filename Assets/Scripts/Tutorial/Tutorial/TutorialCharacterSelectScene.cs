using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//���ѷα׿��� ĳ���� ���� UI������ �� �ѱ��
public class TutorialCharacterSelectScene : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("Prolog02");
    }

    public void SelectedCharacter()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void StartGo()
    {
        SceneManager.LoadScene("MainScene");
    }
}
