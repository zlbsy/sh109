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
    public class VCharacterEquipmentIcon : VEquipmentIcon {
        [SerializeField]private Image equipmentTypeView;
        public App.Model.Master.MEquipment.EquipmentType equipmentType;
        public App.Model.MoveType moveType;
        #region VM处理
        //public VMEquipment ViewModel { get { return (VMEquipment)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {
            base.OnBindingContextChanged(oldViewModel, newViewModel);
        }
        public override void UpdateView(){
            if (equipmentTypeView != null)
            {
                equipmentTypeView.gameObject.SetActive(ViewModel == null);
            }
            base.UpdateView();
        }
        #endregion
        public void Init(){
            icon.gameObject.SetActive(false);
            level.transform.parent.gameObject.SetActive(false);
            stars[0].transform.parent.gameObject.SetActive(false);
            if (equipmentTypeView != null)
            {
                equipmentTypeView.gameObject.SetActive(true);
            }
        }
        /*public void ClickChild(){
            this.Controller.SendMessage("EquipmentIconClick", this);
        }*/
    }
}