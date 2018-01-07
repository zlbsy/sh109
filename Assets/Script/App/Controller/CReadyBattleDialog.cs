using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util.Cacher;
using App.View.Character;
using UnityEngine.UI;
using App.Util;
using App.Controller.Common;
using App.Model.Scriptable;

namespace App.Controller{
    public class CReadyBattleDialog : CDialog {
        [SerializeField]private Text title;
        [SerializeField]private Text labelAp;
        [SerializeField]private Transform selectCharacterContent;
        [SerializeField]private Transform selectShadowContent;
        [SerializeField]private GameObject selectShadow;
        [SerializeField]private Transform characterContent;
        [SerializeField]private GameObject characterIcon;
        App.Model.Master.MBattlefield battleFieldMaster;
        public override IEnumerator OnLoad( Request request ) 
        {  
            int battleId = request.Get<int>("battleId");
            battleFieldMaster = BattlefieldCacher.Instance.Get(battleId);
            title.text = battleFieldMaster.name;
            labelAp.text = string.Format("Ap:{0}", battleFieldMaster.ap);
            SelectCharacterContentInit();
            yield return this.StartCoroutine(base.OnLoad(request));
            if (App.Util.Global.SUser.self.characters == null)
            {
                SCharacter sCharacter = new SCharacter();
                yield return StartCoroutine(sCharacter.RequestList(App.Util.Global.SUser.self.id));
                App.Util.Global.SUser.self.characters = sCharacter.characters;
            }
            BaseCharacterList();
        }
        private void SelectCharacterContentInit(){
            for (int i = 0; i < battleFieldMaster.owns.Length; i++)
            {
                GameObject obj = Instantiate(selectShadow);
                obj.SetActive(true);
                obj.transform.SetParent(selectShadowContent);
                obj.transform.localScale = Vector3.one;
            }
        }
        private void BaseCharacterList(){
            ScrollViewSets(characterContent, characterIcon, App.Util.Global.SUser.self.characters);
        }
        public void CharacterIconClick(VCharacterIcon vCharacterIcon){
            if (!vCharacterIcon.isSelected && selectCharacterContent.childCount >= selectShadowContent.childCount)
            {
                CAlertDialog.Show("人数满了");
                return;
            }
            ToSelectCharacter(vCharacterIcon.ViewModel.CharacterId.Value, !vCharacterIcon.isSelected);
            vCharacterIcon.isSelected = !vCharacterIcon.isSelected;
        }
        private void ToSelectCharacter(int characterId, bool isSelected){
            if (isSelected)
            {
                App.Model.MCharacter character = System.Array.Find(App.Util.Global.SUser.self.characters, _ => _.CharacterId == characterId);
                GameObject shadowObj = Instantiate(selectShadow);
                shadowObj.SetActive(true);
                shadowObj.transform.SetParent(selectCharacterContent);
                shadowObj.transform.localScale = Vector3.one;
                GameObject obj = Instantiate(characterIcon);
                obj.transform.SetParent(shadowObj.transform);
                //obj.transform.localScale = Vector3.one * 0.65f;
                obj.transform.localScale = Vector3.one;
                VCharacterIcon vCharacterIcon = obj.GetComponent<VCharacterIcon>();
                vCharacterIcon.BindingContext = character.ViewModel;
                vCharacterIcon.UpdateView();
            }
            else
            {
                VCharacterIcon[] icons = selectCharacterContent.GetComponentsInChildren<VCharacterIcon>();
                VCharacterIcon icon = System.Array.Find(icons, _=>_.ViewModel.CharacterId.Value == characterId);
                GameObject.Destroy(icon.transform.parent.gameObject);
            }
        }
        public void BattleStart(){
            if (battleFieldMaster.ap > Global.SUser.self.GetCurrentAp(App.Service.HttpClient.Now))
            {
                CAlertDialog.Show("Ap不足");
                return;
            }
            VCharacterIcon[] icons = selectCharacterContent.GetComponentsInChildren<VCharacterIcon>();
            if (icons.Length == 0)
            {
                CAlertDialog.Show("请选择出战人员");
                return;
            }
            this.StartCoroutine(BattleStartRun(icons));
        }
        public IEnumerator BattleStartRun(VCharacterIcon[] icons){
            if (Global.SUser.self.BattlingId > 0)
            {
                yield return this.StartCoroutine(App.Util.Global.SBattlefield.RequestBattleReset());
            }
            yield return this.StartCoroutine(Global.SBattlefield.RequestBattleStart(battleFieldMaster.id));
            if (!Global.SBattlefield.battleStartResponse.result)
            {
                yield break;
            }
            yield return this.StartCoroutine(Global.SUser.Download(EffectAsset.Url, Global.versions.effect, (AssetBundle assetbundle)=>{
                EffectAsset.assetbundle = assetbundle;
                App.Controller.Battle.CBattlefield.effectAnimation = EffectAsset.Data.effectAnimation;
                App.Model.Scriptable.EffectAsset.Clear();
            }));
            List<int> characterIds = new List<int>();
            foreach (VCharacterIcon icon in icons)
            {
                characterIds.Add(icon.ViewModel.CharacterId.Value);
            }
            Request req = Request.Create("battlefieldId", battleFieldMaster.id, "characterIds", characterIds);
            if (App.Util.SceneManager.CurrentScene is CStage)
            {
                req.Set("fromScene", App.Util.SceneManager.Scenes.Stage);
                req.Set("fromRequest", (App.Util.SceneManager.CurrentScene as CStage).saveRequest);
            }
            else if (App.Util.SceneManager.CurrentScene is CTop)
            {
                req.Set("fromScene", App.Util.SceneManager.Scenes.Top);
                req.Set("fromRequest", null);
            }
            App.Util.SceneManager.LoadScene( App.Util.SceneManager.Scenes.Battlefield.ToString(), req);
        }
	}
}