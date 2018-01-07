using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.View.Item;
using App.View.Equipment;
using App.Model;
using App.View.Character;

namespace App.View.Character{
    public class VSelectCharacterIcon : VBase {
        [SerializeField]private VCharacterIcon vCharacterIcon;
        public override void UpdateView(MBase model){
            MCharacter mCharacter = model as MCharacter;
            vCharacterIcon.gameObject.SetActive(mCharacter != null);
            if (mCharacter != null)
            {
                vCharacterIcon.BindingContext = mCharacter.ViewModel;
                vCharacterIcon.UpdateView();
            }
            else
            {
                vCharacterIcon.BindingContext = null;
            }
        }
        public int CharacterIconId{
            get{
                if (!vCharacterIcon.gameObject.activeSelf)
                {
                    return 0;
                }
                return vCharacterIcon.ViewModel.CharacterId.Value;
            }
        }
    }
}