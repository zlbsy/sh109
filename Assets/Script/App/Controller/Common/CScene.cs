using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;


namespace App.Controller.Common{
    /// <summary>
    /// 场景
    /// </summary>
    public class CScene : CBase {
        protected static string[] scriptWaitPaths = null;
        public void WaitScript(string[] paths){
            scriptWaitPaths = paths;
            this.StartCoroutine(WaitScriptContinue());
        }
        private IEnumerator WaitScriptContinue(){
            yield return new WaitForEndOfFrame();
            GameObject targetObj = GameObject.Find(scriptWaitPaths[0]);
            Debug.LogError("WaitScriptContinue : " + scriptWaitPaths[0] + ", targetObj : " + targetObj);
            if (targetObj == null)
            {
                yield return new WaitForEndOfFrame();
                App.Util.SceneManager.CurrentScene.StartCoroutine(WaitScriptContinue());
                yield break;
            }
            Transform target = targetObj.transform;
            int index = 1;
            while (index < scriptWaitPaths.Length)
            {
                Transform tran = target.Find(scriptWaitPaths[index]);
                if (tran == null)
                {
                    App.Util.SceneManager.CurrentScene.StartCoroutine(WaitScriptContinue());
                    yield break;
                }
                else
                {
                    target = tran;
                    index++;
                }
            }
            scriptWaitPaths = null;
            App.Util.LSharp.LSharpScript.Instance.Analysis();
            yield break;
        }
        public override IEnumerator Start()
        {
            //场景子窗口排序初始化
            App.Util.Global.DialogSortOrder = 0;
            //保存当前场景
            App.Util.SceneManager.CurrentScene = this;
            yield return StartCoroutine (OnLoad(App.Util.SceneManager.CurrentSceneRequest));
            App.Util.SceneManager.CurrentSceneRequest = null;
        }
        public override IEnumerator OnLoad( Request request ) 
        {  
            SUser sUser = Global.SUser;
            if (sUser != null && sUser.self != null && Global.Constant != null)
            {
                if (sUser.self.IsTutorial)
                {
                    this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.TutorialDialog));
                }
            }
            yield return 0;
            if (scriptWaitPaths != null && !(this is App.Controller.Battle.CBattlefield))
            {
                this.StartCoroutine(WaitScriptContinue());
            }
		}
	}
}