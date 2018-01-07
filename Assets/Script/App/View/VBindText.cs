using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;

namespace App.View{
    public class VBindText : VBase {
        [SerializeField]private string key;
        public IEnumerator Start()
        {
            if (string.IsNullOrEmpty(key))
            {
                yield break;
            }
            while (!App.Util.Language.IsReady)
            {
                yield return 0;
            }
            Text label = this.GetComponent<Text>();
            label.text = App.Util.Language.Get(key);Debug.Log("Start over");
        }
	}
}