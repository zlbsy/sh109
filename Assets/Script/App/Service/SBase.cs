using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace App.Service{
	public class SBase {
        //public IEnumerator Download (string url, string path)
        public IEnumerator Download (string url, int ver, System.Action<AssetBundle> handle, bool destory = true)
        {
            Debug.Log("url="+url);
            bool showConnecting = false;
            if (App.Util.Global.SceneManager != null && !App.Util.Global.SceneManager.DialogIsShow(App.Util.SceneManager.Prefabs.ConnectingDialog) && !App.Util.Global.SceneManager.DialogIsShow(App.Util.SceneManager.Prefabs.LoadingDialog))
            {
                showConnecting = true;
                App.Controller.CConnectingDialog.ToShow();
            }
            var www = WWW.LoadFromCacheOrDownload(url, ver);
            while (!www.isDone)
            {
                if (App.Util.Global.SceneManager != null && App.Util.Global.SceneManager.CurrentDialog != null)
                {
                    App.Controller.CLoadingDialog.UpdatePlusProgress(www.progress);
                }
                if(!string.IsNullOrEmpty(www.error))
                {
                    Debug.Log(url);
                    Debug.LogError(www.error);
                    break;
                }
                yield return null;
            }
            if (showConnecting)
            {
                App.Controller.CConnectingDialog.ToClose();
            }
            //yield return www;
            if(!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                yield break;
            }
            AssetBundle assetBundle = www.assetBundle;
            handle(assetBundle);
            if (destory && assetBundle != null)
            {
                 assetBundle.Unload(false);
            }
            //www.assetBundle.Unload(false);
            /*using (WWW www = new WWW (url + "?time=" + System.DateTime.Now.GetHashCode())) {

                yield return www;

                File.Delete (path);
                File.WriteAllBytes (path, www.bytes);
            }
            yield return 0;*/
        }
        protected string ImplodeList<T>(List<T> objs){
            string res = "";
            string plus = "";
            foreach (T obj in objs)
            {
                res = res + plus + obj.ToString();
                plus = ",";
            }
            return res;
        }
        protected string ImplodeList<T>(T[] objs){
            string res = "";
            string plus = "";
            foreach (T obj in objs)
            {
                res = res + plus + obj.ToString();
                plus = ",";
            }
            return res;
        }
	}
}