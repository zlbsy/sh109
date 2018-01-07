using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class WorldAsset : AssetBase<WorldAsset> {
        [SerializeField]public App.Model.Master.MWorld[] worlds;

	}
}