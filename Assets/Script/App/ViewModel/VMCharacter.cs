using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMCharacter : VMBase
    {
        public VMProperty<int> Id = new VMProperty<int>();
        public VMProperty<int> UserId = new VMProperty<int>();
        public VMProperty<int> Exp = new VMProperty<int>();
        public VMProperty<MSkill> CurrentSkill = new VMProperty<MSkill>();
        public VMProperty<int> CharacterId = new VMProperty<int>();
        public VMProperty<int> Hp = new VMProperty<int>();
        public VMProperty<int> Mp = new VMProperty<int>();
        public VMProperty<string> Name = new VMProperty<string>();
        public VMProperty<string> Nickname = new VMProperty<string>();
        public VMProperty<int> Level = new VMProperty<int>();
        public VMProperty<int> Fragment = new VMProperty<int>();
        public VMProperty<int> Star = new VMProperty<int>();
		public VMProperty<int> Head = new VMProperty<int>();
        public VMProperty<int> Hat = new VMProperty<int>();
        public VMProperty<bool> IsHide = new VMProperty<bool>();
        public VMProperty<int> Horse = new VMProperty<int>();
        public VMProperty<int> Weapon = new VMProperty<int>();
        public VMProperty<int> Clothes = new VMProperty<int>();
        public VMProperty<Mission> Mission = new VMProperty<Mission>();
		public VMProperty<WeaponType> WeaponType = new VMProperty<WeaponType>();
		public VMProperty<MoveType> MoveType = new VMProperty<MoveType>();
        public VMProperty<ActionType> Action = new VMProperty<ActionType>();
        public VMProperty<float> X = new VMProperty<float>();
        public VMProperty<float> Y = new VMProperty<float>();
        public VMProperty<int> CoordinateX = new VMProperty<int>();
        public VMProperty<int> CoordinateY = new VMProperty<int>();
        public VMProperty<Direction> Direction = new VMProperty<Direction>();
        public VMProperty<Belong> Belong = new VMProperty<Belong>();
        public VMProperty<MSkill[]> Skills = new VMProperty<MSkill[]>();
        public VMProperty<MCharacter> Target = new VMProperty<MCharacter>();
        public VMProperty<List<App.Model.MBase>> Aids = new VMProperty<List<App.Model.MBase>>();
        public VMProperty<List<App.Model.MBase>> Status = new VMProperty<List<App.Model.MBase>>();
        public VMProperty<MCharacterAbility> Ability = new VMProperty<MCharacterAbility>();
        public VMProperty<bool> ActionOver = new VMProperty<bool>();
	}
}