using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;
using App.Util.Cacher;
using App.Controller;

namespace App.Util.Search{
    /// <summary>
    /// 广度优先搜索
    /// </summary>
    public class BreadthFirst{
        private App.Controller.Battle.CBattlefield cBattlefield;
        private App.Controller.Common.CBaseMap cBaseMap;
        private MBaseMap mBaseMap;
        private VBaseMap vBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        private List<VTile> tiles;
        public BreadthFirst(App.Controller.Common.CBaseMap controller, MBaseMap model, VBaseMap view){
            cBaseMap = controller;
            mBaseMap = model;
            vBaseMap = view;
            if (cBaseMap is App.Controller.Battle.CBattlefield)
            {
                cBattlefield = cBaseMap as App.Controller.Battle.CBattlefield;
            }
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public List<VTile> Search(MCharacter mCharacter, int movePower = 0, bool obstacleEnable = false){
            SearchInit(mCharacter, obstacleEnable);
            tiles = new List<VTile>();
            if (movePower == 0)
            {
                movePower = mCharacter.Ability.MovingPower;
                if (movePower == 0)
                {
                    Debug.LogError("movePower = " + movePower);
                    movePower = 4;
                }
            }
            int i = mCharacter.CoordinateY * vBaseMap.mapWidth + mCharacter.CoordinateX;
            VTile tile = vBaseMap.tileUnits[i];
            tile.MovingPower = movePower;
            LoopSearch(tile);
            return tiles;
        }
        private void LoopSearch(VTile vTile){
            if(!vTile.IsRoad){
                return;
            }
            if (!vTile.IsChecked)
            {
                vTile.IsChecked = true;
                tiles.Add(vTile);
            }
            if (vTile.MovingPower <= 0 || vTile.IsAllCost)
            {
                return;
            }
            List<Vector2> coordinates = cBaseMap.mapSearch.GetNeighboringCoordinates(baseMapMaster.GetCoordinateFromIndex(vTile.Index));
            foreach (Vector2 vec in coordinates)
            {
                VTile tile = vBaseMap.tileUnits[(int)vec.y * vBaseMap.mapWidth + (int)vec.x];
                if (tile.IsChecked && tile.MovingPower >= vTile.MovingPower)
                {
                    continue;
                }
                int cost = 1;
                tile.MovingPower = vTile.MovingPower - cost;
                LoopSearch(tile);
            }
        }
        private void SearchInit(MCharacter mCharacter, bool obstacleEnable = false){
            foreach (VTile tile in vBaseMap.tileUnits)
            {
                tile.SearchInit();
            }
            if (cBattlefield == null || !obstacleEnable)
            {
                return;
            }
            foreach (MCharacter character in mBaseMap.Characters)
            {
                if (character.Hp == 0 || character.IsHide || cBattlefield.charactersManager.IsSameCharacter(mCharacter, character))
                {
                    continue;
                }
                VTile tile = cBattlefield.mapSearch.GetTile(character.CoordinateX, character.CoordinateY);
                if (cBattlefield.charactersManager.IsSameBelong(mCharacter.Belong, character.Belong))
                {
                    continue;
                }
                tile.IsRoad = false;

                List<Vector2> coordinates = cBattlefield.mapSearch.GetNeighboringCoordinates(baseMapMaster.GetCoordinateFromIndex(tile.Index));
                foreach (Vector2 vec in coordinates)
                {
                    VTile childTile = vBaseMap.tileUnits[(int)vec.y * vBaseMap.mapWidth + (int)vec.x];
                    if (childTile.CoordinateX == mCharacter.CoordinateX && childTile.CoordinateY == mCharacter.CoordinateY)
                    {
                        continue;
                    }
                    childTile.IsAllCost = true;
                }
            }
        }
    }
}