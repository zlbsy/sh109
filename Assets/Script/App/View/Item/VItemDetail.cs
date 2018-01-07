using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;
using Holoville.HOTween;

namespace App.View.Item{
    public class VItemDetail : VBase {
        [SerializeField]private VItemIcon itemIcon;
        [SerializeField]private Text itemName;
        [SerializeField]private Text explanation;
        [SerializeField]private CanvasGroup canvasGroup;
        [SerializeField]private GameObject listButtons;
        [SerializeField]private GameObject useButtons;
        public bool UseOnly{
            get{ 
                return useButtons.activeSelf;
            }
            set{ 
                useButtons.SetActive(value);
                listButtons.SetActive(!value);
            }
        }
        #region VM处理
        public VMItem ViewModel { get { return (VMItem)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMItem oldVm = oldViewModel as VMItem;
            if (oldVm != null)
            {
                oldVm.ItemId.OnValueChanged -= ItemIdChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.ItemId.OnValueChanged += ItemIdChanged;
            }
        }
        private void ItemIdChanged(int oldvalue, int newvalue)
        {
            App.Model.Master.MItem mItem = ItemCacher.Instance.Get(newvalue);
            itemName.text = mItem.name;
            explanation.text = mItem.explanation;
        }
        public override void UpdateView(){
            ItemIdChanged(0, ViewModel.ItemId.Value);
            itemIcon.BindingContext = BindingContext;
            itemIcon.UpdateView();
            Debug.LogError("canvasGroup.alpha = " + canvasGroup.alpha);
            if (canvasGroup.alpha < 1)
            {
                HOTween.To(canvasGroup, 0.3f, new TweenParms().Prop("alpha", 1));
            }
        }
        #endregion
        void OnEnable(){
            canvasGroup.alpha = 0;
        }
        public void ClickSaleItem(){
            this.Controller.SendMessage("ClickSaleItem", ViewModel.Id.Value);
        }
        public void ClickUseItem(){
            this.Controller.SendMessage("ClickUseItem", ViewModel.Id.Value);
        }
        public void Close(){
            HOTween.To(canvasGroup, 0.3f, new TweenParms().Prop("alpha", 0));
        }
    }
}