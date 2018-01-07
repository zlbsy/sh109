using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;
using App.Util.Cacher;


namespace App.Model
{
    public class MCharacterAbility : MBase
    {
        public MCharacterAbility()
        {
            viewModel = new VMCharacterAbility();
        }

        public static MCharacterAbility Create(MCharacter mCharacter)
        {
            MCharacterAbility ability = new MCharacterAbility();
            ability.Update(mCharacter);
            return ability;
        }

        public VMCharacterAbility ViewModel { get { return (VMCharacterAbility)viewModel; } }

        public void Update(MCharacter mCharacter)
        {
            App.Model.Master.MCharacter master = mCharacter.Master;
            if (master == null)
            {
                return;
            }
            App.Model.MSkill[] skills = mCharacter.Skills;
            this.Power = master.power;
            this.Knowledge = master.knowledge;
            this.Speed = master.speed;
            this.Trick = master.trick;
            this.Endurance = master.endurance;
            this.MovingPower = master.moving_power;
            this.Riding = master.riding;
            this.Walker = master.walker;
            this.Pike = master.pike;
            this.Sword = master.sword;
            this.LongKnife = master.long_knife;
            this.Knife = master.knife;
            this.LongAx = master.long_ax;
            this.Ax = master.ax;
            this.LongSticks = master.long_sticks;
            this.Sticks = master.sticks;
            this.Archery = master.archery;
            this.HiddenWeapons = master.hidden_weapons;
            this.DualWield = master.dual_wield;
            this.ResistanceMetal += master.resistance_metal;
            this.ResistanceWood += master.resistance_wood;
            this.ResistanceWater += master.resistance_water;
            this.ResistanceFire += master.resistance_fire;
            this.ResistanceEarth += master.resistance_earth;
            int hp = master.hp;
            int mp = master.mp;
            if (skills != null)
            {
                foreach (App.Model.MSkill skill in skills)
                {
                    App.Model.Master.MSkill skillMaster = skill.Master;
                    if (skillMaster == null)
                    {
                        Debug.LogError("master.name=" + master.name+", "+skill.SkillId+","+skill.Level);
                        return;
                    }
                    if (!System.Array.Exists(skillMaster.types, s => s == SkillType.ability))
                    {
                        continue;
                    }
                    hp += skillMaster.hp;
                    mp += skillMaster.mp;
                    this.Power += skillMaster.power;
                    this.Knowledge += skillMaster.knowledge;
                    this.Speed += skillMaster.speed;
                    this.Trick += skillMaster.trick;
                    this.Endurance += skillMaster.endurance;
                    this.MovingPower += skillMaster.moving_power;
                    this.Riding += skillMaster.riding;
                    this.Walker += skillMaster.walker;
                    this.Pike += skillMaster.pike;
                    this.Sword += skillMaster.sword;
                    this.LongKnife += skillMaster.long_knife;
                    this.Knife += skillMaster.knife;
                    this.LongAx += skillMaster.long_ax;
                    this.Ax += skillMaster.ax;
                    this.LongSticks += skillMaster.long_sticks;
                    this.Sticks += skillMaster.sticks;
                    this.Archery += skillMaster.archery;
                    this.HiddenWeapons += skillMaster.hidden_weapons;
                    this.DualWield += skillMaster.dual_wield;
                    this.Magic += skillMaster.magic;
                    this.ResistanceMetal += skillMaster.resistance_metal;
                    this.ResistanceWood += skillMaster.resistance_wood;
                    this.ResistanceWater += skillMaster.resistance_water;
                    this.ResistanceFire += skillMaster.resistance_fire;
                    this.ResistanceEarth += skillMaster.resistance_earth;
                }
            }
            List<App.Model.Master.MEquipment> equipments = new List<App.Model.Master.MEquipment>();

            if (mCharacter.Weapon > 0)
            {
                equipments.Add(EquipmentCacher.Instance.GetEquipment(mCharacter.Weapon, App.Model.Master.MEquipment.EquipmentType.weapon));
            }
            if (mCharacter.Clothes > 0)
            {
                equipments.Add(EquipmentCacher.Instance.GetEquipment(mCharacter.Clothes, App.Model.Master.MEquipment.EquipmentType.clothes));
            }
            if (mCharacter.Horse > 0)
            {
                equipments.Add(EquipmentCacher.Instance.GetEquipment(mCharacter.Horse, App.Model.Master.MEquipment.EquipmentType.horse));
            }
            int physicalAttack = 0;
            int physicalDefense = 0;
            int magicAttack = 0;
            int magicDefense = 0;
            foreach (App.Model.Master.MEquipment equipment in equipments)
            {
                hp += equipment.hp;
                mp += equipment.mp;
                this.Power += equipment.power;
                this.Knowledge += equipment.knowledge;
                this.Speed += equipment.speed;
                this.Trick += equipment.trick;
                this.Endurance += equipment.endurance;
                this.MovingPower += equipment.moving_power;
                this.Riding += equipment.riding;
                this.Walker += equipment.walker;
                this.Pike += equipment.pike;
                this.Sword += equipment.sword;
                this.LongKnife += equipment.long_knife;
                this.Knife += equipment.knife;
                this.LongAx += equipment.long_ax;
                this.Ax += equipment.ax;
                this.LongSticks += equipment.long_sticks;
                this.Sticks += equipment.sticks;
                this.Archery += equipment.archery;
                this.HiddenWeapons += equipment.hidden_weapons;
                this.DualWield += equipment.dual_wield;
                this.Magic += equipment.magic;
                this.ResistanceMetal += equipment.resistance_metal;
                this.ResistanceWood += equipment.resistance_wood;
                this.ResistanceWater += equipment.resistance_water;
                this.ResistanceFire += equipment.resistance_fire;
                this.ResistanceEarth += equipment.resistance_earth;
                physicalAttack += equipment.physical_attack;
                physicalDefense += equipment.physical_defense;
                magicAttack += equipment.magic_attack;
                magicDefense += equipment.magic_defense;
            }

            this.HpMax = Mathf.FloorToInt(mCharacter.Level * (10 + this.Endurance * 0.2f) + hp);
            this.MpMax = Mathf.FloorToInt(mCharacter.Level * (5 + this.Knowledge * 0.1f) + mp);
            float moveTypeValue = (mCharacter.MoveType == MoveType.cavalry ? this.Riding : this.Walker);
            switch (mCharacter.WeaponType)
            {
                case WeaponType.archery:
                    moveTypeValue += this.Archery;
                    break;
                case WeaponType.pike:
                    moveTypeValue += this.Pike;
                    break;
                case WeaponType.sword:
                    moveTypeValue += this.Sword;
                    break;
                case WeaponType.longAx:
                    moveTypeValue += this.LongAx;
                    break;
                case WeaponType.ax:
                    moveTypeValue += this.Ax;
                    break;
                case WeaponType.longKnife:
                    moveTypeValue += this.LongKnife;
                    break;
                case WeaponType.shortKnife:
                    moveTypeValue += this.Knife;
                    break;
                case WeaponType.longSticks:
                    moveTypeValue += this.LongSticks;
                    break;
                case WeaponType.sticks:
                    moveTypeValue += this.Sticks;
                    break;
                case WeaponType.dualWield:
                    moveTypeValue += this.DualWield;
                    break;
                case WeaponType.magic:
                    moveTypeValue += this.Magic;
                    break;
            }
            float starPower = 0.7f + mCharacter.Star * 0.06f;
            this.PhysicalAttack = Mathf.FloorToInt((this.Power + this.Knowledge) * 0.3f + (this.Power * 2f + this.Knowledge) * (0.4f + (moveTypeValue * 0.5f) * 0.006f) * (1f + mCharacter.Level * starPower * 0.5f) * 0.1f);
            this.PhysicalAttack += physicalAttack;
            this.MagicAttack = Mathf.FloorToInt((this.Trick + this.Knowledge) * 0.3f + (this.Trick * 2f + this.Knowledge) * (0.4f + (moveTypeValue * 0.5f) * 0.006f) * (1f + mCharacter.Level * starPower * 0.5f) * 0.1f);
            this.MagicAttack += magicAttack;
            this.PhysicalDefense = Mathf.FloorToInt((this.Power * 0.5f + this.Knowledge) * 0.3f + (this.Power + this.Knowledge) * (1f + mCharacter.Level * starPower * 0.5f) * 0.04f);
            this.PhysicalDefense += physicalDefense;
            this.MagicDefense = Mathf.FloorToInt((this.Trick * 0.5f + this.Knowledge) * 0.3f + (this.Trick + this.Knowledge) * (1f + mCharacter.Level * starPower * 0.5f) * 0.04f);
            this.MagicDefense += magicDefense;
        }

