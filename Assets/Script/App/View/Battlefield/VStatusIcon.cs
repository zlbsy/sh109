using UnityEngine.UI;
using App.Model;
using UnityEngine;
using App.ViewModel;
using App.Util;
using App.Util.Cacher;


namespace App.View.Battlefield{
    public class VStatusIcon : VBase {
        [SerializeField]private GameObject[] icons;
        public override void UpdateView(App.Model.MBase model)
        {
            App.Model.Master.MStrategy mStrategy = model as App.Model.Master.MStrategy;
            string iconName = string.Format("{0}_{1}", mStrategy.aid_type.ToString(), mStrategy.hert > 0 ? "up" : "down");
            //Debug.LogError("VStatusIcon iconName = " + iconName);
            foreach (GameObject icon in icons)
            {
                //Debug.LogError(icon.name+" == "+iconName + ", " + (icon.name == iconName));
                icon.SetActive(icon.name == iconName);
            }
        }
    }
}