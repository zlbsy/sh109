using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    [System.Serializable]
    public class EffectAsset : AssetBase<EffectAsset> {
        [SerializeField]public App.View.Effect.VEffectAnimation effectAnimation;
	}
}