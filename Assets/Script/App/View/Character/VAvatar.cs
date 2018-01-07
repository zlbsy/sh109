using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Model.Master;
using App.Util.Cacher;
using App.Util.Battle;

namespace App.View.Character{
    public partial class VAvatar : VBase {

        [SerializeField]Anima2D.SpriteMeshInstance weapon;
        [SerializeField]Anima2D.SpriteMeshInstance[] weapons;
        [SerializeField]Anima2D.SpriteMeshInstance clothes;
        [SerializeField]Anima2D.SpriteMeshInstance[] clothesList;
        private int weaponIndex = 0;
        private int clothesIndex = 0;
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {
            base.OnBindingContextChanged(oldViewModel, newViewModel);
        }
        void OnGUI(){
            if (GUI.Button(new Rect(50, 100, 200, 30), "ChangeWeapon"))
            {
                if (weaponIndex >= weapons.Length)
                {
                    weaponIndex = 0;
                }
                weapon.spriteMesh = weapons[weaponIndex++].spriteMesh;
            }
            if (GUI.Button(new Rect(50, 150, 200, 30), "ChangeClothes"))
            {
                if (clothesIndex >= clothesList.Length)
                {
                    clothesIndex = 0;
                }
                clothes.spriteMesh = clothesList[clothesIndex++].spriteMesh;
            }
        }
    }
}