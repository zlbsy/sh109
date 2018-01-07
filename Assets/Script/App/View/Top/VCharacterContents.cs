
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;
using App.Util;
using App.Controller;
using App.View.Common;

namespace App.View.Top{
    public class VCharacterContents : VBase{
        [SerializeField]protected Transform rotateLayer;
        [SerializeField]private App.View.Character.VFaceSpriteRenderer[] characters;
        protected float angles;
        protected Vector2 mousePosition = Vector2.zero;
        protected Vector2 dragPosition = Vector2.zero;
        protected bool _camera3DEnable = true;
        public void UpdateView(App.Model.MCharacter[] mCharacters){
            List<App.Model.MCharacter> characterList = new List<App.Model.MCharacter>();
            while(characterList.Count < 6){
                foreach(App.Model.MCharacter mCharacter in mCharacters){
                    if (mCharacter.IsUserCharacter)
                    {
                        continue;
                    }
                    characterList.Add(mCharacter);
                }
                if (characterList.Count == 0)
                {
                    foreach(App.View.Character.VFaceSpriteRenderer character in characters){
                        character.gameObject.SetActive(false);
                    }
                    return;
                }
            }
            characterList.Sort((a, b)=>b.Master.qualification - a.Master.qualification);
            for(int i=0;i < characterList.Count && i < 6;i++){
                App.Model.MCharacter mCharacter = characterList[i];
                App.View.Character.VFaceSpriteRenderer character = characters[i];
                character.CharacterId = mCharacter.CharacterId;
            }
        }

        public bool Camera3DEnable{
            set{ 
                _camera3DEnable = value;
            }
            get{ 
                return _camera3DEnable;
            }
        }
        void OnMouseDown(){
            if (Global.SceneManager.DialogIsShow() || !Camera3DEnable)
            {
                mousePosition.x = int.MinValue;
                return;
            }
            mousePosition.x = Input.mousePosition.x;
            angles = rotateLayer.localEulerAngles.y;
        }
        void OnMouseUp(){
            if (Global.SceneManager.DialogIsShow() || !Camera3DEnable)
            {
                return;
            }
            int angle = (int)rotateLayer.localEulerAngles.y;
            int minAngle = Mathf.FloorToInt(angle / 60) * 60;
            HOTween.To(rotateLayer, 0.3f, new TweenParms().Prop("localEulerAngles", 
                new Vector3(0, minAngle + (angle % 60 > 30 ? 60 : 0), 0)));
            /*bool _isDraging = Mathf.Abs(Input.mousePosition.x - mousePosition.x) > 4f || Mathf.Abs(Input.mousePosition.y - mousePosition.y) > 4f;
            if (!_isDraging)
            {
                //rotateLayer.localEulerAngles = new Vector3(0, minAngle + (angle % 60 > 30 ? 60 : 0), 0);
                return;
            }*/
            mousePosition.x = int.MinValue;
        }
        void OnMouseDrag(){
            if (mousePosition.x == int.MinValue)
            {
                return;
            }
            rotateLayer.localEulerAngles = new Vector3(0, angles + (mousePosition.x - Input.mousePosition.x) * 0.1f, 0);
        }
	}
}