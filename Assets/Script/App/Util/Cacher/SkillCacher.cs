using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Util.Cacher{
    public class SkillCacher: CacherBase<SkillCacher, App.Model.Master.MSkill> {
        public App.Model.Master.MSkill Get(int id, int level){
            return System.Array.Find(datas, _=>_.id == id && _.level == level);
        }
    }
}