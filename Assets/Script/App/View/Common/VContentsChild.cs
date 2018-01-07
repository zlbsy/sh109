using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.View.Item;
using App.View.Equipment;
using App.Model;
using App.View.Character;

namespace App.View.Common{
    public class VContentsChild : VBase {
        [SerializeField]private VItemIcon vItemIcon;
        [SerializeField]private VEquipmentIcon vEquipmentIcon;
        [SerializeField]private VCharacterIcon vCharacterIcon;
        [SerializeField]private GameObject contentChild;
        public bool showComplete{ get; set;}
        public string ContentName{ get; set;}
        public override void UpdateView(MBase model){
            MContent mContent = model as MContent;
            vItemIcon.gameObject.SetActive(false);
            vEquipmentIcon.gameObject.SetActive(false);
            vCharacterIcon.gameObject.SetActive(false);
            contentChild.SetActive(false);
            switch (mContent.type)
            {
                case ContentType.item:
                    SetItem(mContent);
                    break;
                case ContentType.horse:
                case ContentType.weapon:
                case ContentType.clothes:
                    SetEquipment(mContent);
                    break;
                case ContentType.character:
                    SetCharacter(mContent);
                    break;
                default:
                    SetContent(mContent);
                    break;
            }
        }
        private void SetCharacter(MContent mContent){
            MCharacter character = new MCharacter();
            character.CharacterId = mContent.content_id;
            character.Level = 0;
            vCharacterIcon.gameObject.SetActive(true);
            vCharacterIcon.BindingContext = character.ViewModel;
            vCharacterIcon.UpdateView();
            ContentName = character.Master.name;
        }
        private void SetEquipment(MContent mContent){
            MEquipment equipment = new MEquipment();
            equipment.EquipmentId = mContent.content_id;
            equipment.EquipmentType = (App.Model.Master.MEquipment.EquipmentType)System.Enum.Parse(typeof(App.Model.Master.MEquipment.EquipmentType), mContent.type.ToString(), true);
            vEquipmentIcon.gameObject.SetActive(true);
            vEquipmentIcon.BindingContext = equipment.ViewModel;
            vEquipmentIcon.UpdateView();
            ContentName = equipment.Master.name;
        }
        private void SetItem(MContent mContent){
            MItem item = new MItem();
            item.ItemId = mContent.content_id;
            item.Cnt = 1;
            vItemIcon.gameObject.SetActive(true);
            vItemIcon.BindingContext = item.ViewModel;
            vItemIcon.UpdateView();
            ContentName = item.Master.name;
        }
        private void SetContent(MContent mContent){
            contentChild.SetActive(true);
            Image[] icons = contentChild.transform.Find("Icon").GetComponentsInChildren<Image>(true);
            foreach (Image icon in icons)
            {
                icon.gameObject.SetActive(icon.name == mContent.type.ToString());
            }
            Text num = contentChild.GetComponentInChildren<Text>();
            num.text = mContent.value.ToString();
            ContentName = Language.Get(mContent.type.ToString());
        }
    }
}