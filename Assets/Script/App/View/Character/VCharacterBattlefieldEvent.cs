using App.Model;
using Holoville.HOTween;
using UnityEngine;

namespace App.View.Character{
    public partial class VCharacter{
        private static Vector3 numScale = new Vector3(0.01f, 0.01f, 0.01f);
        public void OnDamage(App.Model.Battle.MDamageParam arg){
            this.ChangeAction(ActionType.hert);
            this.num.text = arg.value.ToString();
            this.num.gameObject.SetActive(true);
            this.num.transform.localPosition = new Vector3(0,0.2f,0);
            this.num.color = Color.white;
            this.num.transform.localScale = Vector3.zero;
            Sequence seqHp = new Sequence ();
            seqHp.Insert (0f, HOTween.To (this.num.transform, 0.2f, new TweenParms().Prop("localScale", numScale * 2f, false).Ease(EaseType.EaseInQuart)));
            seqHp.Insert (0.2f, HOTween.To (this.num.transform, 0.3f, new TweenParms().Prop("localScale", numScale, false).Ease(EaseType.EaseOutBounce)));
            seqHp.Insert (0.5f, HOTween.To (this.num, 0.2f, new TweenParms().Prop("color", new Color(this.num.color.r, this.num.color.g, this.num.color.b, 0f), false).OnComplete(()=>{
                this.num.gameObject.SetActive(false);
            })));
            seqHp.Insert (0f, HOTween.To (this.ViewModel.Hp, 0.2f, new TweenParms().Prop("Value", this.ViewModel.Hp.Value + arg.value, false).Ease(EaseType.EaseInQuart)));
            seqHp.Play ();
        }
        public void OnBlock(){
            this.ChangeAction(ActionType.block);
        }
        public void OnHeal(App.Model.Battle.MDamageParam arg){
            this.ChangeAction(ActionType.block);
            OnHealWithoutAction(arg);
            /*this.num.text = string.Format("+{0}", arg.value);
            this.num.gameObject.SetActive(true);
            this.num.transform.localPosition = new Vector3(0,0.2f,0);
            this.num.color = Color.green;
            this.num.transform.localScale = Vector3.zero;
            Sequence seqHp = new Sequence ();
            seqHp.Insert (0f, HOTween.To (this.num.transform, 0.2f, new TweenParms().Prop("localScale", numScale * 2f, false).Ease(EaseType.EaseInQuart)));
            seqHp.Insert (0.2f, HOTween.To (this.num.transform, 0.3f, new TweenParms().Prop("localScale", numScale, false).Ease(EaseType.EaseOutBounce)));
            seqHp.Insert (0.5f, HOTween.To (this.num, 0.2f, new TweenParms().Prop("color", new Color(this.num.color.r, this.num.color.g, this.num.color.b, 0f), false).OnComplete(()=>{
                this.num.gameObject.SetActive(false);
            })));
            seqHp.Insert (0f, HOTween.To (this.ViewModel.Hp, 0.2f, new TweenParms().Prop("Value", this.ViewModel.Hp.Value + arg.value, false).Ease(EaseType.EaseInQuart)));
            seqHp.Play ();*/
        }
        public void OnHealWithoutAction(App.Model.Battle.MDamageParam arg){
            this.num.text = string.Format("+{0}", arg.value);
            this.num.gameObject.SetActive(true);
            this.num.transform.localPosition = new Vector3(0,0.2f,0);
            this.num.color = Color.green;
            this.num.transform.localScale = Vector3.zero;
            Sequence seqHp = new Sequence ();
            seqHp.Insert (0f, HOTween.To (this.num.transform, 0.2f, new TweenParms().Prop("localScale", numScale * 2f, false).Ease(EaseType.EaseInQuart)));
            seqHp.Insert (0.2f, HOTween.To (this.num.transform, 0.3f, new TweenParms().Prop("localScale", numScale, false).Ease(EaseType.EaseOutBounce)));
            seqHp.Insert (0.5f, HOTween.To (this.num, 0.2f, new TweenParms().Prop("color", new Color(this.num.color.r, this.num.color.g, this.num.color.b, 0f), false).OnComplete(()=>{
                this.num.gameObject.SetActive(false);
            })));
            seqHp.Insert (0f, HOTween.To (this.ViewModel.Hp, 0.2f, new TweenParms().Prop("Value", this.ViewModel.Hp.Value + arg.value, false).Ease(EaseType.EaseInQuart)));
            seqHp.Play ();
        }
    }
}