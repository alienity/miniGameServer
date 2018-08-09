using UnityEngine;

public class StayUnRotated : MonoBehaviour
{
    private RectTransform rectTransform;
    private Quaternion zero = new Quaternion(0,0,0,1);

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.rotation = zero;
    }

}