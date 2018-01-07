using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;
using App.View.Common;

namespace App.View.Shop{
    public class VShopItemChild : VBase {
        [SerializeField]private VContentsChild vContentsChild;
        [SerializeField]private Text lblName;
        [SerializeField]private Text lblMessage;
        [SerializeField]private Text lblPrice;
        [SerializeField]private GameObject[] units;
        private App.Model.Master.MShopItem mShopItem;
        public override void UpdateView(App.Model.MBase model){
            mShopItem = model as App.Model.Master.MShopItem;
            vContentsChild.UpdateView(mShopItem.shop_content);
            lblName.text = vContentsChild.ContentName;
            lblMessage.text = mShopItem.shop_content.message;
            lblPrice.text = mShopItem.price.ToString();
            string priceType = mShopItem.priceType;
            foreach (GameObject unit in units)
            {
                unit.SetActive(unit.name == priceType);
            }
        }
        public void ClickBuy(){
            this.Controller.SendMessage("ClickBuy", mShopItem);
        }
    }
}