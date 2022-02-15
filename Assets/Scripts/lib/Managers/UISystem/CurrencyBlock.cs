using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyBlock : MonoBehaviour
{

    [SerializeField] private bool hideOnZero;

    [SerializeField] private CurrencyType currencyType;

    public Transform IncomeImageTr => coinImage.transform;

    [SerializeField] private Image coinImage;
    [SerializeField] private Text coinText;

    //CollectAnim
    private bool isCollected;
    private Tween doCounterTween;

    private void OnEnable()
    {
        isCollected = true;
        coinText.text = ExchangeService.GetValue(currencyType).ToString();
        ExchangeService.OnDoExchange[currencyType].AddListener(UpdateUI);
        UpdateUI(currencyType, 0, 0);
    }
    private void UpdateUI(CurrencyType currencyType, int arg1, int arg2)
    {
        coinText.text = ExchangeService.GetValue(currencyType).ToString();
        if (hideOnZero)
            gameObject.SetActive(ExchangeService.GetValue(currencyType) == 0 ? false : true);
    }

    public void UpdateIncomeTextAnim(int accountCoin, int newCollectedCoin, float duration, Action callback)
    {
        doCounterTween?.Kill();

        isCollected = false;

        doCounterTween = coinText.DOCounter(accountCoin, accountCoin + newCollectedCoin, duration, false).OnComplete(() =>
       {
           callback?.Invoke();
           isCollected = true;
       });

        //In order not to lose the coin if it is closed before the animation ends.
        doCounterTween.OnKill(() =>
        {
            if (!isCollected)
            {
                callback?.Invoke();
                isCollected = true;
            }
        });
    }
}
