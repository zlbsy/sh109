using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class WordAsset : AssetBase<WordAsset> {
        [SerializeField]public App.Model.Master.MWord[] words;

	}
}