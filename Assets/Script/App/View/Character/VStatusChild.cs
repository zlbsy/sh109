using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Character{
    public class VStatusChild : VBase {
        [SerializeField]private Text statusLabel;
        [SerializeField]private Text statusValue;
        public void Set(string label, string value)
        {
            statusLabel.text = label;
            statusValue.text = value;
        }

    }
}