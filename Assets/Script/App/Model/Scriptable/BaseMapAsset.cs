using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class BaseMapAsset : AssetBase<BaseMapAsset> {
        [SerializeField]public App.Model.Master.MBaseMap[] baseMaps;

	}
}