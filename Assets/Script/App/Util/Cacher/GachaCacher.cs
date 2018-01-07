using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Service;

namespace App.Util.Cacher{
    public class GachaCacher: CacherBase<GachaCacher, App.Model.Master.MGacha> {

        public App.Model.Master.MGacha[] GetAllOpen(){
            if (Global.SUser.self.IsTutorial)
            {
                return System.Array.FindAll(datas, g=>g.id == Global.Constant.tutorial_gacha && g.fromTime <= HttpClient.Now && g.toTime >= HttpClient.Now);
            }
            else
            {
                return System.Array.FindAll(datas, g=>g.id != Global.Constant.tutorial_gacha && g.fromTime <= HttpClient.Now && g.toTime >= HttpClient.Now);
            }
        }
    }
}