using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using UnityEngine.UI;
using App.Controller.Common;


namespace App.Controller.Battle{
    public class CBattleSkillListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        [SerializeField]private GameObject preview;
        private MCharacter mCharacter;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));
            mCharacter = request.Get<MCharacter>("character");
            HidePreview();
            ScrollViewSets(content, childItem, System.Array.FindAll(mCharacter.Skills, (s)=>{
                App.Model.Master.MSkill skillMaster = s.Master;
                Debug.LogError("CBattleSkillListDialog : " + skillMaster.id + ", " + skillMaster.name);
                bool isType = App.Model.Master.MSkill.IsSkillType(skillMaster, SkillType.attack) || App.Model.Master.MSkill.IsSkillType(skillMaster, SkillType.magic) || App.Model.Master.MSkill.IsSkillType(skillMaster, SkillType.heal);
                if(!isType){
                    return false;
                }
                bool isWeaponSkill = App.Model.Master.MSkill.IsWeaponType(skillMaster, mCharacter.WeaponType);
                return isWeaponSkill;
            }));
        }
        public void SkillIconClick(int skillId){
            mCharacter.CurrentSkill = System.Array.Find(mCharacter.Skills, _=>_.SkillId == skillId);
            this.Close();
        }
        public void ShowPreview(int skillId){
            int index = System.Array.FindIndex(mCharacter.Skills, _=>_.SkillId == skillId);
            MSkill skill = mCharacter.Skills[index];
            RectTransform trans = preview.GetComponent<RectTransform>();
            trans.anchoredPosition = new Vector2(110 * index - 190, trans.anchoredPosition.y);
            preview.SetActive(true);
            preview.transform.Find("Name").GetComponent<Text>().text = skill.Master.name;
            preview.transform.Find("Detailed").GetComponent<Text>().text = skill.Master.explanation;
        }
        public void HidePreview(){
            preview.SetActive(false);
        }
	}
}