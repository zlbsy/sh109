using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class StrategyAsset : AssetBase<StrategyAsset> {
        [SerializeField]public App.Model.Master.MStrategy[] strategys;

	}
}