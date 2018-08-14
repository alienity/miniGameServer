using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilledArrowSpriteController : MonoBehaviour
{
    // 外框精灵
    public SpriteRenderer boardSpriteRender;
    // 填充精灵
    public SpriteRenderer fillSpriteRender;
    // 闪光纹理
    public SpriteRenderer shineSpriteRender;
    // 填充纹理对象
    private Material fillSpriteMaterial;
    // 充满程度
    [SerializeField] private float progress = 0.0f;

    // 最大拉伸距离比例
    public float arrowMaxRatio = 3.0f;
    // 最小拉伸距离比例
    public float arrowMinRatio = 1.6f;
    // 闪现时长
    public float shineDuring = 0.8f;
    
    [SerializeField] private Color curArrowColor;
    [SerializeField] private Vector2 arrowSize;

    private bool flashed = false;

    private void Awake()
	{
        fillSpriteMaterial = fillSpriteRender.material;
    }

    private void Start()
    {
        Color tmpColor = shineSpriteRender.color;
        tmpColor.a = 0;
        shineSpriteRender.color = tmpColor;

        arrowSize = boardSpriteRender.size;
    }

    // 设置填充进度
    public void SetProgress(float progress)
	{
        this.progress = progress;
        fillSpriteMaterial.SetFloat("_Cutoff", 1 - progress);

        if (Mathf.Abs(progress - 1f) < 0.1f)
        {
            Color tmpColor = shineSpriteRender.color;
            tmpColor.a = 1;
            shineSpriteRender.color = tmpColor;
        }
        else
        {
            Color tmpColor = shineSpriteRender.color;
            tmpColor.a = 0;
            shineSpriteRender.color = tmpColor;
        }
    }

    // 设置箭头颜色
    public void SetArrowColor(Color color)
    {
        curArrowColor = color;
        boardSpriteRender.color = color;
        shineSpriteRender.color = color;
        fillSpriteMaterial.SetColor("_Color", color);
    }

    // 根据比例设置箭头长度
    public void SetArrowLen(float arrowRatio)
    {
        Vector2 newArrowSize = arrowSize;
        newArrowSize.y = Mathf.Lerp(arrowMinRatio, arrowMaxRatio, arrowRatio) * arrowSize.y;
        boardSpriteRender.size = arrowSize;
        fillSpriteRender.size = arrowSize;
        shineSpriteRender.size = arrowSize;
    }

}
