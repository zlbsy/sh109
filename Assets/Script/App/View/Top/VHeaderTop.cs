using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;
using System;

namespace App.View.Top{
    public class VHeaderTop : VBase {
        [SerializeField]public Text gold;
        [SerializeField]public Text silver;
        [SerializeField]public Text ap;
        #region VM处理
        public VMUser ViewModel { get { return (VMUser)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMUser oldVm = oldViewModel as VMUser;
            if (oldVm != null)
            {
                oldVm.Gold.OnValueChanged -= GoldChanged;
                oldVm.Silver.OnValueChanged -= SilverChanged;
                //oldVm.Ap.OnValueChanged -= ApChanged;
                //oldVm.Level.OnValueChanged -= ApChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Gold.OnValueChanged += GoldChanged;
                ViewModel.Silver.OnValueChanged += SilverChanged;
                //ViewModel.Ap.OnValueChanged += ApChanged;
                //ViewModel.Level.OnValueChanged += ApChanged;
            }
        }
        private void GoldChanged(int oldvalue, int newvalue)
        {
            gold.text = newvalue.ToString();
        }
        private void SilverChanged(int oldvalue, int newvalue)
        {
            silver.text = newvalue.ToString();
        }
        private void ApChanged(int oldvalue, int newvalue)
        {
            StopCoroutine("UpdateAp");
            StartCoroutine("UpdateAp");
        }
        #endregion
        public override void UpdateView(){
            gold.text = ViewModel.Gold.Value.ToString();
            silver.text = ViewModel.Silver.Value.ToString();
            StopCoroutine("UpdateAp");
            StartCoroutine("UpdateAp");
        }
        private IEnumerator UpdateAp()
        {
            int maxAp = Global.SUser.self.MaxAp;
            if (ViewModel.Ap.Value >= maxAp)
            {
                ap.text = string.Format("{0}/{1}", maxAp, maxAp);
                yield break;
            }
            int currentAp = Global.SUser.self.GetCurrentAp(App.Service.HttpClient.Now);
            ap.text = string.Format("{0}/{1}", currentAp, maxAp);
            if (currentAp >= maxAp)
            {
                yield break;
            }
            Countdown();
            yield return new WaitForSeconds(1f);
            StartCoroutine(UpdateAp());
        }
        private void Countdown(){
            DateTime lastApDate = ViewModel.LastApDate.Value;
            TimeSpan ts = App.Service.HttpClient.Now - lastApDate;
            int totalSeconds = (int)ts.TotalSeconds;
            int apSeconds = Global.Constant.recover_ap_time - totalSeconds % Global.Constant.recover_ap_time;
            ap.text = string.Format("{0}  <color=\"#FFFFFFFF\">{1}:{2}</color>", ap.text, (int)(apSeconds / 60), (apSeconds % 60).ToString("00"));
        }
        public void OpenMoneyShop(){
            App.Controller.shop.CShopDialog.Show();
        }
        public void OpenItemShop(){
            App.Controller.shop.CShopDialog.Show(App.Model.Master.MShopItem.ShopType.item);
        }
    }
}