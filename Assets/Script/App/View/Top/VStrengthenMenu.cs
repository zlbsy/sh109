
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;
using App.Util;
using App.Controller;
using App.View.Common;

namespace App.View.Top{
    public class VStrengthenMenu : VBottomMenu{
        public void OpenBuildingDialog(){
            (this.Controller as CTop).CloseMenu(()=>{
                StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BuildingDialog));
            });
        }
        public override void Open()
        {
            HOTween.To(this.GetComponent<RectTransform>(), 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(0f,100f)));
        }
        public override void Close(System.Action complete){
            HOTween.To(this.GetComponent<RectTransform>(), 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(0f,0f)).OnComplete(()=>{
                if(complete != null){
                    complete();
                }
            }));
        }
        public void ClickLevelUp(){
            /*CTalkDialog.ToShow(2,"这是一个测试，测试对话。这是一个测试，测试对话。这是一个测试，测试对话。",true, ()=>{
                CTalkDialog.ToShow(1,"这是一个测试，测试对话。这是一个测试，测试对话。这是一个测试，测试对话。", false);
            });*/
        }
	}
}