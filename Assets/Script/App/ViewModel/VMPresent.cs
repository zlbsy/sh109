using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMPresent : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<System.DateTime> LimitTime = new VMProperty<System.DateTime>();
        public VMProperty<MContent> Content = new VMProperty<MContent>();
	}
}