using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.View.Equipment;
using App.View.Character;
using App.View.Battlefield;
using App.Controller.Common;


namespace App.Controller.Battle{
    public class CBattleCharacterPreviewDialog : CSingleDialog {
        [SerializeField]private VBattleCharacterPreview vBattleCharacterPreview;
        [SerializeField]private VCharacterIcon icon;
        public override IEnumerator OnLoad( Request request ) 
        {  
            MCharacter mCharacter = request.Get<MCharacter>("character");
            VMInit(mCharacter);
            yield return StartCoroutine(base.OnLoad(request));
		}
        private void VMInit(MCharacter mCharacter){
            icon.BindingContext = mCharacter.ViewModel;
            icon.UpdateView();
            vBattleCharacterPreview.BindingContext = mCharacter.ViewModel;
            vBattleCharacterPreview.UpdateView();
        }
	}
}