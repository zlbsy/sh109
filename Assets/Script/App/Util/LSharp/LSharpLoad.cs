﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using System;
using System.Reflection;

namespace App.Util.LSharp{
    public class LSharpLoad : LSharpBase<LSharpLoad> {
        public void Script(string[] arguments){
            string url = string.Format(ScenarioUrl, arguments[0]);
            SceneManager.CurrentScene.StartCoroutine(Global.SUser.Download(url,App.Util.Global.versions.scenario, (AssetBundle assetbundle)=>{
                App.Model.Scriptable.ScenarioAsset.assetbundle = assetbundle;
                List<string> script = App.Model.Scriptable.ScenarioAsset.Data.script;
                LSharpScript.Instance.Analysis(script);
            }));
        }
        public static string ScenarioUrl{
            get{ 
                return App.Service.HttpClient.assetBandleURL + "scenario/scenario_{0}.unity3d";
            }
        }
	}
}