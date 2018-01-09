using System.Collections;
using System.Collections.Generic;
using App.Model;

namespace App.Util.Battle{
    public class SkillManagerParam{
        public App.Model.Master.SkillEffectSpecial special;
        public List<MCharacter> actionCharacterList;
        public MCharacter actionCharacter;
        public SkillManagerParam(App.Model.Master.SkillEffectSpecial special){
            this.special = special;
        }
    }
    public class SkillManager : SingleClass<SkillManager>{
        public void Handle(SkillManagerParam param){
            
        }
    }
}