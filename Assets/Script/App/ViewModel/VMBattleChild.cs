using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMBattleChild : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<int> BattlefieldId = new VMProperty<int>();
        public VMProperty<int> Level = new VMProperty<int>();
        public VMProperty<int> Star = new VMProperty<int>();
	}
}