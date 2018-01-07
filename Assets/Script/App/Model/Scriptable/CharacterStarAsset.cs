using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class CharacterStarAsset : AssetBase<CharacterStarAsset> {
        [SerializeField]public App.Model.Master.MCharacterStar[] characterStars;
	}
}