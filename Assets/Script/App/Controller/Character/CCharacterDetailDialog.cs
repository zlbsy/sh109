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
using App.Controller.Common;
using UnityEngine.UI;


namespace App.Controller.Character{
    public class CCharacterDetailDialog : CDialog {
        [SerializeField]private VCard vCard;
        [SerializeField]private App.View.Character.VCharacterDetail characterDetail;
        [SerializeField]private App.View.Character.VCharacterGrade characterGrade;
        [SerializeField]private App.View.Character.VCharacter vCharacter;
        [SerializeField]private VCharacterStatus vCharacterStatus;
        [SerializeField]private VEquipments vEquipment;
        [SerializeField]private VCharacterSkill vCharacterSkill;
        [SerializeField]private GameObject skillPreview;
        private GameObject currentContent;
        private VBase[] contents;
        App.Model.MCharacter character;
        SEquipment sEquipment = new SEquipment();
        public override IEnumerator OnLoad( Request request ) 
        {  
            HideSkillDetail();
            int characterId = request.Get<int>("character_id");
            if (Global.SUser.self.equipments == null)
            {
                yield return StartCoroutine(sEquipment.RequestList());
                Global.SUser.self.equipments = sEquipment.equipments;
            }
            character = System.Array.Find(Global.SUser.self.characters, _=>_.CharacterId == characterId);
            character.StatusInit();
            characterDetail.BindingContext = character.ViewModel;
            characterDetail.UpdateView();
            vCard.BindingContext = character.ViewModel;
            vCard.UpdateView();
            characterGrade.BindingContext = character.ViewModel;
            characterGrade.UpdateView();
            vCharacter.BindingContext = character.ViewModel;
            vCharacter.UpdateView();
            vCharacterStatus.BindingContext = character.ViewModel;
            vCharacterStatus.UpdateView();
            vEquipment.BindingContext = character.ViewModel;
            vEquipment.UpdateView();
            vCharacterSkill.BindingContext = character.ViewModel;
            vCharacterSkill.UpdateView();
            contents = new VBase[]{ vCharacterStatus, vEquipment, vCharacterSkill };
            ShowContentFromIndex(0);
            yield return StartCoroutine(base.OnLoad(request));
		}
        public void EquipmentIconClick(VCharacterEquipmentIcon vEquipmentIcon){
            System.Action<int> selectEvent = (int selectId)=>{
                StartCoroutine(EquipmentChange(selectId));
            };
            Request req = Request.Create("equipmentType", vEquipmentIcon.equipmentType, "selectEvent", selectEvent);
            if (vEquipmentIcon.equipmentType == App.Model.Master.MEquipment.EquipmentType.horse)
            {
                req.Set("moveType", vEquipmentIcon.moveType);
            }
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.EquipmentListDialog, req));
        }
        private IEnumerator EquipmentChange(int id){
            yield return StartCoroutine(sEquipment.RequestEquip(character.CharacterId, id));
            App.Model.MEquipment mEquipment = System.Array.Find(Global.SUser.self.equipments, _=>_.Id == id);
            Global.SUser.self.equipments = sEquipment.equipments;
            if (mEquipment.EquipmentType == App.Model.Master.MEquipment.EquipmentType.weapon)
            {
                character.Weapon = mEquipment.EquipmentId;
            }else if (mEquipment.EquipmentType == App.Model.Master.MEquipment.EquipmentType.clothes)
            {
                character.Clothes = mEquipment.EquipmentId;
            }else if (mEquipment.EquipmentType == App.Model.Master.MEquipment.EquipmentType.horse)
            {
                character.Horse = mEquipment.EquipmentId;
            }
            character.StatusInit();
        }
        public void SkillLevelUp(int id){
            App.Model.MSkill mSkill = System.Array.Find(character.Skills, s=>s.Id == id);
            if (Global.SUser.self.Silver < mSkill.Master.price)
            {
                CAlertDialog.Show("银两不够");
                return;
            }
            StartCoroutine(SkillLevelUpRun(id, mSkill));
        }
        private IEnumerator SkillLevelUpRun(int id, App.Model.MSkill mSkill){
            SSkill sSkill = new SSkill();
            yield return StartCoroutine(sSkill.RequestLevelUp(id));
            mSkill.Update(sSkill.skill);
        }
        public void SkillUnlock(int skill_id){
            App.Model.Master.MCharacterSkill[] skills = CharacterCacher.Instance.Get(character.CharacterId).skills;
            App.Model.Master.MCharacterSkill skill = System.Array.Find(skills, s => s.skill_id == skill_id);
            CConfirmDialog.Show("解锁新技能",string.Format("此技能需要消耗{0}个技能书！继续解锁吗？", skill.skill_point),()=>{
                StartCoroutine(SkillUnlockRun(skill_id));
            });
        }
        private IEnumerator SkillUnlockRun(int skill_id){
            App.Model.Master.MCharacterSkill[] skills = CharacterCacher.Instance.Get(character.CharacterId).skills;
            App.Model.Master.MCharacterSkill skill = System.Array.Find(skills, s => s.skill_id == skill_id);
            if (Global.SUser.self.items == null)
            {
                SItem sItem = new SItem();
                yield return StartCoroutine(sItem.RequestList());
                Global.SUser.self.items = sItem.items;
            }
            App.Model.MItem mItem = System.Array.Find(Global.SUser.self.items, i=>i.Master.item_type == App.Model.Master.MItem.ItemType.skillPoint);
            if (mItem == null || skill.skill_point > mItem.Cnt)
            {
                CConfirmDialog.Show("确认","没有足够的技能书，无法解锁新技能！要购买技能书吗？",()=>{
                    
                });
                yield break;
            }
            SSkill sSkill = new SSkill();
            yield return StartCoroutine(sSkill.RequestUnlock(character.CharacterId, skill_id));
            character.Skills = sSkill.skills;
            Global.SUser.self.items = sSkill.items;
        }
        public void SkillLearn(){
            System.Action<int> useItemEvent = (id) =>
            {
                StartCoroutine(SkillLearnRun(id));
            };
            Request req = Request.Create("itemType", App.Model.Master.MItem.ItemType.skillBook, "useOnly", true, "useItemEvent", useItemEvent);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ItemListDialog, req));
        }
        private IEnumerator SkillLearnRun(int id){
            SSkill sSkill = new SSkill();
            yield return StartCoroutine(sSkill.RequestLeran(character.CharacterId, id));
            character.Skills = sSkill.skills;
            Global.SUser.self.items = sSkill.items;
            CDialog cDialog = Global.SceneManager.FindDialog(SceneManager.Prefabs.ItemListDialog);
            cDialog.Close();
        }
        public void ChangeContent(){
            int index = System.Array.FindIndex(contents, _=>_.gameObject.name == currentContent.name) + 1;
            ShowContentFromIndex(index);
        }
        public void ShowContentFromIndex(int index){
            System.Array.ForEach(contents, _=>_.gameObject.SetActive(false));
            if (index >= contents.Length)
            {
                index = 0; 
            }
            VBase view = contents[index];
            currentContent = view.gameObject;
            currentContent.SetActive(true);
            if (view is VCharacterStatus)
            {
                view.UpdateView();
            }
        }
        public void ChangeCharacterAction(){
            switch (character.Action)
            {
                case ActionType.idle:
                    character.Action = ActionType.move;
                    break;
                case ActionType.move:
                    character.Action = ActionType.attack;
                    break;
                case ActionType.attack:
                    character.Action = ActionType.block;
                    break;
                case ActionType.block:
                    character.Action = ActionType.hert;
                    break;
                case ActionType.hert:
                    character.Action = ActionType.idle;
                    break;
            }
        }
        public void ShowSkillDetail(int skillId){
            skillPreview.SetActive(true);
            App.Model.Master.MSkill skillMaster = App.Util.Cacher.SkillCacher.Instance.Get(skillId);
            RectTransform trans = skillPreview.GetComponent<RectTransform>();
            trans.position = new Vector3(trans.position.x, Input.mousePosition.y, 0f);
            skillPreview.SetActive(true);
            skillPreview.transform.Find("Name").GetComponent<Text>().text = skillMaster.name;
            skillPreview.transform.Find("Detailed").GetComponent<Text>().text = skillMaster.explanation;
            this.StopAllCoroutines();
            this.StartCoroutine(WaitForHideSkillDetail());
        }
        private IEnumerator WaitForHideSkillDetail(){
            yield return new WaitForSeconds(1f);
            HideSkillDetail();
        }
        public void HideSkillDetail(){
            skillPreview.SetActive(false);
        }
	}
}