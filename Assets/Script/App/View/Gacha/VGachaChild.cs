using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;
using App.Model.Master;
using System;

namespace App.View.Gacha{
    public class VGachaChild : VBase {
        [SerializeField]public Text gachaName;
        [SerializeField]public Image icon;
        [SerializeField]public VGachaButton[] buttons;
        #region VM处理
        public VMGacha ViewModel { get { return (VMGacha)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMGacha oldVm = oldViewModel as VMGacha;
            if (oldVm != null)
            {
                oldVm.LimitCount.OnValueChanged -= LimitCountChanged;
                oldVm.LastTime.OnValueChanged -= LastTimeChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.LimitCount.OnValueChanged += LimitCountChanged;
                ViewModel.LastTime.OnValueChanged += LastTimeChanged;
            }
        }
        private void LastTimeChanged(DateTime oldvalue, DateTime newvalue)
        {
            UpdateGachaButtons();
        }
        private void LimitCountChanged(int oldvalue, int newvalue)
        {
            UpdateGachaButtons();
        }
        private void UpdateGachaButtons()
        {
            App.Model.Master.MGacha gachaMaster = GachaCacher.Instance.Get(ViewModel.GachaId.Value);
            for (int i = 0; i < gachaMaster.prices.Length; i++)
            {
                MGachaPrice gachaPrice = gachaMaster.prices[i];
                VGachaButton button = buttons[i];
                button.UpdateView(gachaPrice);
            }
        }
        public override void UpdateView(){
            icon.sprite = App.Model.Master.MGacha.GetIcon(ViewModel.GachaId.Value);
            App.Model.Master.MGacha gachaMaster = GachaCacher.Instance.Get(ViewModel.GachaId.Value);
            gachaName.text = gachaMaster.name;
            this.gameObject.name = string.Format("Gacha_{0}", ViewModel.GachaId.Value == Global.Constant.tutorial_gacha ? "tutorial" : ViewModel.GachaId.Value.ToString());
            UpdateGachaButtons();
        }
        #endregion
        public void OnClickGacha(int priceId, int cnt, bool free_gacha){
            (this.Controller as App.Controller.Gacha.CGachaDialog).OnClickGacha(ViewModel.GachaId.Value, priceId, cnt, cnt == 1 && free_gacha);
        }
        public void OnClickDetail(){
            (this.Controller as App.Controller.Gacha.CGachaDialog).OnClickDetail(ViewModel.GachaId.Value);
        }
    }
}