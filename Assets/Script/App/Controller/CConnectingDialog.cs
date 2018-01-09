using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Controller.Common;
using App.Util;


namespace App.Controller{
    public class CConnectingDialog : CDialog {
        private int _loadCounter = 0;
        private string delayClose = string.Empty;
        public override IEnumerator OnLoad( Request request ) {
            if (string.IsNullOrEmpty(this.delayClose)) {
                this.StopCoroutine(this.delayClose);
                this.delayClose = string.Empty;
            }
            _loadCounter++;
            yield return base.OnLoad(request);
        }
        public override void Close(){
            _loadCounter--;
            if (_loadCounter > 0)
            {
                return;
            }
            delayClose = "CloseSync";
            this.StartCoroutine(delayClose);
        }
        private IEnumerator CloseSync(){
            yield return new WaitForSeconds(0.2f);
            base.Close();
        }
        public static void ToShow(){
            if (SceneManager.CurrentScene == null)
            {
                return;
            }
            SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ConnectingDialog));
        }
        public static void ToClose(){
            CDialog dialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.ConnectingDialog);
            if (dialog != null)
            {
                dialog.Close();
            }
        }
    }
}