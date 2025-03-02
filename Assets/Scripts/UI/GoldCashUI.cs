using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldCashUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI cashText;

    private void Start()
    {
        UIManager.Instance.OnGoldChanged += UpdateGoldUI;
        UIManager.Instance.OnCashChanged += UpdateCashUI;

        UpdateGoldUI(UIManager.Instance.Gold);
        UpdateCashUI(UIManager.Instance.Cash);
    }

    private void UpdateGoldUI(int gold)
    {
        goldText.text = $"{gold}";
    }

    private void UpdateCashUI(int cash)
    {
        cashText.text = $" {cash}";
    }

    private void OnDestroy()
    {
        // 씬 전환 시 이벤트 중복 구독 방지
        UIManager.Instance.OnGoldChanged -= UpdateGoldUI;
        UIManager.Instance.OnCashChanged -= UpdateCashUI;
    }
}
