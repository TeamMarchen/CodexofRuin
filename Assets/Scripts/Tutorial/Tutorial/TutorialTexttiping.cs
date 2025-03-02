using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialTexttiping : MonoBehaviour
{
    public TMP_Text text;
    public string[] prologDialo;

    private string[] dialoges;
    private int talkNum;
    private Coroutine typingCoroutine;

    private void OnEnable()
    {
        StartTalk(prologDialo);
    }

    private void OnDisable()
    {
        StopAllCoroutines(); // 비활성화될 때 코루틴 중지
    }

    public void StartTalk(string[] talks)
    {
        dialoges = talks;
        talkNum = 0;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine); // 기존 코루틴 중지

        typingCoroutine = StartCoroutine(LoopTalk());
    }

    private IEnumerator LoopTalk()
    {
        while (gameObject.activeSelf) // UI가 활성화 상태일 때만 반복
        {
            yield return StartCoroutine(Typing(dialoges[talkNum]));

            talkNum++;
            if (talkNum >= dialoges.Length)
                talkNum = 0; // 다시 처음으로 돌아감

            yield return new WaitForSeconds(1f); // 다음 대사 전 대기시간
        }
    }

    private IEnumerator Typing(string talk)
    {
        text.text = "";
        if (talk.Contains("  ")) talk = talk.Replace("  ", "\n");

        foreach (char letter in talk)
        {
            text.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.5f); // 대사 끝난 후 대기
    }
}
