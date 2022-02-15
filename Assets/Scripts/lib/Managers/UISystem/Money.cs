using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : MonoBehaviour
{
    [SerializeField] private PoolType poolType;
    [SerializeField] private float tDrop;
    [SerializeField] private float timeToBar;
    [SerializeField] private float timeToBarDelay = 0.5f;
    [SerializeField] private float randomCircleRadius;
    [SerializeField] private float yTop;
    //[SerializeField] private TrailRenderer trail;
    [SerializeField] private float rotateSpeed = 12;
    [SerializeField] private bool isLookCam = true;

    private RectTransform RectTransform => rectTransform != null ? rectTransform : (rectTransform = GetComponent<RectTransform>());
    private bool isInitialized = false;
    private Camera camera;
    private Transform xpBar;
    private Vector3 targetBarPos;

    private int deadXP;

    private Vector3 defaultScale;
    private RectTransform rectTransform;

    public void Start()
    {
        camera = Camera.main;
        defaultScale = RectTransform.localScale;
        //trail.enabled = false;
        xpBar = FindObjectOfType<CurrencyBlock>()?.IncomeImageTr;
    }

    private void Update()
    {
        if (isLookCam && isInitialized && camera != null)
            transform.rotation = camera.transform.rotation;
    }

    public void Adjust(int _deadXP)
    {
        isInitialized = true;
        deadXP = _deadXP;
        StartCoroutine(StartEffect1());
    }

    public void FillEffect(Vector3 _targetBarPos, float _scatterDuration = .5f, float _timeToBarDelay = .5f, float _timeToBarDuration = .5f, float _randomCircleR = .5f, System.Action callback = null)
    {
        tDrop = _scatterDuration;
        timeToBarDelay = _timeToBarDelay;
        timeToBar = _timeToBarDuration;
        randomCircleRadius = _randomCircleR;
        targetBarPos = _targetBarPos;

        StartCoroutine(StartFillEffect());
    }

    public void PayEffect(Vector3 _targetBarPos, float _scatterDuration = .5f, float _timeToBarDelay = .5f, float _timeToBarDuration = .5f, float _randomCircleR = .5f, System.Action callback = null)
    {
        tDrop = _scatterDuration;
        timeToBarDelay = _timeToBarDelay;
        timeToBar = _timeToBarDuration;
        randomCircleRadius = _randomCircleR;
        targetBarPos = _targetBarPos;

        StartCoroutine(StartFillEffect(false, callback));
    }

    public void Embed()
    {
        //trail.enabled = false;
        PoolingSystem.Instance.Destroy(poolType, gameObject);
    }

    public void Reset()
    {
        //trail.enabled = false;
        PoolingSystem.Instance.Destroy(poolType, gameObject);
    }

    private IEnumerator StartEffect1()
    {
        float t1 = tDrop;
        float y1 = yTop;
        Vector2 randomMoveDirection = Random.insideUnitCircle * randomCircleRadius;
        Vector3 p1 = transform.localPosition + new Vector3(randomMoveDirection.x / 2.0f, 0, y1);

        transform.DOMove(p1, t1).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.DOMove(camera.ScreenToWorldPoint(xpBar.position), timeToBar).SetEase(Ease.Linear).OnComplete(() =>
            {
                EarnMoney();
                Embed();
            });
        });
        yield break;
    }

    private IEnumerator StartFillEffect(bool isEarn= true, System.Action callback=null)
    {
        Vector2 randomMoveDirection = Random.insideUnitCircle * randomCircleRadius;
        Vector3 targetPos = transform.position + new Vector3(randomMoveDirection.x, randomMoveDirection.y, 0);

        if (!isInitialized)
        {
            defaultScale = RectTransform.localScale;
            isInitialized = true;
        }

        RectTransform.localScale = defaultScale;

        transform.DOMove(targetPos, tDrop).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(tDrop + timeToBarDelay);

        transform.DOScale(RectTransform.localScale * .85f, timeToBar).SetEase(Ease.Linear);
        transform.DOMove(targetBarPos, timeToBar).SetEase(Ease.InQuad);

        yield return new WaitForSeconds(timeToBar);

        if(isEarn)
            EarnMoney();
        Embed();
        callback?.Invoke();
    }

    private void EarnMoney()
    {
        int xpValue;
        ExchangeService.DoExchange(CurrencyType.Coin,
            ConfigurationService.Configurations.MoneyPerOneBale, ExchangeMethod.UnChecked,
            out xpValue);
        EventService.OnMoneyUpdate.Invoke();
    }
}