        /// <summary>
        /// 物攻 = Lv + (力量*2+技巧)*(骑术|步战)/100
        /// </summary>
        public int PhysicalAttack
        {
            set
            {
                this.ViewModel.PhysicalAttack.Value = value;
            }
            get
            { 
                return this.ViewModel.PhysicalAttack.Value;
            }
        }

        /// <summary>
        /// 法攻 = Lv + (谋略*2+技巧)*(骑术|步战)/100
        /// </summary>
        public int MagicAttack
        {
            set
            {
                this.ViewModel.MagicAttack.Value = value;
            }
            get
            { 
                return this.ViewModel.MagicAttack.Value;
            }
        }

        /// <summary>
        /// 物防 = Lv + 力量+技巧
        /// </summary>
        public int PhysicalDefense
        {
            set
            {
                this.ViewModel.PhysicalDefense.Value = value;
            }
            get
            { 
                return this.ViewModel.PhysicalDefense.Value;
            }
        }

        /// <summary>
        /// 法防 = Lv + 谋略+技巧
        /// </summary>
        public int MagicDefense
        {
            set
            {
                this.ViewModel.MagicDefense.Value = value;
            }
            get
            { 
                return this.ViewModel.MagicDefense.Value;
            }
        }

        /// <summary>
        /// 力量 = 初始 + 技能
        /// </summary>
        public int Power
        {
            set
            {
                this.ViewModel.Power.Value = value;
            }
            get
            { 
                return this.ViewModel.Power.Value;
            }
        }

