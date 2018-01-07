
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;
using App.Util;
using App.Controller;
using App.View.Common;

namespace App.View.Battlefield{
    public class VBout : VBase{
		public int num{ get; set;}
		public App.Model.Belong belong{ get; set;}
        public virtual void Open(){
            HOTween.To(this.GetComponent<RectTransform>(), 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(0f,100f)));
        }
        public virtual void Close(System.Action complete){
            HOTween.To(this.GetComponent<RectTransform>(), 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(0f,0f)).OnComplete(()=>{
                if(complete != null){
                    complete();
                }
            }));
        }
	}
}