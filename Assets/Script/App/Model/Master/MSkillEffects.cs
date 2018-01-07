using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
    public enum SkillEffectSpecial{
        none,
        /// <summary>
        /// 引导攻击
        /// </summary>
        continue_attack,
        /// <summary>
        /// 反击后反击
        /// </summary>
        attack_back_attack,
        /// <summary>
        /// 先手攻击
        /// </summary>
        force_first,
        /// <summary>
        /// 吸血
        /// </summary>
        vampire,
        /// <summary>
        /// 能力,状态变化
        /// </summary>
        status,
        /// <summary>
        /// 攻击范围扩大
        /// </summary>
        attack_distance,
        /// <summary>
        /// 反击不受任何限制
        /// </summary>
        force_back_attack,
        /// <summary>
        /// 无反攻击
        /// </summary>
        no_back_attack,
        /// <summary>
        /// 必中
        /// </summary>
        force_hit,
        /// <summary>
        /// 溅射
        /// </summary>
        quantity_plus,
        /// <summary>
        /// 对骑兵攻击加成
        /// </summary>
        horse_hert,
        /// <summary>
        /// 攻击后移动
        /// </summary>
        move_after_attack,
        /// <summary>
        /// 移动攻击
        /// </summary>
        move_and_attack,
        /// <summary>
        /// 每回合固定伤害
        /// </summary>
        bout_fixed_damage,
        /// <summary>
        /// 攻击次数
        /// </summary>
        attack_count,
        /// <summary>
        /// 反击次数
        /// </summary>
        counter_attack_count,
        /// <summary>
        /// 对攻击范围内所有人进行攻击
        /// </summary>
        attack_all_near,
        /// <summary>
        /// 回马枪
        /// </summary>
        back_thrust,
        /// <summary>
        /// 固定伤害攻击
        /// </summary>
        fixed_damage,
        /// <summary>
        /// 地形辅助
        /// </summary>
        tile,
    }
    [System.Serializable]
	public class MSkillEffects : MBase {
        public MSkillEffects(){
        }
        public SkillEffectSpecial special;
        public int special_value;
        public MSkillEffect enemy;
        public MSkillEffect self;
	}
}