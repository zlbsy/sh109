using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.View.Item;
using App.Controller.Common;
using App.Model.Scriptable;
using App.Util.Cacher;
using UnityEngine.UI;


namespace App.Controller.shop{
    public class CShopDialog : CDialog {
        [SerializeField]private Text title;
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        private App.Model.Master.MShopItem.ShopType[] types = new App.Model.Master.MShopItem.ShopType[]{
            App.Model.Master.MShopItem.ShopType.money,
            App.Model.Master.MShopItem.ShopType.item
        };
        private int typeIndex = 0;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            SShop sShop = new SShop();
            yield return StartCoroutine(sShop.Download(ShopAsset.Url, App.Util.Global.versions.shop, (AssetBundle assetbundle)=>{
                ShopAsset.assetbundle = assetbundle;
                ShopCacher.Instance.Reset(ShopAsset.Data.shopItems);
                ShopAsset.Clear();
            }));
            App.Model.Master.MShopItem.ShopType type = request.Get<App.Model.Master.MShopItem.ShopType>("type");
            typeIndex = System.Array.FindIndex(types, t=>t==type);
            ScrollViewSets();
            yield return 0;
        }
        public void ClickLeft(){
            typeIndex--;
            if (typeIndex < 0)
            {
                typeIndex = types.Length - 1;
            }
            ScrollViewSets();
        }
        public void ClickRight(){
            typeIndex++;
            if (typeIndex > types.Length - 1)
            {
                typeIndex = 0;
            }
            ScrollViewSets();
        }
        private void ScrollViewSets(){
            App.Model.Master.MShopItem.ShopType type = types[typeIndex];
            title.text = Language.Get(string.Format("shop_{0}", type.ToString()));
            ScrollViewSets(content, childItem, ShopCacher.Instance.GetAll(type));
        }
        public void ClickBuy(App.Model.Master.MShopItem mShopItem){
            if (mShopItem.type == App.Model.Master.MShopItem.ShopType.money)
            {
            }
            else
            {
                StartCoroutine(Buy(mShopItem));
            }
        }
        public IEnumerator Buy(App.Model.Master.MShopItem mShopItem){
            SShop sShop = new SShop();
            yield return StartCoroutine(sShop.RequestBuy("item", mShopItem.id));
        }
        public override void Close(){
            ShopCacher.Instance.Clear();
            base.Close();
        }
        public static void Show(App.Model.Master.MShopItem.ShopType type = App.Model.Master.MShopItem.ShopType.money){
            Request request = Request.Create("type",type);
            App.Util.SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ShopDialog, request));
        }
	}
}