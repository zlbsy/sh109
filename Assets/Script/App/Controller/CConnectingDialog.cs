using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using UnityEngine.UI;
using App.Model.Scriptable;
using App.Controller.Common;


namespace App.Controller{
    public class CConnectingDialog : CDialog {
        [SerializeField]private Transform image;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            StartCoroutine(Run());
		}
        private IEnumerator Run(){
            image.eulerAngles =  new Vector3(0, 0, image.eulerAngles.z - 5);
            yield return new WaitForEndOfFrame();
            StartCoroutine(Run());
        }
        public static void ToShow(){
            SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ConnectingDialog));
        }
        public static void ToClose(){
            CDialog dialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.ConnectingDialog);
            if (dialog != null)
            {
                dialog.Close();
            }
            /*if (Global.SceneManager.CurrentDialog is CConnectingDialog)
            {
                Global.SceneManager.CurrentDialog.Close();
            }*/
        }
	}
}