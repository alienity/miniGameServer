using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/*
 *挂载到对应的要缩放的东西上
 */
public class Popup : MonoBehaviour
{
    public bool popUp;
    public float popScale = 2;

//    public float bigx = 10
    private RectTransform rectTransform;
    private Text text;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponent<Text>();
    }

    private void Update()
     {
         if (popUp)
         {
             Color tempColor = text.color;
             tempColor.a = 1;
             text.color = tempColor;
             popUp = false;
             enabled = true;
             Sequence seq = DOTween.Sequence();
             seq.Append(rectTransform.DOScale(popScale, 0.5f)).OnComplete(delegate
             {
                 tempColor.a = 0;
                 text.color = tempColor;
                 rectTransform.localScale = Vector3.one;
             });
         }
     }
 }