using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Character{
    public class VFace : VBase {
        private Image icon;
        public int CharacterId
        {
            set{
                this.StartCoroutine(LoadFaceIcon(value));
            }
        }
        public IEnumerator LoadFaceIcon(int characterId)
        {
            if (icon == null)
            {
                icon = this.GetComponent<Image>();
            }
            icon.color = new Color32(255, 255, 255, 1);
            while (FaceCacher.Instance.IsLoadingId(characterId))
            {
                yield return new WaitForEndOfFrame();
            }
            App.Model.Scriptable.MFace mFace = FaceCacher.Instance.Get(characterId);
            if (mFace != null)
            {
                icon.sprite = Sprite.Create(mFace.image, new Rect (0, 0, mFace.image.width, mFace.image.height), Vector2.zero);
                icon.color = new Color32(255, 255, 255, 255);
                yield break;
            }
            string url = string.Format(App.Model.Scriptable.FaceAsset.FaceUrl, characterId);
            FaceCacher.Instance.LoadingId(characterId);
            yield return this.StartCoroutine(Global.SUser.Download(url, App.Util.Global.versions.face, (AssetBundle assetbundle)=>{
                App.Model.Scriptable.FaceAsset.assetbundle = assetbundle;
                mFace = App.Model.Scriptable.FaceAsset.Data.face;
                icon.sprite = Sprite.Create(mFace.image, new Rect (0, 0, mFace.image.width, mFace.image.height), Vector2.zero);
                mFace.id = characterId;
                FaceCacher.Instance.Set(mFace);
                icon.color = new Color32(255, 255, 255, 255);
            }));
        }

    }
}