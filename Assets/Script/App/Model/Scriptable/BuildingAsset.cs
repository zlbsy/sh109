using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class BuildingAsset : AssetBase<BuildingAsset> {
        [SerializeField]public App.Model.Master.MBuilding[] buildings;

	}
}