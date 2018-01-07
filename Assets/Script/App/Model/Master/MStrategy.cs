using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
    public enum AidType{
        none,
        physicalAttack,
        magicAttack,
        physicalDefense,
        magicDefense,
        /// <summary>
        /// 混乱
        /// </summary>
        chaos,
        /// <summary>
        /// 睡眠
        /// </summary>
        sleep,
        /// <summary>
        /// 定身/麻痹
        /// </summary>
        hemp,
        /// <summary>
        /// 毒
        /// </summary>
        poison,
    }
    public enum StrategyEffectType{
        aid,
        status,
        animation,
        vampire
    }
    [System.Serializable]
    public class MStrategy : MBase {
        public MStrategy(){
        }
        public AidType aid_type;
        public StrategyEffectType effect_type;
        public float hert;
        public string effect;
	}
}