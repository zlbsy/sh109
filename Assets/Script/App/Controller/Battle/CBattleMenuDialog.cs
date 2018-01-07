using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using App.Controller.Common;


namespace App.Controller.Battle{
    public class CBattleMenuDialog : CDialog {
        [SerializeField]private Text title;
        [SerializeField]private Text bout;
        public override IEnumerator OnLoad( Request request ) 
        {  
            title.text = request.Get<string>("title");
            bout.text = request.Get<string>("bout");
            yield return StartCoroutine(base.OnLoad(request));

        }
        public void BattleOver(){
            this.StartCoroutine(BattleOverRun());
        }
        public IEnumerator BattleOverRun(){
            yield return this.StartCoroutine(App.Util.Global.SBattlefield.RequestBattleReset());
            (App.Util.SceneManager.CurrentScene as CBattlefield).BattleEnd();
        }
    }
}