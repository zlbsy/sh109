using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using UnityEngine.UI;
using App.Model.Scriptable;
using Holoville.HOTween;
using App.Controller.Common;


namespace App.Controller{
    public class CLoadingDialog : CDialog {
        [SerializeField]private Image barBackground;
        [SerializeField]private Image barPrevious;
        [SerializeField]private Text progress;
        [SerializeField]private Text message;
        [SerializeField]private Image review;
        private float width;
        private float height;
        private float _nextProgress;
        private float _nowProgress;
        private float _plusProgress;
        private MPromptMessage[] promptMessages;
        private int promptMessageIndex = 0;
        public override void OnEnable(){
            message.gameObject.SetActive(false);
            review.gameObject.SetActive(false);
            width = 4 - barBackground.GetComponent<RectTransform>().rect.width;
            height = barPrevious.GetComponent<RectTransform>().offsetMax.y;
            base.OnEnable();
        }
        public override IEnumerator OnLoad( Request request ) 
        {  
            Progress = 0f;
            yield return StartCoroutine(base.OnLoad(request));
            StartCoroutine(SetDefaultPromptMessage());
        }
        public float NextProgress{
            set{ 
                _nextProgress = value;
                _nowProgress = Progress;
            }
        }
        public float PlusProgress{
            set{ 
                Progress = _nowProgress + value * (_nextProgress - _nowProgress);
            }
        }
        public float Progress{
            get{ 
                return float.Parse(progress.text.Substring(0, progress.text.Length - 1));
            }
            set{ 
                progress.text = string.Format("{0}%", (Mathf.Floor(value * 100f) * 0.01f).ToString("F"));
                barPrevious.GetComponent<RectTransform>().offsetMax = new Vector2(width * (100 - value) * 0.01f, height);
            }
        }
        public IEnumerator SetDefaultPromptMessage(){
            if (PromptMessageAsset.Data == null)
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(SetDefaultPromptMessage());
                yield break;
            }
            if (promptMessages == null)
            {
                promptMessages = PromptMessageAsset.Data.promptMessages;
                System.Array.Sort(promptMessages, (a, b)=>{return Random.Range(0f, 1f) > 0.5 ? 1 : -1;});
            }
            if (promptMessageIndex >= promptMessages.Length)
            {
                promptMessageIndex = 0;
            }
            MPromptMessage promptMessage = promptMessages[promptMessageIndex++];
            HOTween.To(review, 0.2f, new TweenParms().Prop("color", new Color(0f, 0f, 0f, 0f)));
            yield return new WaitForSeconds(0.2f);
            message.text = promptMessage.message;
            review.sprite = Sprite.Create(promptMessage.image, new Rect (0, 0, promptMessage.image.width, promptMessage.image.height), Vector2.zero);
            HOTween.To(review, 0.2f, new TweenParms().Prop("color", new Color(1f, 1f, 1f, 1f)));
            yield return new WaitForSeconds(0.2f);
            message.gameObject.SetActive(true);
            review.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            StartCoroutine(SetDefaultPromptMessage());
        }
        public static void ToShow(){
            SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.LoadingDialog));
        }
        public static void SetNextProgress(float value){
            if (Global.SceneManager.CurrentDialog is CLoadingDialog)
            {
                (Global.SceneManager.CurrentDialog as CLoadingDialog).NextProgress = value;
            }
        }
        public static void UpdatePlusProgress(float value){
            if (Global.SceneManager.CurrentDialog is CLoadingDialog)
            {
                (Global.SceneManager.CurrentDialog as CLoadingDialog).PlusProgress = value;
            }
        }
        public static void ToClose(){
            if (Global.SceneManager.CurrentDialog is CLoadingDialog)
            {
                Global.SceneManager.CurrentDialog.Close();
            }
        }
	}
}