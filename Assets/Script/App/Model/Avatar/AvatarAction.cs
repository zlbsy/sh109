using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Avatar{
	[System.Serializable]
	public class AvatarAction {
		public AvatarAction(){
		}
		public AvatarProperty horse;
		public AvatarProperty body;
		public AvatarProperty clothes;
		public AvatarProperty head;
		public AvatarProperty hat;
        public AvatarProperty weapon;
        public string clothesType;
	}
}