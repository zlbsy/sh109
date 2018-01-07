using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class ItemAsset : AssetBase<ItemAsset> {
        [SerializeField]public App.Model.Master.MItem[] items;
	}
}