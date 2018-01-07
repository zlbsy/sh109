using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Avatar{
    [System.Serializable]
    public class AvatarAsset : App.Model.Scriptable.AssetBase<AvatarAsset> {
		[SerializeField]public MoveArms cavalry;
		[SerializeField]public MoveArms infantry;
		public AvatarAction GetAvatarAction(MoveType moveType, WeaponType weaponType, ActionType actionType, int index){
			MoveArms moveArms = moveType == MoveType.cavalry ? cavalry : infantry;
			return moveArms.GetAvatarAction (weaponType, actionType, index);
		}
	}
}