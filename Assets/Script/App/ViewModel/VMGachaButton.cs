using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using System;

namespace App.ViewModel
{
    public class VMGachaButton : VMBase
    {
        public VMProperty<int> GachaId = new VMProperty<int>();
        public VMProperty<string> Name = new VMProperty<string>();
        public VMProperty<int> Cnt = new VMProperty<int>();
        public VMProperty<int> Price = new VMProperty<int>();
        public VMProperty<App.Model.PriceType> PriceType = new VMProperty<App.Model.PriceType>();
	}
}