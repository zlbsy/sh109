using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using UnityEngine.UI;
using App.View.Character;
using App.Controller.Common;


namespace App.Controller{
    public class CTutorialDialog : CDialog {
        [SerializeField]private RectTransform rect;
        [SerializeField]private Image backgroundClip;
        private RectTransform icon;
        private RectTransform left;
        private RectTransform right;
        private RectTransform up;
        private RectTransform down;
        public override IEnumerator Start(){
            yield return this.StartCoroutine(OnLoad(null));
        }
        public override IEnumerator OnLoad( Request request ) 
        {  
            icon = rect.Find("Icon") as RectTransform;
            HideFocus();
            yield return StartCoroutine(base.OnLoad(request));
        }
        public void ClickFocus(){
            HideFocus();
            App.Util.LSharp.LSharpScript.Instance.Analysis();
        }
        public void HideFocus(){
            ShowFocus(-1f,-1f,1f,1f);
            backgroundClip.material.SetColor("_Color", new Color(0, 0, 0, 0));
            rect.gameObject.SetActive(false);
        }
        public void ShowFocus(float x, float y, float width, float height){
            int screenWidth = 640;
            float scale = screenWidth * 1f / Camera.main.pixelWidth;
            backgroundClip.material.SetFloat("_HoleWidth", 1f * ((width/scale) / Camera.main.pixelWidth));
            backgroundClip.material.SetFloat("_HoleHeight", 1f * ((height/scale) / Camera.main.pixelHeight));
            backgroundClip.material.SetFloat("_StartPosX", 1f * ((x + width * 0.5f) / Camera.main.pixelWidth));
            backgroundClip.material.SetFloat("_StartPosY", 1f + 1f * ((-y - height * 0.5f) / Camera.main.pixelHeight));
            backgroundClip.material.SetColor("_Color", new Color(0, 0, 0, 0.7f));
            float lightSize = 26f;
            rect.sizeDelta = new Vector2(width + lightSize, height + lightSize);
            rect.anchoredPosition = new Vector2((x + width * 0.5f)*scale,(-y - height * 0.5f)*scale);
            icon.sizeDelta = new Vector2(width + lightSize, height + lightSize);
            rect.gameObject.SetActive(true);
        }
	}
}