using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class ExpAsset : AssetBase<ExpAsset> {
        [SerializeField]public App.Model.Master.MExp[] exps;
	}
}