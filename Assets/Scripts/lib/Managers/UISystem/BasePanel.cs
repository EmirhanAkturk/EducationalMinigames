using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public enum PanelAnimType
{
    None,
    Shring,
    Fade
}

public class PanelData
{

}

public abstract class BasePanel : MonoBehaviour
{
    [SerializeField] protected PanelAnimType animationType = PanelAnimType.None;
    [SerializeField] protected float animationDuration = 0.3f;
    [SerializeField] protected float zoomOutSize = 0.1f;
    [SerializeField] protected bool hideOnInputExit = true;

    public bool IsVisable;

    public GameObject panel;
    public Image Background;
    public float FadeAmount = 0.2f;

    protected virtual void Awake()
    {
        if (Background != null && hideOnInputExit)
            Background.gameObject.AddComponent<HideParentPanelOnClick>();
    }

    public void SetScaleSubPanel()
    {
        if (panel != null) panel.transform.localScale = new Vector3(zoomOutSize, zoomOutSize, zoomOutSize);
    }

    //single responsibility
    public virtual void ShowPanel(PanelData data)
    {
        if (animationType != PanelAnimType.None)
        {
            SetScaleSubPanel();
            gameObject.SetActive(true);
            PlayPanelOpenAnimation(animationType, animationDuration);
        }
        else
        {
            transform.gameObject.SetActive(true);
        }
        IsVisable = true;
    }

    //single responsibility
    public virtual void HidePanel()
    {
        if (animationType != PanelAnimType.None)
        {
            PlayPanelCloseAnimation(animationType, animationDuration);
        }
        else
        {
            transform.gameObject.SetActive(false);
        }

        IsVisable = false;
    }

    private void PlayPanelOpenAnimation(PanelAnimType animType, float duration)
    {
        KillPlayingTweens();
        if (animType == PanelAnimType.Shring)
        {
            panel?.transform.DOScale(1f, duration).SetUpdate(true);
            Background?.DOFade(FadeAmount, duration).From(0).SetUpdate(true).OnComplete(() =>
            {

            });
        }
        else if (animType == PanelAnimType.Fade)
        {
            Background?.DOFade(FadeAmount, duration).SetUpdate(true);
        }
    }

    private void PlayPanelCloseAnimation(PanelAnimType animType, float duration)
    {
        if (!gameObject.activeSelf) return;

        KillPlayingTweens();

        if (animType == PanelAnimType.Shring)
        {
            panel?.transform.DOScale(zoomOutSize, duration).SetUpdate(true);
            Background?.DOFade(0, duration).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        else if (animType == PanelAnimType.Fade)
        {
            Background?.DOFade(0, duration).SetUpdate(true).OnComplete(() =>
            {
                transform.gameObject.SetActive(false);
            });
        }
    }

    private void KillPlayingTweens()
    {
        if (panel != null)
            DOTween.Kill(panel.transform);
        DOTween.Kill(Background);
    }
}
