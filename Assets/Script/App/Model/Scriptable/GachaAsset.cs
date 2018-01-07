using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class GachaAsset : AssetBase<GachaAsset> {
        [SerializeField]public App.Model.Master.MGacha[] gachas;

	}
}