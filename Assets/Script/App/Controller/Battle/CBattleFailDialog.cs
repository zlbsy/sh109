using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using App.Controller.Common;
using Holoville.HOTween;


namespace App.Controller.Battle{
    public class CBattleFailDialog : CDialog {
        [SerializeField]private GameObject buttonContents;
        public override IEnumerator OnLoad( Request request ) 
        {  
            buttonContents.SetActive(false);
            yield return StartCoroutine(base.OnLoad(request));

        }
        public void ButtonContentsShow(){
            if (buttonContents.activeSelf)
            {
                return;
            }
            buttonContents.SetActive(true);
            buttonContents.transform.localScale = Vector3.zero;
            HOTween.To(buttonContents.transform, 0.3f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInQuart));

        }
        public void BattleOver(){
            this.StartCoroutine(BattleOverRun());
        }
        public IEnumerator BattleOverRun(){
            yield return this.StartCoroutine(App.Util.Global.SBattlefield.RequestBattleReset());
            (App.Util.SceneManager.CurrentScene as CBattlefield).BattleEnd();
        }
        public void GoldRelive(){
            CAlertDialog.Show("待开发");
        }
        public void ItemRelive(){
            CAlertDialog.Show("待开发");
        }
    }
}