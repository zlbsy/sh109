using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMItem : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<int> ItemId = new VMProperty<int>();
        public VMProperty<int> Cnt = new VMProperty<int>();
	}
}