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


namespace App.Controller{
    /// <summary>
    /// 选择框
    /// </summary>
    public class CConfirmDialog : CDialog {
        [SerializeField]private Text title;
        [SerializeField]private Text message;
        [SerializeField]private Text yesText;
        [SerializeField]private Text noText;
        private System.Action _okEvent;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            message.text = request.Get<string>("message");
            _okEvent = request.Get<System.Action>("okEvent");
            if (request.Has("yesText"))
            {
                yesText.text = request.Get<string>("yesText");
            }
            if (request.Has("noText"))
            {
                noText.text = request.Get<string>("noText");
            }
            if (request.Has("title"))
            {
                title.text = request.Get<string>("title");
            }
            else
            {
                title.text = string.Empty;
            }
        }
        public void OkEvent(){
            _okEvent();
            this.Close();
        }
        /// <summary>
        /// 选择框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">文本</param>
        /// <param name="buttonText">yes按钮文本</param>
        /// <param name="buttonText">no按钮文本</param>
        /// <param name="closeEvent">选择确定时的回调</param>
        public static void Show(string title, string message, string yesText, string noText, System.Action okEvent){
            Request req = new Request();
            req.Set("message", message);
            if (!string.IsNullOrEmpty(title))
            {
                req.Set("title", title);
            }
            if (!string.IsNullOrEmpty(yesText))
            {
                req.Set("yesText", yesText);
            }
            if (!string.IsNullOrEmpty(noText))
            {
                req.Set("noText", noText);
            }
            if (okEvent != null)
            {
                req.Set("okEvent", okEvent);
            }
            App.Util.SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ConfirmDialog, req));
        }
        /// <summary>
        /// 选择框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">文本</param>
        /// <param name="closeEvent">选择确定时的回调</param>
        public static void Show(string title, string message, System.Action okEvent){
            Show(title, message, null, null, okEvent);
        }
        /// <summary>
        /// 选择框
        /// </summary>
        /// <param name="message">文本</param>
        /// <param name="closeEvent">选择确定时的回调</param>
        public static void Show(string message, System.Action okEvent){
            Show(null, message, null, null, okEvent);
        }
    }
}