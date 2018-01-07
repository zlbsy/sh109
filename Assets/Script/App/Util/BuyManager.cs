using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;

namespace App.Util{
    public class BuyManager{
        public static bool CanBuy(int price, App.Model.PriceType priceType = App.Model.PriceType.silver){
            MUser mUser = Global.SUser.self;
            if (priceType == App.Model.PriceType.gold)
            {
                return mUser.Gold >= price;
            }else if (priceType == App.Model.PriceType.silver)
            {
                return mUser.Silver >= price;
            }
            return false;
        }
    }
}