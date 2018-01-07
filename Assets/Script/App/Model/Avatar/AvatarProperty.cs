using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Avatar{
	[System.Serializable]
	public class AvatarProperty {
		public AvatarProperty(){
		}
		public int index = 0;
        public Vector3 position;
        public string position_value{
            set{ 
                string[] values = value.Split(',');
                position = new Vector3(float.Parse(values[0]), float.Parse(values[1]));
            }
            get{
                return string.Empty;
            }
        }
		public int sibling = -1;
        public Vector3 scale;
        public string scale_value{
            set{ 
                string[] values = value.Split(',');
                scale = new Vector3(float.Parse(values[0]), float.Parse(values[1]));
            }
            get{
                return string.Empty;
            }
        }
	}
}