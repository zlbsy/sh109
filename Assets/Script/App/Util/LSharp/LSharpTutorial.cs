using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using App.Controller;

namespace App.Util.LSharp{
    public class LSharpTutorial : LSharpBase<LSharpTutorial> {
        public void Close(string[] arguments){
            App.Controller.Common.CDialog dialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.TutorialDialog);
            dialog.Close();
            LSharpScript.Instance.Analysis();
        }
        public void Clickmask(string[] arguments){
            string[] paths = arguments[0].Split('.');
            float x = float.Parse(arguments[1]);
            float y = float.Parse(arguments[2]);
            float w = float.Parse(arguments[3]);
            float h = float.Parse(arguments[4]);
            Transform target = GameObject.Find(paths[0]).transform;
            int index = 1;
            while (index < paths.Length)
            {
                Transform tran = target.Find(paths[index]);
                if (tran == null)
                {
                    LSharpScript.Instance.Analysis();
                    return;
                }
                target = tran;
                index++;
            }
            CTutorialDialog dialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.TutorialDialog) as CTutorialDialog;
            dialog.ShowFocus(target.position.x -w*0.5f + x, Camera.main.pixelHeight - target.position.y - h*0.5f + y, w, h);
        }
        public void Clickmask3d(string[] arguments){
            string[] paths = arguments[0].Split('.');
            float x = float.Parse(arguments[1]);
            float y = float.Parse(arguments[2]);
            float w = float.Parse(arguments[3]);
            float h = float.Parse(arguments[4]);
            Transform target = GameObject.Find(paths[0]).transform;
            int index = 1;
            while (index < paths.Length)
            {
                Transform tran = target.Find(paths[index]);
                if (tran == null)
                {
                    LSharpScript.Instance.Analysis();
                    return;
                }
                target = tran;
                index++;
            }
            CTutorialDialog dialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.TutorialDialog) as CTutorialDialog;
            int intDefault = LayerMask.NameToLayer("Default");
            //int intUI = LayerMask.NameToLayer("UI");
            Camera[] cameras = App.Util.SceneManager.CurrentScene.GetComponentsInChildren<Camera>();
            Camera camera3D = System.Array.Find(cameras, c=>c.gameObject.layer == intDefault);
            //Camera cameraUI = System.Array.Find(cameras, c=>c.gameObject.layer == intUI);
            Vector2 vec = RectTransformUtility.WorldToScreenPoint(camera3D, target.position);
            dialog.ShowFocus(vec.x + x, Camera.main.pixelHeight - vec.y + y, w, h);
        }
        public void Call(string[] arguments){
            string[] paths = arguments[0].Split('.');
            Transform target = GameObject.Find(paths[0]).transform;
            int index = 1;
            while (index < paths.Length)
            {
                Transform tran = target.Find(paths[index]);
                if (tran == null)
                {
                    LSharpScript.Instance.Analysis();
                    return;
                }
                target = tran;
                index++;
            }
            if (arguments.Length > 2)
            {
                string type = arguments[2];
                switch (type)
                {
                    case "int":
                        target.gameObject.SendMessage(arguments[1], int.Parse(arguments[3]));
                        break;
                    default:
                        target.gameObject.SendMessage(arguments[1], arguments[3]);
                        break;
                }
            }
            else
            {
                target.gameObject.SendMessage(arguments[1]);
            }
            LSharpScript.Instance.Analysis();
        }
        public void Wait(string[] arguments){
            string[] paths = arguments[0].Split('.');
            App.Util.SceneManager.CurrentScene.GetComponent<App.Controller.Common.CScene>().WaitScript(paths);
        }
        public void Waitbelong(string[] arguments){
            App.Model.Belong belong = (App.Model.Belong)System.Enum.Parse(typeof(App.Model.Belong), arguments[0]);
            App.Controller.Battle.CBattlefield cBattlefield = App.Util.SceneManager.CurrentScene as App.Controller.Battle.CBattlefield;
            if (cBattlefield == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBattlefield.StartCoroutine(WaitBelongRun(cBattlefield, belong));
        }
        private IEnumerator WaitBelongRun(App.Controller.Battle.CBattlefield cBattlefield, App.Model.Belong belong){
            Debug.LogError("WaitBelongRun " + cBattlefield.currentBelong + ", " + belong);
            while (cBattlefield.currentBelong != belong)
            {
                yield return new WaitForEndOfFrame();
            }
            LSharpScript.Instance.Analysis();
        }
        public void Camerato(string[] arguments){
            int id = int.Parse(arguments[0]);
            App.Controller.Common.CBaseMap cBasemap = SceneManager.CurrentScene as App.Controller.Common.CBaseMap;
            cBasemap.CameraTo(id);
        }
	}
}