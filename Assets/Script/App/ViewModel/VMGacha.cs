using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using System;

namespace App.ViewModel
{
    public class VMGacha : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<int> GachaId = new VMProperty<int>();
        public VMProperty<int> LimitCount = new VMProperty<int>();
        public VMProperty<DateTime> LastTime = new VMProperty<DateTime>();
	}
}