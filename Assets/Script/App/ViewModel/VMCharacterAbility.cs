using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMCharacterAbility : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<int> PhysicalAttack = new VMProperty<int>();
        public VMProperty<int> MagicAttack = new VMProperty<int>();
        public VMProperty<int> PhysicalDefense = new VMProperty<int>();
        public VMProperty<int> MagicDefense = new VMProperty<int>();
        public VMProperty<int> HpMax = new VMProperty<int>();
        public VMProperty<int> MpMax = new VMProperty<int>();
        public VMProperty<int> Power = new VMProperty<int>();
        public VMProperty<int> Knowledge = new VMProperty<int>();
        public VMProperty<int> Speed = new VMProperty<int>();
        public VMProperty<int> Trick = new VMProperty<int>();
        public VMProperty<int> Endurance = new VMProperty<int>();
        public VMProperty<int> MovingPower = new VMProperty<int>();
        public VMProperty<int> Riding = new VMProperty<int>();
        public VMProperty<int> Walker = new VMProperty<int>();
        public VMProperty<int> Pike = new VMProperty<int>();
        public VMProperty<int> Sword = new VMProperty<int>();
        public VMProperty<int> LongKnife = new VMProperty<int>();
        public VMProperty<int> Knife = new VMProperty<int>();
        public VMProperty<int> LongAx = new VMProperty<int>();
        public VMProperty<int> Ax = new VMProperty<int>();
        public VMProperty<int> LongSticks = new VMProperty<int>();
        public VMProperty<int> Sticks = new VMProperty<int>();
        public VMProperty<int> Archery = new VMProperty<int>();
        public VMProperty<int> HiddenWeapons = new VMProperty<int>();
        public VMProperty<int> DualWield = new VMProperty<int>();
        public VMProperty<int> Magic = new VMProperty<int>();
        public VMProperty<int> ResistanceMetal = new VMProperty<int>();
        public VMProperty<int> ResistanceWood = new VMProperty<int>();
        public VMProperty<int> ResistanceWater = new VMProperty<int>();
        public VMProperty<int> ResistanceFire = new VMProperty<int>();
        public VMProperty<int> ResistanceEarth = new VMProperty<int>();
	}
}