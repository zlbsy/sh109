using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMLoginBonus : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<int> Day = new VMProperty<int>();
        public VMProperty<MContent[]> Contents = new VMProperty<MContent[]>();
        public VMProperty<bool> Received = new VMProperty<bool>();
	}
}