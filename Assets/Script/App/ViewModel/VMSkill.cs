using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMSkill : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<int> SkillId = new VMProperty<int>();
        public VMProperty<int> Level = new VMProperty<int>();
        public VMProperty<bool> CanUnlock = new VMProperty<bool>();
	}
}