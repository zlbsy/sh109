using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class LanguageAsset : AssetBase<LanguageAsset> {
        public static string WORD_URL{
            get{ 
                return App.Service.HttpClient.assetBandleURL + "wordasset.unity3d";
            }
        }
        public static string CHARACTER_WORD_URL{
            get{ 
                return App.Service.HttpClient.assetBandleURL + "characterwordasset.unity3d";
            }
        }
        [SerializeField]public App.Model.Master.MWord[] words;

	}
}