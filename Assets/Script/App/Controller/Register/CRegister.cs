using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View.Character;
using App.Controller.Common;
using App.Service;
using App.Model;
using UnityEngine.UI;
using App.Util;
using App.View.Equipment;
using App.View;


namespace App.Controller.Register{
    public class CRegister : CScene {
        [SerializeField]private VFace faceIcon;
        [SerializeField]private VCharacterStatus vCharacterStatus;
        [SerializeField]private App.View.Character.VCharacter vCharacter;
        private MCharacter[] allCharacters;
        private MCharacter[] characters;
        private int index = 0;
        private Gender gender = Gender.male;
        public override IEnumerator OnLoad( Request request ) 
        {  
            SRegister sRegister = new SRegister();
            yield return StartCoroutine(sRegister.RequestList());
            allCharacters = sRegister.characters;
            foreach (MCharacter character in allCharacters)
            {
                character.StatusInit();
            }
            ChangeGender();
            //yield return StartCoroutine(base.OnLoad(request));
		}
        public void CharacterSelectComplete(){
            MCharacter character = characters[index];
            Request req = Request.Create("characterId", character.CharacterId);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.RegisterConfirmDialog, req));
        }
        private void CharacterUpdate(){
            MCharacter character = characters[index];
            faceIcon.CharacterId = character.CharacterId;

            vCharacter.BindingContext = character.ViewModel;
            vCharacter.UpdateView();
            vCharacterStatus.BindingContext = character.ViewModel;
            vCharacterStatus.UpdateView();
        }
        public void ChangeGender(Toggle toggle = null){
            gender = (toggle == null || toggle.isOn) ? Gender.male : Gender.female;
            characters = System.Array.FindAll(allCharacters, chara => chara.Gender == gender);
            CharacterUpdate();
        }
        public void SelectLeft(){
            index -= 1;
            if (index < 0)
            {
                index = characters.Length - 1;
            }
            CharacterUpdate();
        }
        public void SelectRight(){
            index += 1;
            if (index > characters.Length - 1)
            {
                index = 0;
            }
            CharacterUpdate();
        }
        public void ReturnLogo(){
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Logo.ToString() );
        }
	}
}