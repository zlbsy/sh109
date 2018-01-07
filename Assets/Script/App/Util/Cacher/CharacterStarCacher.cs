using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Util.Cacher{
    public class CharacterStarCacher: CacherBase<CharacterStarCacher, App.Model.Master.MCharacterStar> {
        public int Cost(int star){
            App.Model.Master.MCharacterStar model = System.Array.Find(this.datas, s=>s.star == star);
            return model == null ? 0 : model.cost;
        }
    }
}