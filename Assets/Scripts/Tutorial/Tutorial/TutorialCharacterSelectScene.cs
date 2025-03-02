using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//프롤로그에서 캐릭터 선택 UI에서의 씬 넘기기
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
        Invoke("EndingCredit", 1f);
    }
    public void EndingCredit()
    {
        SceneManager.LoadScene("StartScene");
    }
}
