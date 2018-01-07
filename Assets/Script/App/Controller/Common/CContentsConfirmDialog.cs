using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.View.Item;
using App.Controller.Common;


namespace App.Controller.Common{
	public class CContentsConfirmDialog : CDialog {
		[SerializeField]private Text title;
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            title.text = request.Get<string>("title");
            MContent[] contents = request.Get<MContent[]>("contents");
            ScrollViewSets(content, childItem, contents);
        }
	}
}