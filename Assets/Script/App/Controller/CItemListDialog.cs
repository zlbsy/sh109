using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.View.Item;
using App.Controller.Common;


namespace App.Controller{
    public class CItemListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        [SerializeField]private VItemDetail itemDetail; 
        private App.Model.MItem[] items;
        private App.Model.Master.MItem.ItemType itemType;
        private System.Action<int> useItemEvent;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            if (Global.SUser.self.items == null)
            {
                SItem sItem = new SItem();
                yield return StartCoroutine(sItem.RequestList());
                Global.SUser.self.items = sItem.items;
            }
            items = Global.SUser.self.items;
            useItemEvent = null;
            if (request.Has("useItemEvent"))
            {
                useItemEvent = request.Get<System.Action<int>>("useItemEvent");
            }
            if (request.Has("itemType"))
            {
                itemType = request.Get<App.Model.Master.MItem.ItemType>("itemType");
                items = System.Array.FindAll(items, i=>i.Master.item_type == itemType);
            }
            itemDetail.UseOnly = request.Has("useOnly") && request.Get<bool>("useOnly");
            ScrollViewSets(content, childItem, items);
            yield return 0;
        }
        public void ItemIconClick(VItemIcon icon){
            App.Model.MItem mItem = System.Array.Find(items, i=>i.Id == icon.ViewModel.Id.Value);
            itemDetail.BindingContext = mItem.ViewModel;
            itemDetail.UpdateView();
        }
        public void ClickSaleItem(int id){
            Debug.LogError("ClickSaleItem");
        }
        public void ClickUseItem(int id){
            if (useItemEvent == null)
            {
                return;
            }
            App.Model.MItem mItem = System.Array.Find(items, i=>i.Id == id);
            CConfirmDialog.Show("确认",string.Format("物品使用后会消失，要使用 <color=\"#FF0000FF\">{0}</color> 吗？", mItem.Master.name),()=>{
                useItemEvent(id);
            });
        }
        public override void Close(){
            itemDetail.Close();
            base.Close();
        }
	}
}