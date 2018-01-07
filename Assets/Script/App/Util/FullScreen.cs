using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Util{
    public class FullScreen: MonoBehaviour {

        void Start()
        {
            Image image = this.GetComponent<Image>();
            int imgWidth = image.mainTexture.width;
            int imgHeight = image.mainTexture.height;
            int width = Screen.width;
            int height = Screen.height;
            Debug.LogError(imgWidth + ", "+imgHeight + ", " + width + ", " + height);
            image.rectTransform.sizeDelta = new Vector2(width, imgHeight * width / imgWidth);
        }
	}
}