        /// <summary>
        /// 技巧 = 初始 + 技能
        /// </summary>
        public int Knowledge
        {
            set
            {
                this.ViewModel.Knowledge.Value = value;
            }
            get
            { 
                return this.ViewModel.Knowledge.Value;
            }
        }

        /// <summary>
        /// 速度 = 初始 + 技能
        /// </summary>
        public int Speed
        {
            set
            {
                this.ViewModel.Speed.Value = value;
            }
            get
            { 
                return this.ViewModel.Speed.Value;
            }
        }

        /// <summary>
        /// 谋略 = 初始 + 技能
        /// </summary>
        public int Trick
        {
            set
            {
                this.ViewModel.Trick.Value = value;
            }
            get
            { 
                return this.ViewModel.Trick.Value;
            }
        }

        /// <summary>
        /// 耐力 = 初始 + 技能
        /// </summary>
        public int Endurance
        {
            set
            {
                this.ViewModel.Endurance.Value = value;
            }
            get
            { 
                return this.ViewModel.Endurance.Value;
            }
        }

        /// <summary>
        /// 轻功 = 初始 + 技能
        /// </summary>
        public int MovingPower
        {
            set
            {
                this.ViewModel.MovingPower.Value = value;
            }
            get
            { 
                return this.ViewModel.MovingPower.Value;
            }
        }

        /// <summary>
        /// 骑术 = 初始 + 技能
        /// </summary>
        public int Riding
        {
            set
            {
                this.ViewModel.Riding.Value = value;
            }
            get
            { 
                return this.ViewModel.Riding.Value;
            }
        }

        /// <summary>
        /// 步战 = 初始 + 技能
        /// </summary>
        public int Walker
        {
            set
            {
                this.ViewModel.Walker.Value = value;
            }
            get
            { 
                return this.ViewModel.Walker.Value;
            }
        }

        /// <summary>
        /// 长枪 = 初始 + 技能
        /// </summary>
        public int Pike
        {
            set
            {
                this.ViewModel.Pike.Value = value;
            }
            get
            { 
                return this.ViewModel.Pike.Value;
            }
        }

        /// <summary>
        /// 短剑 = 初始 + 技能
        /// </summary>
        public int Sword
        {
            set
            {
                this.ViewModel.Sword.Value = value;
            }
            get
            { 
                return this.ViewModel.Sword.Value;
            }
        }

