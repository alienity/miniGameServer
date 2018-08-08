using UnityEngine;

/*
 * 把这个脚本挂载到player上
 */
public class FollowPlayer : MonoBehaviour
{
    public float xOffset;
    public float yOffset;
    public RectTransform followerTransform;
 
    void Update ()
    {
        Vector2 player2DPosition = Camera.main.WorldToScreenPoint(transform.position);
        followerTransform.position = player2DPosition + new Vector2(xOffset, yOffset);
 
        if (player2DPosition.x > Screen.width || player2DPosition.x < 0 || player2DPosition.y > Screen.height || player2DPosition.y < 0)
        {
            followerTransform.gameObject.SetActive(false);
        }
        else
        {
            followerTransform.gameObject.SetActive(true);
        }
    }

}