using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.Model.Scriptable;
using App.View.Common;
using Holoville.HOTween;
using App.Controller.Common;


namespace App.Controller.Gacha{
    public class CGachaResultDialog : CDialog {
        [SerializeField]private Transform[] positions;
        [SerializeField]private GameObject contentsChild;
        [SerializeField]private Transform contentPanel;
        [SerializeField]private App.View.Character.VCard vCard;
        private Sequence seqCard = null;
        private bool isComplete = false;
        public override IEnumerator OnLoad( Request request ) 
        {  
            isComplete = false;
            StartCoroutine(base.OnLoad(request));

            App.Model.MContent[] contents = request.Get<App.Model.MContent[]>("contents");
            if (contents.Length == 1)
            {
                StartCoroutine(CoroutineShowContents(new App.Model.MContent[]{contents[0]}));
            }
            else
            {
                StartCoroutine(CoroutineShowContents(contents));
            }
            yield break;
        }
        private IEnumerator CoroutineShowContents(App.Model.MContent[] contents){
            for (int i = 0; i < contents.Length; i++)
            {
                App.Model.MContent content = contents[i];
                VContentsChild vContentsChild = CoroutineShowContent(content, contents.Length == 1 ? Vector3.zero : positions[i].localPosition);
                while (!vContentsChild.showComplete)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }
            isComplete = true;
        }
        public bool isPaused{
            get{ 
                return !seqCard.isEmpty && seqCard.isPaused;
            }
        }
        private VContentsChild CoroutineShowContent(App.Model.MContent content, Vector3 position){
            GameObject obj = Instantiate(contentsChild);
            obj.transform.SetParent(contentPanel);
            obj.transform.SetAsFirstSibling();
            VContentsChild vContentsChild = obj.GetComponent<VContentsChild>();vContentsChild.name = content.content_id.ToString();
            if (content.type == ContentType.character)
            {
                obj.transform.localPosition = position;
                obj.transform.localScale = new Vector3(0.001f, 0.001f, 1f);
                obj.transform.eulerAngles = Vector3.zero;
                MCharacter mCharacter = new MCharacter();
                mCharacter.CharacterId = content.content_id;
                vCard.gameObject.SetActive(true);
                vCard.transform.localPosition = Vector3.zero;
                vCard.transform.eulerAngles = Vector3.zero;
                vCard.transform.localScale = Vector3.zero;
                vCard.BindingContext = mCharacter.ViewModel;
                vCard.UpdateView();

                seqCard = new Sequence();
                seqCard.Insert (0f, HOTween.To (vCard.transform, 0.5f, new TweenParms()
                    .Prop("eulerAngles", new Vector3(0, 0, 360f))
                    .Prop("localScale", Vector3.one, false).OnComplete(()=>{
                    seqCard.Pause();
                })));
                seqCard.Insert (0.5f, HOTween.To (vCard.transform, 0.3f, new TweenParms()
                    .Prop("localPosition", position)
                    .Prop("eulerAngles", new Vector3(0.001f, 0.001f, 1f))
                    .Prop("localScale", new Vector3(120f/240f, 120f/360f, 1f), false).OnComplete(()=>{
                    vCard.gameObject.SetActive(false);
                    obj.transform.localScale = Vector3.one;
                    seqCard.Kill();
                    seqCard = null;
                    vContentsChild.showComplete = true;
                })));
                seqCard.Play ();
            }
            else
            {
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.zero;
                obj.transform.eulerAngles = Vector3.zero;
                HOTween.To(obj.transform, 0.6f, new TweenParms()
                    .Prop("eulerAngles", new Vector3(0, 0, 360f))
                    .Prop("localPosition", position)
                    .Prop("localScale", Vector3.one)
                    .OnComplete(()=>{
                        vContentsChild.showComplete = true;
                    }));
            }
            vContentsChild.showComplete = false;
            vContentsChild.UpdateView(content);
            return vContentsChild;
        }
        public void BackgroundClick(){
            if (seqCard != null && isPaused)
            {
                seqCard.Play ();
                return;
            }
            if (!isComplete)
            {
                return;
            }
            this.Close();
        }
        public void EquipmentIconClick(App.View.Equipment.VEquipmentIcon icon){
            BackgroundClick();
        }
        public void CharacterIconClick(App.View.Character.VCharacterIcon vCharacterIcon){
            BackgroundClick();
        }
	}
}