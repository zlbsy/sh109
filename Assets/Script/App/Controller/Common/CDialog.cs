using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using Holoville.HOTween;
using UnityEngine.UI;


namespace App.Controller.Common{
    /// <summary>
    /// 窗口弹出方式
    /// </summary>
    public enum OpenType{
        Middle,//从中间扩大
        Down,//从下面上升
        None,//无
        Fade,//逐渐显示
        Up,//从上面下降
    }
    /// <summary>
    /// 新窗口
    /// </summary>
    public class CDialog : CBase {
        [SerializeField]private OpenType opentype;
        [SerializeField]private bool noBackground;
        [SerializeField]private int staticSortingOrder = 0;
        Transform panel;
        protected UnityEngine.UI.Image background;
        [HideInInspector]public int index;
        private bool _isClose;
        private Vector2 _savePosition;
        private static int dialogIndex = 0;
        protected Canvas canvas;
        protected System.Action closeEvent;
        protected System.Action onLoadCompleteEvent;
        public override IEnumerator Start()
        {
            //覆盖CBase的Start，防止OnLoad自动发生
            yield break;
        }
        public static int GetIndex(){
            return dialogIndex++;
        }
        public virtual void OnEnable(){
            if (panel == null){
                panel = this.transform.Find("Panel");
                if (!noBackground)
                {
                    GameObject backgroundObj = App.Util.Global.SceneManager.LoadPrefab("DialogBackground");
                    backgroundObj.transform.SetParent(this.transform);
                    RectTransform rect = backgroundObj.GetComponent<RectTransform>();
                    rect.offsetMin = new Vector2(0f, 0f);
                    rect.offsetMax = new Vector2(0f, 0f);
                    rect.localScale = new Vector3(1.1f, 1.1f, 1f);
                    background = backgroundObj.GetComponent<UnityEngine.UI.Image>();
                }
            }
            if (background != null)
            {
                background.transform.SetAsFirstSibling();
                background.color = new Color(0, 0, 0, 0);
            }
            if (opentype == OpenType.Middle)
            {
                panel.localScale = new Vector3(panel.localScale.x, 0, panel.localScale.z);
            }else if (opentype == OpenType.Down)
            {
                RectTransform trans = panel as RectTransform;
                _savePosition = trans.anchoredPosition;
                trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, Camera.main.pixelHeight * -1f);
            }else if (opentype == OpenType.Up)
            {
                RectTransform trans = panel as RectTransform;
                _savePosition = trans.anchoredPosition;
                trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, Camera.main.pixelHeight * 1f);
            }else if (opentype == OpenType.Fade)
            {
                CanvasGroup canvasGroup = panel.gameObject.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = panel.gameObject.AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha = 0;
            }
            canvas = this.GetComponent<Canvas>();
            if (canvas == null)
            {
                return;
            }
            if (staticSortingOrder == 0)
            {
                canvas.sortingOrder = ++App.Util.Global.DialogSortOrder;
            }
            else
            {
                canvas.sortingOrder = staticSortingOrder;
            }
        }
        /// <summary>
        /// 设置新窗口唯一标示索引
        /// </summary>
        public void SetIndex(){
            this.index = CDialog.GetIndex();
        }
        public override IEnumerator OnLoad( Request request ) 
        {  
            if (request != null && request.Has("closeEvent"))
            {
                closeEvent = request.Get<System.Action>("closeEvent");
            }
            else
            {
                closeEvent = null;
            }
            if (request != null && request.Has("onLoadComplete"))
            {
                onLoadCompleteEvent = request.Get<System.Action>("onLoadComplete");
            }
            else
            {
                onLoadCompleteEvent = null;
            }
            if (background != null)
            {
                HOTween.To(background, 0.1f, new TweenParms().Prop("color", new Color(0,0,0,0.6f)));
            }
            if (opentype == OpenType.Middle)
            {
                HOTween.To(panel, 0.3f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).OnComplete(OnLoadAnimationOver));
            }else if (opentype == OpenType.Down || opentype == OpenType.Up)
            {
                HOTween.To(panel as RectTransform, 0.3f, new TweenParms().Prop("anchoredPosition", _savePosition).OnComplete(OnLoadAnimationOver));
            }else if (opentype == OpenType.Fade)
            {
                HOTween.To(panel.gameObject.GetComponent<CanvasGroup>(), 0.3f, new TweenParms().Prop("alpha", 1).OnComplete(OnLoadAnimationOver));
            }
            _isClose = false;
            yield return 0;
        }
        private void OnLoadAnimationOver(){
            if (onLoadCompleteEvent != null)
            {
                onLoadCompleteEvent();
            }
        }
        public virtual void Close(){
            if (_isClose)
            {
                return;
            }
            _isClose = true;
            if (canvas != null && staticSortingOrder == 0)
            {
                App.Util.Global.DialogSortOrder--;
            }
            if (background != null)
            {
                background.transform.SetAsLastSibling();
            }
            if (opentype == OpenType.Middle)
            {
                HOTween.To(panel, 0.2f, new TweenParms().Prop("localScale", new Vector3(1f, 0, 1f)).OnComplete(Delete));
            }
            else if (opentype == OpenType.Down)
            {
                RectTransform trans = panel as RectTransform;
                HOTween.To(panel as RectTransform, 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(trans.anchoredPosition.x, Camera.main.pixelHeight * -1f)).OnComplete(Delete));
            }
            else if (opentype == OpenType.Up)
            {
                RectTransform trans = panel as RectTransform;
                HOTween.To(panel as RectTransform, 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(trans.anchoredPosition.x, Camera.main.pixelHeight * 1f)).OnComplete(Delete));
            }
            else if (opentype == OpenType.Fade)
            {
                HOTween.To(panel.gameObject.GetComponent<CanvasGroup>(), 0.3f, new TweenParms().Prop("alpha", 0).OnComplete(Delete));
            }
            else if (opentype == OpenType.None)
            {
                Delete();
            }
        }
        public virtual void Delete(){
            if (background != null)
            {
                HOTween.To(background, 0.1f, new TweenParms().Prop("color", new Color(0, 0, 0, 0)).OnComplete(() =>
                        {
                            App.Util.Global.SceneManager.DestoryDialog(this);
                            if (closeEvent != null)
                            {
                                closeEvent();
                            }
                        }));
            }
            else
            {
                App.Util.Global.SceneManager.DestoryDialog(this);
                if (closeEvent != null)
                {
                    closeEvent();
                }
            }
        }
	}
}