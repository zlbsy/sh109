using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class TutorialAsset : AssetBase<TutorialAsset> {
        [SerializeField]public List<string> tutorial;
        public static string TutorialUrl(int id){
            return App.Service.HttpClient.assetBandleURL + "tutorial/tutorial_" + id + ".unity3d";
        }
	}
}