using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;

namespace App.Util.Search{
    /// <summary>
    /// 地图块儿搜索相关
    /// </summary>
    public class TileMap{
        private VBaseMap vBaseMap;
        private MBaseMap mBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        public TileMap(MBaseMap model, VBaseMap view){
            mBaseMap = model;
            vBaseMap = view;
            if (mBaseMap.MapId > 0)
            {
                baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
            }
        }
        public VTile GetTile(int index){
            Vector2 coordinate = baseMapMaster.GetCoordinateFromIndex(index);
            return GetTile(coordinate);
        }
        public VTile GetTile(Vector2 coordinate){
            return GetTile((int)coordinate.x, (int)coordinate.y);
        }
        public VTile GetTile(int x, int y){
            int i = y * vBaseMap.mapWidth + x;
            return vBaseMap.tileUnits[i];
        }
        public VTile GetTile(VTile tile, Direction direction){
            VTile resultTile = null;
            switch (direction)
            {
                case Direction.left:
                    resultTile = GetTile(tile.CoordinateX - 1, tile.CoordinateY);
                    break;
                case Direction.right:
                    resultTile = GetTile(tile.CoordinateX + 1, tile.CoordinateY);
                    break;
                case Direction.leftUp:
                    resultTile = GetTile(tile.CoordinateX - ((tile.CoordinateY + 1) % 2), tile.CoordinateY - 1);
                    break;
                case Direction.leftDown:
                    resultTile = GetTile(tile.CoordinateX - ((tile.CoordinateY + 1) % 2), tile.CoordinateY + 1);
                    break;
                case Direction.rightUp:
                    resultTile = GetTile(tile.CoordinateX + (tile.CoordinateY % 2), tile.CoordinateY - 1);
                    break;
                case Direction.rightDown:
                    resultTile = GetTile(tile.CoordinateX + (tile.CoordinateY % 2), tile.CoordinateY + 1);
                    break;
            }
            return resultTile;
        }
        public Direction GetDirection(VTile tile, VTile target){
            return GetDirection(tile.CoordinateX, tile.CoordinateY, target.CoordinateX, target.CoordinateY);
        }
        public Direction GetDirection(int x, int y, int cx, int cy){
            if (cy == y)
            {
                return cx > x ? Direction.right : Direction.left;
            }
            else if (cy > y)
            {
                return cx < x + (y % 2) ? Direction.leftDown : Direction.rightDown;
            }
            else
            {
                return cx < x + (y % 2) ? Direction.leftUp : Direction.rightUp;
            }
        }
        public int GetDistance(VTile tile1, VTile tile2){
            return GetDistance(tile1.CoordinateX, tile1.CoordinateY, tile2.CoordinateX, tile2.CoordinateY);
            /*if (tile2.CoordinateY == tile1.CoordinateY)
            {
                return Mathf.Abs(tile2.CoordinateX - tile1.CoordinateX);
            }
            int distance = 0;
            int directionY = tile2.CoordinateY > tile1.CoordinateY ? 1 : -1;
            int x = tile1.CoordinateX;
            int y = tile1.CoordinateY;
            do{
                distance += 1;
                if(tile2.CoordinateX != x){
                    if(y % 2 == 0){
                        if(tile2.CoordinateX < x){
                            x -= 1;
                        }
                    }else{
                        if(tile2.CoordinateX > x){
                            x += 1;
                        }
                    }
                }
                y += directionY;
            }while (tile2.CoordinateY != y);
            return Mathf.Abs(tile2.CoordinateX - x) + distance;*/
        }
        public int GetDistance(int x, int y, int cx, int cy){
            if (cy == y)
            {
                return Mathf.Abs(cx - x);
            }
            int distance = 0;
            int directionY = cy > y ? 1 : -1;
            do{
                distance += 1;
                if(cx != x){
                    if(y % 2 == 0){
                        if(cx < x){
                            x -= 1;
                        }
                    }else{
                        if(cx > x){
                            x += 1;
                        }
                    }
                }
                y += directionY;
            }while (cy != y);
            return Mathf.Abs(cx - x) + distance;
        }
        public List<Vector2> GetNeighboringCoordinates(Vector2 coordinate){
            return GetNeighboringCoordinates((int)coordinate.x, (int)coordinate.y);
        }
        public List<Vector2> GetNeighboringCoordinates(int x, int y){
            List<Vector2> coordinates = new List<Vector2>();
            if (y > 0)
            {
                if (y % 2 == 0)
                {
                    if (x > 0)
                    {
                        coordinates.Add(new Vector2(x - 1, y - 1));
                    }
                    coordinates.Add(new Vector2(x, y - 1));
                }
                else
                {
                    coordinates.Add(new Vector2(x, y - 1));
                    if (x + 1 < baseMapMaster.width)
                    {
                        coordinates.Add(new Vector2(x + 1, y - 1));
                    }
                }
            }
            if (x + 1 < baseMapMaster.width)
            {
                coordinates.Add(new Vector2(x + 1, y));
            }
            if (y + 1 < baseMapMaster.height)
            {
                if (y % 2 == 0)
                {
                    coordinates.Add(new Vector2(x, y + 1));
                    if (x > 0)
                    {
                        coordinates.Add(new Vector2(x - 1, y + 1));
                    }
                }
                else
                {
                    if (x + 1 < baseMapMaster.width)
                    {
                        coordinates.Add(new Vector2(x + 1, y + 1));
                    }
                    coordinates.Add(new Vector2(x, y + 1));
                }
            }
            if (x > 0)
            {
                coordinates.Add(new Vector2(x - 1, y));
            }
            return coordinates;
        }
    }
}