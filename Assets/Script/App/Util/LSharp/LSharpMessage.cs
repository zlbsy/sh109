﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using App.Controller;
using App.Controller.Common;

namespace App.Util.LSharp{
    public class LSharpMessage : LSharpBase<LSharpMessage> {
        public void Show(string[] arguments){
            string message = arguments[0];
            System.Action onComplete = LSharpScript.Instance.Analysis;
            Request req = Request.Create("message",message,"closeEvent",onComplete);
            SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.MessageDialog, req));
        }
	}
}