        /// <summary>
        /// 大刀 = 初始 + 技能
        /// </summary>
        public int LongKnife
        {
            set
            {
                this.ViewModel.LongKnife.Value = value;
            }
            get
            { 
                return this.ViewModel.LongKnife.Value;
            }
        }

        /// <summary>
        /// 短刀 = 初始 + 技能
        /// </summary>
        public int Knife
        {
            set
            {
                this.ViewModel.Knife.Value = value;
            }
            get
            { 
                return this.ViewModel.Knife.Value;
            }
        }

        /// <summary>
        /// 长斧 = 初始 + 技能
        /// </summary>
        public int LongAx
        {
            set
            {
                this.ViewModel.LongAx.Value = value;
            }
            get
            { 
                return this.ViewModel.LongAx.Value;
            }
        }

        /// <summary>
        /// 短斧 = 初始 + 技能
        /// </summary>
        public int Ax
        {
            set
            {
                this.ViewModel.Ax.Value = value;
            }
            get
            { 
                return this.ViewModel.Ax.Value;
            }
        }

        /// <summary>
        /// 长棍棒 = 初始 + 技能
        /// </summary>
        public int LongSticks
        {
            set
            {
                this.ViewModel.LongSticks.Value = value;
            }
            get
            { 
                return this.ViewModel.LongSticks.Value;
            }
        }

        /// <summary>
        /// 短棍棒 = 初始 + 技能
        /// </summary>
        public int Sticks
        {
            set
            {
                this.ViewModel.Sticks.Value = value;
            }
            get
            { 
                return this.ViewModel.Sticks.Value;
            }
        }

        /// <summary>
        /// 箭术 = 初始 + 技能
        /// </summary>
        public int Archery
        {
            set
            {
                this.ViewModel.Archery.Value = value;
            }
            get
            { 
                return this.ViewModel.Archery.Value;
            }
        }

        /// <summary>
        /// 暗器 = 初始 + 技能
        /// </summary>
        public int HiddenWeapons
        {
            set
            {
                this.ViewModel.HiddenWeapons.Value = value;
            }
            get
            { 
                return this.ViewModel.HiddenWeapons.Value;
            }
        }

        /// <summary>
        /// 双手 = 初始 + 技能
        /// </summary>
        public int DualWield
        {
            set
            {
                this.ViewModel.DualWield.Value = value;
            }
            get
            { 
                return this.ViewModel.DualWield.Value;
            }
        }

        /// <summary>
        /// 法宝 = 初始 + 技能
        /// </summary>
        public int Magic
        {
            set
            {
                this.ViewModel.Magic.Value = value;
            }
            get
            { 
                return this.ViewModel.Magic.Value;
            }
        }

        /// <summary>
        /// HpMax = 初始HP + 耐力*等级
        /// </summary>
        public int HpMax
        {
            set
            {
                this.ViewModel.HpMax.Value = value;
            }
            get
            { 
                return this.ViewModel.HpMax.Value;
            }
        }

        /// <summary>
        /// MpMax = 初始MP + 技巧*等级
        /// </summary>
        public int MpMax
        {
            set
            {
                this.ViewModel.MpMax.Value = value;
            }
            get
            { 
                return this.ViewModel.MpMax.Value;
            }
        }

        public int ResistanceMetal
        {
            set
            {
                this.ViewModel.ResistanceMetal.Value = value;
            }
            get
            { 
                return this.ViewModel.ResistanceMetal.Value;
            }
        }

        public int ResistanceWood
        {
            set
            {
                this.ViewModel.ResistanceWood.Value = value;
            }
            get
            { 
                return this.ViewModel.ResistanceWood.Value;
            }
        }

        public int ResistanceWater
        {
            set
            {
                this.ViewModel.ResistanceWater.Value = value;
            }
            get
            { 
                return this.ViewModel.ResistanceWater.Value;
            }
        }

        public int ResistanceFire
        {
            set
            {
                this.ViewModel.ResistanceFire.Value = value;
            }
            get
            { 
                return this.ViewModel.ResistanceFire.Value;
            }
        }

        public int ResistanceEarth
        {
            set
            {
                this.ViewModel.ResistanceEarth.Value = value;
            }
            get
            { 
                return this.ViewModel.ResistanceEarth.Value;
            }
        }
    }
}