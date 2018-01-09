using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using App.Model.Scriptable;
using App.Controller.Common;
using UnityEngine.UI;


namespace App.Controller{
    public class CLogo : CScene {
        [SerializeField]private InputField account;
        [SerializeField]private InputField password;
        [SerializeField]private Transform loginWindow;void Awake () 
        {
            Camera.main.orthographicSize = Screen.height / 2;
        }
        public override IEnumerator Start()
        {
            Global.Initialize();
            yield return StartCoroutine (base.Start());
        }
        public override IEnumerator OnLoad( Request request ) 
        {
			yield return 0;
		}
        public void ClearCacher(){
            Caching.ClearCache();
            PlayerPrefs.DeleteAll();
        }
        public void GameStart(){
            if (loginWindow.gameObject.activeSelf)
            {
                return;
            }
            bool hasAccount = PlayerPrefs.HasKey("account");
            if (hasAccount)
            {
                string accountStr = PlayerPrefs.GetString("account");
                string passwordStr = PlayerPrefs.GetString("password");
                StartCoroutine(ToLoginStart( accountStr, passwordStr ));
            }
            else
            {
                loginWindow.localScale = new Vector3(0f, 0f, 0f);
                loginWindow.gameObject.SetActive(true);
                Holoville.HOTween.HOTween.To(loginWindow, 0.2f, new Holoville.HOTween.TweenParms().Prop("localScale", Vector3.one));
            }
        }
        public void ToRegister(){
            StartCoroutine(ToRegisterStart( ));
        }
        public IEnumerator ToRegisterStart( ) 
        {  
            yield return AppInitialize.Initialize();
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Register.ToString() );
        }
        public void ToLogin( ) 
        {  
            StartCoroutine(ToLoginStart( account.text.Trim(), password.text.Trim() ));
        }
        public IEnumerator ToLoginStart( string accountStr, string passwordStr ) 
        {  
            CConnectingDialog.ToShow();
            yield return StartCoroutine (App.Util.Global.SUser.RequestLogin(accountStr, passwordStr));
            if (App.Util.Global.SUser.self == null)
            {
                CConnectingDialog.ToClose();
                yield break;
            }
            yield return AppInitialize.Initialize();
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
        }
	}
}