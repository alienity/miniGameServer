using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
	public Image blackImage;
	public CanvasGroup startCanvas;

	private void Start()
	{
		startCanvas.alpha = 0;
		Sequence seq = DOTween.Sequence();
		seq.Append(blackImage.DOFade(0f, 0.5f));
		seq.Append(blackImage.DOFade(0.5f, 1.5f));
		seq.Append(startCanvas.DOFade(1, 1.5f));


	}
}
