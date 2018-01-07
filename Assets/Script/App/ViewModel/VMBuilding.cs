using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;

namespace App.ViewModel
{
    public class VMBuilding : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<string> Name = new VMProperty<string>();
        public VMProperty<int> TileId = new VMProperty<int>();
	}
}