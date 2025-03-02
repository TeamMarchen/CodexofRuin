using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        StartCoroutine(DelayGo());
    }

    private IEnumerator DelayGo()
    {
        yield return new WaitForSeconds(1.5f);
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("Prolog01");
    }
}
