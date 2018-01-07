using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
    public class MShopItem : MBase {
        public enum ShopType
        {
            money,
            item
        }
        public MShopItem(){
        }
        public ShopType type;
        public MContent shop_content;
        public int gold;
        public int silver;
        public int money;
        public string limit_time;
        public int price{
            get{ 
                return gold + silver + money;
            }
        }
        public string priceType{
            get{ 
                if (gold > 0)
                {
                    return "gold";
                }else if (money > 0)
                {
                    return "money";
                }
                return "silver";
            }
        }
	}
}