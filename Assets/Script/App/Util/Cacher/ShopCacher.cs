using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Util.Cacher{
    public class ShopCacher: CacherBase<ShopCacher, App.Model.Master.MShopItem> {

        public App.Model.Master.MShopItem[] GetAll(App.Model.Master.MShopItem.ShopType type){
            return System.Array.FindAll(datas, d=>d.type == type);
        }

    }
}