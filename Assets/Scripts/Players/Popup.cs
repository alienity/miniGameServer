using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/*
 *挂载到对应的要缩放的东西上
 */
public class Popup : MonoBehaviour
{
    public bool popUp;
    public float popScale = 0.02f;

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
             popUp = false;
             Vector3 oldScale = rectTransform.localScale;
             Quaternion quaternion = rectTransform.rotation;
       
             Color tempColor = text.color;
             tempColor.a = 1;
             text.color = tempColor;
             Sequence seq = DOTween.Sequence();
             seq.Append(rectTransform.DOScale(popScale, 3f));

             seq.Insert(0, text.DOColor(Color.red, 0.5f)).OnComplete(delegate
             {
                 tempColor.a = 0;
                 text.color = tempColor;
                 rectTransform.localScale = oldScale;
                 rectTransform.rotation = quaternion;
             });

         }
     }
 }