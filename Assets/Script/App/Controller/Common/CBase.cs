using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Controller.Common{
    public class CBase : MonoBehaviour {
		public virtual IEnumerator Start()
		{
			yield return StartCoroutine (OnLoad(null));
		}
        public virtual IEnumerator OnLoad( Request request ) 
		{  
			yield return 0;
		}
        public GameObject GetObject(GameObject obj){
            return Instantiate(obj) as GameObject;
        }
        public void ScrollViewSets(Transform parentContent, GameObject content, List<App.Model.MBase> models){
            App.Util.Global.ClearChild(parentContent.gameObject);
            foreach(App.Model.MBase model in models){
                ScrollViewSetChild(parentContent, content, model);
            }
        }
        public void ScrollViewSets(Transform parentContent, GameObject content, App.Model.MBase[] models){
            App.Util.Global.ClearChild(parentContent.gameObject);
            foreach(App.Model.MBase model in models){
                ScrollViewSetChild(parentContent, content, model);
            }
        }
        public GameObject ScrollViewSetChild(Transform parentContent, GameObject content, App.Model.MBase model){
            GameObject obj = Instantiate(content);
            obj.transform.SetParent(parentContent);
            obj.SetActive(true);
            obj.transform.localScale = Vector3.one;
            App.View.VBase view = obj.GetComponent<App.View.VBase>();
            if (model.VM == null)
            {
                view.UpdateView(model);
            }
            else
            {
                view.BindingContext = model.VM;
                view.UpdateView();
            }
            return obj;
        }
	}
}