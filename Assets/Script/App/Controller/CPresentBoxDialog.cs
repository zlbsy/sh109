using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.View.Item;
using App.Controller.Common;


namespace App.Controller{
    public class CPresentBoxDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            SPresent sPresent = new SPresent();
            yield return StartCoroutine(sPresent.RequestList());

            ScrollViewSets(content, childItem, sPresent.presents);
            yield return 0;
        }
        public void ClickReceive(App.View.Present.VPresentChild child){
            StartCoroutine(Receive(child));
        }
        public IEnumerator Receive(App.View.Present.VPresentChild child){
            SPresent sPresent = new SPresent();
            yield return StartCoroutine(sPresent.RequestReceive(child.ViewModel.Id.Value));
            if (sPresent.responseReceive.result)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        public override void Close(){
            base.Close();
        }
	}
}