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
        Invoke("StartMainScene", 1f);
    }
    public void StartMainScene()
    {
        SceneManager.LoadScene("Prolog01");
    }
    public void Ending()
    {
        SoundManager.Instance.Play("SFX_Scifi_Loading_Screen");
        Invoke("EndingCredit", 1f);
    }
    public void EndingCredit()
    {
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("StartScene");
    }
}
