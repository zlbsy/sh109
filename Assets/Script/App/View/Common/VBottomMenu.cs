﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;
using App.Util;
using App.Controller;
using App.View.Common;

namespace App.View.Common{
    public class VBottomMenu : VBase{
        public virtual void Open(){
            HOTween.To(this.GetComponent<RectTransform>(), 0.2f, new TweenParms().Prop("anchoredPosition", new Vector2(0f,100f)));
        }
        public virtual void Close(System.Action complete){
            HOTween.To(this.GetComponent<RectTransform>(), 0.2f, new TweenParms().Prop("anchoredPosition", new Vector2(0f,0f)).OnComplete(()=>{
                if(complete != null){
                    complete();
                }
            }));
        }
	}
}