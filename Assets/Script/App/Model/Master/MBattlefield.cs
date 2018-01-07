using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
	public class MBattlefield : MBase {
		public MBattlefield(){
        }
        public string name;
        public MBattleNpc[] enemys;
        public MBattleNpc[] friends;
        public MBattleOwn[] owns;
        public int[][] own_positions;
        public int world_id;
        public int ap;
        public int max_bout;
        public List<string> script;
        public App.Model.MTile[] tiles;
	}
}