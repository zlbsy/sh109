using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Gacha{
    public class VGachaButton : VBase {
        [SerializeField]public Text buttonName;
        [SerializeField]public Text price;
        [SerializeField]public Text labelFree;
        [SerializeField]public Text timeFree;
        private App.Model.Master.MGachaPrice gachaPrice;
        public override void UpdateView(App.Model.MBase model){
            App.Model.Master.MGachaPrice gachaPrice = model as App.Model.Master.MGachaPrice;
            this.gachaPrice = gachaPrice;
            buttonName.text = string.Format(Language.Get("gacha_button_label"), gachaPrice.cnt);
            price.gameObject.SetActive(true);
            price.text = gachaPrice.price.ToString();
            Image[] icons = price.GetComponentsInChildren<Image>();
            foreach (Image icon in icons)
            {
                icon.gameObject.SetActive(icon.gameObject.name == gachaPrice.price_type.ToString());
            }
            if (labelFree == null)
            {
                return;
            }
            App.Model.MGacha mGacha = System.Array.Find(Global.SGacha.gachas, _=>_.GachaId == gachaPrice.id);
            if (gachaPrice.free_count == 0 || mGacha.LimitCount == 0)
            {
                labelFree.gameObject.SetActive(false);
                timeFree.gameObject.SetActive(false);
                return;
            }
            price.gameObject.SetActive(false);
            App.Model.Master.MGacha gachaMaster = GachaCacher.Instance.Get(gachaPrice.id);
            this.StopAllCoroutines();
            this.StartCoroutine(UpdateFreetime(gachaMaster, mGacha));
        }
        private IEnumerator UpdateFreetime(App.Model.Master.MGacha gachaMaster, App.Model.MGacha mGacha)
        {
            System.TimeSpan timeSpan = App.Service.HttpClient.Now - mGacha.LastTime;
            if ((int)timeSpan.TotalMinutes > gachaPrice.free_time)
            {
                price.gameObject.SetActive(false);
                labelFree.gameObject.SetActive(true);
                timeFree.gameObject.SetActive(false);
                labelFree.text = string.Format(Language.Get("free_gacha"), mGacha.LimitCount, gachaPrice.free_count);
                yield break;
            }
            price.gameObject.SetActive(true);
            labelFree.gameObject.SetActive(false);
            timeFree.gameObject.SetActive(true);
            timeSpan = mGacha.LastTime.AddMinutes(gachaPrice.free_time) - App.Service.HttpClient.Now;
            int hours = (int)timeSpan.TotalHours;
            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;
            timeFree.text = string.Format(Language.Get("gacha_timefree_countdown"), hours.ToString("00"), minutes.ToString("00"), seconds.ToString("00"));
            yield return new WaitForSeconds(1f);
            StartCoroutine(UpdateFreetime(gachaMaster, mGacha));
        }
        public void OnClickGacha(){
            if (labelFree == null || !labelFree.gameObject.activeSelf)
            {
                if (gachaPrice.price_type == App.Model.PriceType.gold && Global.SUser.self.Gold < gachaPrice.price)
                {
                    CAlertDialog.Show("元宝不够");
                    return;
                }
                else if (gachaPrice.price_type == App.Model.PriceType.silver && Global.SUser.self.Silver < gachaPrice.price)
                {
                    CAlertDialog.Show("银两不够");
                    return;
                }
            }
            VGachaChild gachaChild = this.GetComponentInParent<VGachaChild>();
            gachaChild.OnClickGacha(gachaPrice.child_id, gachaPrice.cnt, labelFree != null && labelFree.gameObject.activeSelf);
        }
    }
}