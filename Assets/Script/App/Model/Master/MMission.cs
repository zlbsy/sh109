using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace App.Model.Master{
    [System.Serializable]
    public class MMission : MBase {
        public enum MissionType
        {
            normal,
            daily,
            events
        }
        public MMission(){
        }
        public string name;//
        public MissionType mission_type;
        public string start_time;
        public string end_time;
        public DateTime startTime{
            get{ 
                return Convert.ToDateTime(start_time);
            }
        }
        public DateTime endTime{
            get{ 
                return Convert.ToDateTime(end_time);
            }
        }
        public int parent_id;
        public int battle_id;
        public string story_progress;
        public int level;
        public int character_count;
        public int battle_count;
        public string message;
        public MContent[] rewards;
	}
}