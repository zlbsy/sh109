using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.View.Item;
using App.Controller.Common;


namespace App.Controller.LoginBonus{
	public class CLoginBonusGetDialog : CDialog {
		[SerializeField]private Text title;
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            App.Model.Master.MLoginBonus loginBonuses = request.Get<App.Model.Master.MLoginBonus>("loginBonuses");
            ScrollViewSets(content, childItem, loginBonuses.contents);
            if (request.Has("review") && request.Get<bool>("review"))
            {
                title.text = "登录奖励确认";
            }
            else
            {
                title.text = "每天登录奖励";
            }
        }
        public void ItemIconClick(int id){
            
        }
	}
}