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
using App.Controller.Common;


namespace App.Controller.Battle{
    public class CBattleListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        //private SBattlefield sBattlefield;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            if (Global.SUser.self.battlelist == null)
            {
                yield return StartCoroutine(Global.SBattlefield.RequestBattlelist());
            }
            if (Global.SUser.self.battlelist != null && Global.SUser.self.battlelist.Length > 0)
            {
                ScrollViewSets(content, childItem, Global.SUser.self.battlelist);
            }
        }
        public void BattleChildClick(int id){
            MBattleChild mBattleChild = System.Array.Find(Global.SUser.self.battlelist, _=>_.Id == id);
            Request req = Request.Create("battleId", mBattleChild.BattlefieldId);
            App.Util.SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ReadyBattleDialog, req));
        }
	}
}