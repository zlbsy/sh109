using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace App.Util.Cacher{
    public class BuildingCacher: CacherBase<BuildingCacher, App.Model.Master.MBuilding> {
        List<App.Model.MBuilding> buildings;
        public void SetBuildings(List<App.Model.MBuilding> buildints){
            this.buildings = buildints;
        }
        public App.Model.MBuilding GetBuilding(int buildingId){
            return buildings.Find(_=>_.Id == buildingId);
        }
        public List<App.Model.MBuilding> GetAllBuilding(){
            return buildings;
        }
        public App.Model.Master.MBuilding[] GetAll(int level){
            return System.Array.FindAll(datas, _=>_.from_level <= level && _.to_level >= level);
        }
    }
}