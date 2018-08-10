using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIrregularShape : MonoBehaviour {

    private Image image;
	// Use this for initialization
	void Start () {
        image = this.GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.05f;

    }

}
