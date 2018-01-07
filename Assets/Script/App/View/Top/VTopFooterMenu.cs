
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;
using App.Controller;
using App.Util;
using App.View.Common;

namespace App.View.Top{
    public class VTopFooterMenu : VBottomMenu{
        [SerializeField]private Transform mainButton;
        [SerializeField]private RectTransform mainMenu;
        public void OpenMenu(){
            if (mainButton.transform.localRotation.z == 0)
            {
                (this.Controller as CTop).OpenMenu(this);
                //HOTween.To(mainMenu.transform, 0.3f, new TweenParms().Prop("localRotation", new Vector3(0,0,45f)));
            }
            else
            {
                (this.Controller as CTop).CloseMenu();
                //HOTween.To(mainMenu.transform, 0.3f, new TweenParms().Prop("localRotation", new Vector3(0,0,0)));
            }
        }
        public override void Open()
        {
            HOTween.To(mainButton, 0.3f, new TweenParms().Prop("localRotation", new Vector3(0,0,45f)));
            mainMenu.gameObject.SetActive(true);
            HOTween.To(mainMenu, 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(mainMenu.anchoredPosition.x,100f)));
        }
        public override void Close(System.Action complete){
            HOTween.To(mainMenu, 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(mainMenu.anchoredPosition.x,0f)));
            HOTween.To(mainButton, 0.3f, new TweenParms().Prop("localRotation", new Vector3(0,0,0)).OnComplete(()=>{
                mainMenu.gameObject.SetActive(false);
                if(complete != null){
                    complete();
                }
            }));
        }
        public void OpenGacha(){
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.GachaDialog));
        }
	}
}