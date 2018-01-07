using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Equipment{
    public class VEquipmentIcon : VBase {
        [SerializeField]protected Image icon;
        [SerializeField]protected GameObject[] stars;
        [SerializeField]protected Text level;
        [SerializeField]private bool hideLevel;
        [SerializeField]private bool clickDisabled = false;
        #region VM处理
        public VMEquipment ViewModel { get { return (VMEquipment)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMEquipment oldVm = oldViewModel as VMEquipment;
            if (oldVm != null)
            {
                oldVm.Level.OnValueChanged -= LevelChanged;
                //oldVm.EquipmentType.OnValueChanged -= StarChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Level.OnValueChanged += LevelChanged;
                //ViewModel.EquipmentType.OnValueChanged += StarChanged;
            }
        }
        private void LevelChanged(int oldvalue, int newvalue)
        {
            level.text = newvalue.ToString();
        }
        private void StarChanged(int oldvalue, int newvalue)
        {
            int count = 0;
            foreach (GameObject star in stars)
            {
                star.SetActive(count++ < newvalue);
            }
        }
        public override void UpdateView(){
            if (ViewModel != null && ViewModel.EquipmentId.Value > 0)
            {
                icon.gameObject.SetActive(true);
                level.transform.parent.gameObject.SetActive(true);
                stars[0].transform.parent.gameObject.SetActive(true);
                icon.sprite = ImageAssetBundleManager.GetEquipmentIcon(string.Format("{0}_{1}", ViewModel.EquipmentType.Value, ViewModel.EquipmentId.Value));
                LevelChanged(0, ViewModel.Level.Value);
                level.transform.parent.gameObject.SetActive(!hideLevel);
            }
            else
            {
                icon.gameObject.SetActive(false);
                level.transform.parent.gameObject.SetActive(false);
                stars[0].transform.parent.gameObject.SetActive(false);
            }
        }
        #endregion
        public void ClickChild(){
            if (clickDisabled)
            {
                return;
            }
            this.Controller.SendMessage("EquipmentIconClick", this);
        }
    }
}