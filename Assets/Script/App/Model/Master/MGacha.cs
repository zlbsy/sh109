using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace App.Model.Master{
    [System.Serializable]
	public class MGacha : MBase {
        public MGacha(){
        }
        public string name;//
		//public int gold;
		//public int silver;
		public string from_time;
        public string to_time;
        public DateTime fromTime{
            get{ 
                return Convert.ToDateTime(from_time);
            }
        }
        public DateTime toTime{
            get{ 
                return Convert.ToDateTime(to_time);
            }
        }
        public MGachaChild[] childs;//
        public MGachaPrice[] prices;//
        public static Sprite GetIcon(int id){
            return App.Util.ImageAssetBundleManager.GetGachaIcon(id);
        }
	}
}