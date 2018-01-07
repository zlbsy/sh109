using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    [System.Serializable]
    public class MPromptMessage : MBase{
        public Texture2D image;
        public string message;
    }
    public class PromptMessageAsset : AssetBase<PromptMessageAsset> {
        [SerializeField]public MPromptMessage[] promptMessages;

	}
}