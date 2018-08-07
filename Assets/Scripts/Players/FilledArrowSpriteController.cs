using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilledArrowSpriteController : MonoBehaviour
{
    // 外框精灵
    public SpriteRenderer boardSpriteRender;
    // 填充精灵
    public SpriteRenderer fillSpriteRender;
    // 外框精灵纹理
    private Material boardSpriteMaterial;
    // 填充纹理对象
    private Material fillSpriteMaterial;
    // 充满程度
	private float progress = 0.0f;

    // 最大拉伸距离比例
    public float arrowMaxRatio = 3.0f;
    // 最小拉伸距离比例
    public float arrowMinRatio = 1.6f;

    private void Awake()
	{
        boardSpriteMaterial = boardSpriteRender.material;
        fillSpriteMaterial = fillSpriteRender.material;
    }
	
    // 设置填充进度
	public void SetProgress(float progress)
	{
        fillSpriteMaterial.SetFloat("_Cutoff", 1 - progress);
    }

    // 设置箭头颜色
    public void SetArrowColor(Color color)
    {
        boardSpriteRender.color = color;
        fillSpriteRender.color = color; // 这一句话并没有什么意义
        fillSpriteMaterial.SetColor("_Color", color);
    }

    // 根据比例设置箭头长度
    public void SetArrowLen(float arrowRatio)
    {
        Vector2 arrowSize = boardSpriteRender.size;
        arrowSize.y = Mathf.Lerp(arrowMinRatio, arrowMaxRatio, arrowRatio);
        boardSpriteRender.size = arrowSize;
        fillSpriteRender.size = arrowSize;
    }

}
