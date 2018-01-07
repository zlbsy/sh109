using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using App.Controller.Common;


namespace App.Controller.Battle{
    public class CBoutWaveDialog : CDialog {
        [SerializeField]private Text belongLabel;
        [SerializeField]private Text boutLabel;
        public override IEnumerator OnLoad( Request request ) 
        {  
            App.Model.Belong belong = request.Get<App.Model.Belong>("belong");
            int maxBout = request.Get<int>("maxBout");
            int bout = request.Get<int>("bout");
            belongLabel.text = string.Format(App.Util.Language.Get("bout_wave"), App.Util.Language.Get(belong.ToString()));
            boutLabel.text = string.Format("{0}/{1}", bout, maxBout);
            yield return StartCoroutine(base.OnLoad(request));
            StartCoroutine(WaitToClose());
		}
        private IEnumerator WaitToClose(){
            yield return new WaitForSeconds(1f);
            this.Close();
        }
	}
}