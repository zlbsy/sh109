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


namespace App.Controller.Common{
    /// <summary>
    /// 可重复利用窗口
    /// </summary>
    public class CSingleDialog : CDialog {
        /// <summary>
        /// 覆盖CDialog的Delete，以便重复利用
        /// </summary>
        public override void Delete(){
            if (background != null)
            {
                Holoville.HOTween.HOTween.To(background, 0.1f, new Holoville.HOTween.TweenParms().Prop("color", new Color(0,0,0,0)).OnComplete(()=>{
                    this.gameObject.SetActive(false);
                    if(closeEvent != null){
                        closeEvent();
                    }
                }));
            }
            else
            {
                this.gameObject.SetActive(false);
                if(closeEvent != null){
                    closeEvent();
                }
            }
        }
	}
}