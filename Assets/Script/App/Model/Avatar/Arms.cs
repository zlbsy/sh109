using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Avatar
{
    [System.Serializable]
    public class Arms
    {
        public Arms()
        {
        }

        public AvatarAction[] attack;
        public AvatarAction[] move;
        public AvatarAction[] stand;
        public AvatarAction[] block;
        public AvatarAction[] hert;

        public AvatarAction GetAvatarAction(ActionType actionType, int index)
        {
            //Debug.Log("actionType="+actionType+", index="+index);
            AvatarAction[] actions = null;
            switch (actionType)
            {
                case ActionType.move:
                    actions = move;
                    break;
                case ActionType.attack:
                    actions = attack;
                    break;
                case ActionType.block:
                    actions = block;
                    break;
                case ActionType.hert:
                    actions = hert;
                    break;
                case ActionType.idle:
                default:
                    actions = stand;
                    break;
            }
            if (index >= actions.Length)
            {
                index = 0;
            }
            return actions[index];
        }
    }
}