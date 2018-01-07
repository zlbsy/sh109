using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Effect{
    public class VEffectAnimation : VBase {
        [SerializeField]private SpriteRenderer image;
        [SerializeField]public Animator animator;
        private static List<GameObject> animations = new List<GameObject>();
        public static void AddEffectAnimation(GameObject obj){
            animations.Add(obj);
        }
        public static bool IsRunning{
            get{ 
                return animations.Count > 0;
            }
        }
        public void AnimationEnd(){
            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
            animations.RemoveAll(child=>child == null || !child.activeSelf);
            System.GC.Collect();
        }
    }
}