﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using App.Controller;
using System;
using App.Controller.Common;
using System.Linq;

namespace App.Util.LSharp{
    public class LSharpCharacter : LSharpBase<LSharpCharacter> {
        public void Add(string[] arguments){
            int npcId = int.Parse(arguments[0]);
            string action = arguments[1];
            if (action == "stand")
            {
                action = "idle";
            }
            string directionStr = arguments[2];
            int x = int.Parse(arguments[3]);
            int y = int.Parse(arguments[4]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            App.Model.ActionType actionType = (App.Model.ActionType)Enum.Parse(typeof(App.Model.ActionType), action);
            App.Model.Direction direction = (App.Model.Direction)System.Enum.Parse(typeof(App.Model.Direction), directionStr, true);
            cBaseMap.AddCharacter(npcId, actionType, direction, x, y);
            LSharpScript.Instance.Analysis();
        }
        public void Hide(string[] arguments){
            int npcId = int.Parse(arguments[0]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.HideNpc(npcId, true);
            LSharpScript.Instance.Analysis();
        }
        public void Show(string[] arguments){
            int npcId = int.Parse(arguments[0]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.HideNpc(npcId, false);
            LSharpScript.Instance.Analysis();
        }
        public void Move(string[] arguments){
            int npcId = int.Parse(arguments[0]);
            int x = int.Parse(arguments[1]);
            int y = int.Parse(arguments[2]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.MoveNpc(npcId, x, y);
        }
        public void Moveself(string[] arguments){
            int index = int.Parse(arguments[0]);
            int x = int.Parse(arguments[1]);
            int y = int.Parse(arguments[2]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.MoveSelf(index, x, y);
        }
        /*public void Moveplayer(string[] arguments){
            int x = int.Parse(arguments[0]);
            int y = int.Parse(arguments[1]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.MovePlayer(x, y);
        }*/
        public void Setmission(string[] arguments){
            int npcId = int.Parse(arguments[0]);
            App.Model.Mission mission = (App.Model.Mission)Enum.Parse(typeof(App.Model.Mission), arguments[1]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.SetNpcMission(npcId, mission);
            LSharpScript.Instance.Analysis();
        }
        public void Setdirection(string[] arguments){
            int npcId = int.Parse(arguments[0]);
            App.Model.Direction direction = (App.Model.Direction)System.Enum.Parse(typeof(App.Model.Direction), arguments[1], true);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.SetNpcDirection(npcId, direction);
            LSharpScript.Instance.Analysis();
        }
        public void Setselfdirection(string[] arguments){
            int index = int.Parse(arguments[0]);
            App.Model.Direction direction = (App.Model.Direction)System.Enum.Parse(typeof(App.Model.Direction), arguments[1], true);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.SetSelfDirection(index, direction);
            LSharpScript.Instance.Analysis();
        }
        public void Setaction(string[] arguments){
            int npcId = int.Parse(arguments[0]);
            App.Model.ActionType actionType = (App.Model.ActionType)Enum.Parse(typeof(App.Model.ActionType), arguments[1]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.SetNpcAction(npcId, actionType);
        }
        public void Setselfaction(string[] arguments){
            int index = int.Parse(arguments[0]);
            App.Model.ActionType actionType = (App.Model.ActionType)Enum.Parse(typeof(App.Model.ActionType), arguments[1]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.SetSelfAction(index, actionType);
        }
        public void Addselfskill(string[] arguments){
        }
        public void Addskill(string[] arguments){
            int characterId = int.Parse(arguments[0]);
            App.Model.Belong belong = (App.Model.Belong)Enum.Parse(typeof(App.Model.Belong), arguments[1]);
            int skillId = int.Parse(arguments[2]);
            int skillLevel = int.Parse(arguments[3]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.AddCharacterSkill(characterId, belong, skillId, skillLevel);
            LSharpScript.Instance.Analysis();
        }
        public void Removeskill(string[] arguments){
            int characterId = int.Parse(arguments[0]);
            App.Model.Belong belong = (App.Model.Belong)Enum.Parse(typeof(App.Model.Belong), arguments[1]);
            int skillId = int.Parse(arguments[2]);
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap == null)
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            cBaseMap.RemoveCharacterSkill(characterId, belong, skillId);
            LSharpScript.Instance.Analysis();
        }
	}
}