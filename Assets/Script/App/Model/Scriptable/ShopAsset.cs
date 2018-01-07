using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class ShopAsset : AssetBase<ShopAsset> {
        [SerializeField]public App.Model.Master.MShopItem[] shopItems;
	}
}