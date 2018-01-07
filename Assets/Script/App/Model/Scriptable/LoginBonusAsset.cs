using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    [System.Serializable]
    public class LoginBonusAsset : App.Model.Scriptable.AssetBase<LoginBonusAsset> {
        [SerializeField]public App.Model.Master.MLoginBonus[] loginbonuses;
	}
}