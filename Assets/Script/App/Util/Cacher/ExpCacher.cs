using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Util.Cacher{
    public class ExpCacher: CacherBase<ExpCacher, App.Model.Master.MExp> {
        public int MaxExp(int level){
            if (level == 0)
            {
                return 0;
            }
            App.Model.Master.MExp mExp = System.Array.Find(datas, d=>d.level == level);
            return mExp.value;
        }
    }
}