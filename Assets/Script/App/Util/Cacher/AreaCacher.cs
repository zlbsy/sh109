using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace App.Util.Cacher{
    public class AreaCacher: CacherBase<AreaCacher, App.Model.Master.MArea> {
        public App.Model.Master.MArea[] GetAreas(int worldId){
            return System.Array.FindAll(datas, _=>_.map_id == worldId);
        }
        /*public App.Model.Master.MArea GetArea(int stageId){
            return System.Array.Find(datas, area=>System.Array.Exists(area.stages, stage=>stage.id == stageId));
        }*/
    }
}