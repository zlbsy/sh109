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


namespace App.Controller.Webview{
    public class CWebviewDialog : CDialog {
        [SerializeField] private UniWebView UniWebView;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            string url = request.Get<string>("url");
            Show(url);
		}

        public void Show( string url )
        {
            /*if(WebView == null) {
                if(complete != null)complete();
                return;
            }*/
            UniWebView.CleanCache();
            UniWebView.AddUrlScheme("lufy");
            UniWebView.OnLoadComplete += OnLoadCompleteEvent; // 読み込み完了後のイベント登録
            /*
            UniWebView.OnReceivedMessage += OnReceivedMessage; // URLスキーマー取得のイベント登録
            UniWebView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished; // JavaScript実行完了時のイベント登録
            */
            UniWebView.SetBackgroundColor(Color.clear); // 透過処理を有効化
            UniWebView.backButtonEnable = false; // Androidの戻るボタン（ソフトウェア）を無効化
            UniWebView.SetShowSpinnerWhenLoading(false); // iOSの読み込み中スピナーを無効化
            UniWebView.toolBarShow = false; // iOSのツールブラウザーを無効化
            UniWebView.HideToolBar(false); // iOSのツールブラウザーを無効化
            UniWebView.zoomEnable = false; // iOSの ズームを無効化
            string userAgent = "Mozilla /5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B5110e Safari/601.1 ";
            UniWebView.SetUserAgent( userAgent );

            /*UniWebView.insets.left   = 0;
            UniWebView.insets.top    = 10;
            UniWebView.insets.right  = 0;*/
            UniWebView.insets.bottom = 106;

            RequestLoad(url);
            //if(complete != null)complete();
        }
        private void RequestLoad(string url)
        {
            if (!string.IsNullOrEmpty (url)) {
                UniWebView.url = url;
                UniWebView.Load();
            }
        }
        public override void Close(){
            GameObject.Destroy(UniWebView);
            base.Close();
        }
        private void OnLoadCompleteEvent(UniWebView webView, bool success, string errorMessage)
        {
            if (webView.currentUrl.Contains("about:blank")) {
                success = false;
            }
            if (success) {
                //webView.EvaluatingJavaScript("(function(){return document.title})()");
                webView.Show();
            } else {
                Debug.LogError("errorMessage="+errorMessage);
            }
        }
	}
}