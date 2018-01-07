﻿using System.Collections;
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
    /// 消息框
    /// </summary>
    public class CAlertDialog : CDialog {
        [SerializeField]private Text title;
        [SerializeField]private Text message;
        [SerializeField]private Text buttonText;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            message.text = request.Get<string>("message");
            if (request.Has("buttonText"))
            {
                buttonText.text = request.Get<string>("buttonText");
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
        /// <summary>
        /// 消息框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">文本</param>
        /// <param name="buttonText">按钮文本</param>
        /// <param name="closeEvent">关闭消息框时的回调</param>
        public static void Show(string title, string message, string buttonText, System.Action closeEvent){
            Request req = new Request();
            req.Set("message", message);
            if (!string.IsNullOrEmpty(title))
            {
                req.Set("title", title);
            }
            if (!string.IsNullOrEmpty(buttonText))
            {
                req.Set("buttonText", buttonText);
            }
            if (closeEvent != null)
            {
                req.Set("closeEvent", closeEvent);
            }
            App.Util.SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.AlertDialog, req));
        }
        /// <summary>
        /// 消息框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">文本</param>
        /// <param name="closeEvent">关闭消息框时的回调</param>
        public static void Show(string title, string message, System.Action closeEvent){
            Show(title, message, null, closeEvent);
        }
        /// <summary>
        /// 消息框
        /// </summary>
        /// <param name="message">文本</param>
        /// <param name="closeEvent">关闭消息框时的回调</param>
        public static void Show(string message, System.Action closeEvent){
            Show(null, message, null, closeEvent);
        }
        /// <summary>
        /// 消息框
        /// </summary>
        /// <param name="message">文本</param>
        public static void Show(string message){
            Show(null, message, null, null);
        }
	}
}