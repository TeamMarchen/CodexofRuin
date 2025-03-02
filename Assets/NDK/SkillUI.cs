using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private GameObject _skillButton1;
    [SerializeField] private GameObject _skillButton2;
    [SerializeField] private GameObject _skillButton3;

    [SerializeField] public Image _healthGauge;
    [SerializeField] private Image _expGauge;
    [SerializeField] private Image _mpGauge;
    [SerializeField] public Image _skill1CoolTime;
    [SerializeField] public Image _skill2CoolTime;
    [SerializeField] public Image _skill3CoolTime;

    //나중에 스킬 넣을 수 있도록 수정하기
    public void Initialize()
    {
        Debug.Assert(_skillButton1);
        Debug.Assert(_skillButton2);
        Debug.Assert(_skillButton3);
        
        Debug.Assert(_skill1CoolTime);
        Debug.Assert(_skill2CoolTime);
        Debug.Assert(_skill3CoolTime);
        
        _skillButton1.SetActive(false);
        _skillButton2.SetActive(false);
        _skillButton3.SetActive(false);
    }

    public void Unlock(int level)
    {
        if (level > 5)
        {
            _skillButton1.SetActive(true);
        }

        if (level > 10)
        {
            _skillButton2.SetActive(true);
        }

        if (level > 15)
        {
            _skillButton3.SetActive(true);
        }
    }

    public void OnHealthChanged(float gauge)
    {
        _healthGauge.fillAmount = gauge;
    }

    public void OnExpChanged(float gauge)
    {
        _expGauge.fillAmount = gauge;
    }

    public void OnMPChanged(float guage)
    {
        _mpGauge.fillAmount = guage;
    }
    
    
}
