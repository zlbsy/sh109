using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Skill{
    public class VSkillChild : VBase {
        [SerializeField]private Image icon;
        [SerializeField]private Text skillName;
        [SerializeField]private Text skillLevel;
        [SerializeField]private Text condition;
        [SerializeField]private Text silver;

    }
}