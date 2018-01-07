using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;

namespace App.View.Common{
    public class VTweenPosition : VBase {
        [SerializeField]private Vector3 from;
        [SerializeField]private Vector3 to;
        [SerializeField]private float duration = 0.5f;
        [SerializeField]private float delay = 0f;
        [SerializeField]private bool isLoop;
        [SerializeField]private LoopType loopType;
        private Sequence sequence;
        public void Start()
        {
            RectTransform rectTransform = this.transform as RectTransform;
            rectTransform.anchoredPosition3D = from;
            sequence = new Sequence ();
            sequence.Insert (delay, HOTween.To (rectTransform, duration, new TweenParms().Prop("anchoredPosition3D", to, false).Ease(EaseType.EaseInQuart)));
            if (isLoop)
            {
                sequence.loopType = loopType;
                sequence.loops = int.MaxValue;
            }
            sequence.Play ();
        }
        public override void OnDestroy(){
            sequence.Kill();
        }
	}
}