using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util.Cacher;
using App.View.Character;
using UnityEngine.UI;
using App.Util;
using App.Controller.Common;


namespace App.Controller.Register{
    public class CRegisterConfirmDialog : CDialog {
        [SerializeField]private InputField account;
        [SerializeField]private InputField password;
        [SerializeField]private InputField passwordCheck;
        [SerializeField]private InputField nameInput;
        private int characterId;
        public override IEnumerator OnLoad( Request request ) 
        {  
            characterId = request.Get<int>("characterId");

            yield return this.StartCoroutine(base.OnLoad(request));
        }
        public void Submit(){
            string accountText = account.text.Trim();
            if (string.IsNullOrEmpty(accountText) || accountText.Length < 6)
            {
                CAlertDialog.Show("账号长度不够");
                return;
            }
            string passwordText = password.text.Trim();
            if (string.IsNullOrEmpty(passwordText) || passwordText.Length < 8)
            {
                CAlertDialog.Show("密码长度不够");
                return;
            }
            string passwordCheckText = passwordCheck.text.Trim();
            if (passwordText != passwordCheckText)
            {
                CAlertDialog.Show("两次密码不一致");
                return;
            }
            string nameText = nameInput.text.Trim();
            if (string.IsNullOrEmpty(nameText) || nameText.Length < 2)
            {
                CAlertDialog.Show("名字长度不够");
                return;
            }
            this.StartCoroutine(ToSubmit(accountText, passwordText, nameText));
        }
        public IEnumerator ToSubmit(string accountText, string passwordText, string nameText){
            SRegister sRegister = new SRegister();
            yield return this.StartCoroutine(sRegister.RequestInsert(characterId, accountText, passwordText, nameText));
            if (sRegister.responseInsert.result)
            {
                CConnectingDialog.ToShow();
                yield return StartCoroutine (App.Util.Global.SUser.RequestLogin(account.text.Trim(), password.text.Trim()));
                if (App.Util.Global.SUser.self == null)
                {
                    CConnectingDialog.ToClose();
                    this.Close();
                    yield break;
                }
                yield return StartCoroutine(Global.SUser.RequestGet());
                App.Util.LSharp.LSharpScript.Instance.UpdatePlayer();
                App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Top.ToString() );
            }
        }
	}
}