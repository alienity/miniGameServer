using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
	public Image blackImage;
    public Image gameNameImage;
    public CanvasGroup startCanvas;

    private void Awake()
    {
        startCanvas.alpha = 0;
        gameNameImage.color = new Color(1f, 1f, 1f, 0f);
    }
    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(blackImage.DOFade(0f, 0.5f));
        seq.Append(blackImage.DOFade(0.5f, 1f));
        seq.Append(gameNameImage.DOFade(1f, 1f));
        seq.Append(gameNameImage.DOFade(0f, 1f));
        seq.Append(blackImage.DOFade(1f, 1f));    // 全部关闭
        seq.Append(startCanvas.DOFade(1, 1.5f));

        //Sequence DonQuixoeSeq = DOTween.Sequence();
        //seq.Append(gameNameImage.DOFade(1f, 0.5f));
        //seq.Append(gameNameImage.DOFade(0f, 1.5f));
    }
}
