using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.View.Character;
using App.Controller.Common;


namespace App.Controller.Character{
    public class CCharacterListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            /*if (Global.SUser.self.characters == null)
            {
                SCharacter sCharacter = new SCharacter();
                yield return StartCoroutine(sCharacter.RequestList(Global.SUser.self.id));
                Global.SUser.self.characters = sCharacter.characters;
            }*/
            ScrollViewSets(content, childItem, Global.SUser.self.characters);
			yield return 0;
		}
        public void CharacterIconClick(VCharacterIcon vCharacterIcon){
            Request req = Request.Create("character_id", vCharacterIcon.ViewModel.CharacterId.Value);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.CharacterDetailDialog, req));
        }
	}
}