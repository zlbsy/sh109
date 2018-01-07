using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class AreaAsset : AssetBase<AreaAsset> {
        [SerializeField]public App.Model.Master.MArea[] areas;

	}
}