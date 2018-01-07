using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class MissionAsset : AssetBase<MissionAsset> {
        [SerializeField]public App.Model.Master.MMission[] missions;
	}
}