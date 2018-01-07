using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.UGUI{
    public class UGImage: UnityEngine.UI.Image {
        public bool tofullScreen = false;
        protected override void Start()
        {
            base.Start();
            if (tofullScreen)
            {
                int imgWidth = this.mainTexture.width;
                int imgHeight = this.mainTexture.height;
                int width = Screen.width;
                int height = Screen.height;
                this.rectTransform.sizeDelta = new Vector2(width, imgHeight * width / imgWidth);
            }
        }
	}
}