using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMBattlefield : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<int> BattlefieldId = new VMProperty<int>();
        public VMProperty<int> MapId = new VMProperty<int>();
        public VMProperty<MTile[]> Tiles = new VMProperty<MTile[]>();
        public VMProperty<MCharacter[]> Characters = new VMProperty<MCharacter[]>();
	}
}