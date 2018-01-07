using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using App.Controller;
using App.Controller.Common;

namespace App.Util.LSharp{
    public class LSharpScreen : LSharpBase<LSharpScreen> {
        public void Fadein(string[] arguments){
            System.Action action = LSharpScript.Instance.Analysis;
            Request req = Request.Create("onLoadComplete", action, "closeEvent", action);
            SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BlackScreen, req));
        }
        public void Fadeout(string[] arguments){
            CDialog dialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.BlackScreen);
            dialog.Close();
        }
        public void Refresh(string[] arguments){
            CStage stage = App.Util.SceneManager.CurrentScene as CStage;
            SceneManager.CurrentScene.StartCoroutine(stage.ReLoad(()=>{
                LSharpScript.Instance.Analysis();
            }));
        }
	}
}