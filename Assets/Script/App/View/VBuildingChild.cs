using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View{
    public class VBuildingChild : VBase {
        [SerializeField]public Text buildingName;
        [SerializeField]public Image buildingIcon;
        #region VM处理
        public VMBuilding ViewModel { get { return (VMBuilding)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMBuilding oldVm = oldViewModel as VMBuilding;
            if (oldVm != null)
            {
                oldVm.Name.OnValueChanged -= NameChanged;
                oldVm.TileId.OnValueChanged -= TileIdChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Name.OnValueChanged += NameChanged;
                ViewModel.TileId.OnValueChanged += TileIdChanged;
            }
        }
        private void NameChanged(string oldvalue, string newvalue)
        {
            buildingName.text = Language.Get(newvalue);
        }
        private void TileIdChanged(int oldvalue, int newvalue)
        {
            buildingIcon.sprite = App.Model.Master.MTile.GetIcon(newvalue);
        }

        #endregion
        public void ClickChild(){
            (this.Controller as CBuildingDialog).ToBuild(ViewModel.Id.Value);
        }

    }
}