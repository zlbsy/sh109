using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMBaseMap : VMBase
    {
        public VMProperty<int> MapId = new VMProperty<int>();
        public VMProperty<MTile[]> Tiles = new VMProperty<MTile[]>();
        public VMProperty<MCharacter[]> Characters = new VMProperty<MCharacter[]>();
	}
}