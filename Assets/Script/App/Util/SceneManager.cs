using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Controller.Common;

namespace App.Util{
    public class SceneManager {
        public enum Scenes{
            Logo,
            Top,
            Stage,
            Area,
            World,
            Register,
            Battlefield
        }
        public enum Prefabs{
            BuildingDialog,
            LoadingDialog,
            ConnectingDialog,
            AlertDialog,
            ConfirmDialog,
            CharacterListDialog,
            ItemListDialog,
            LoginBonusDialog,
            LoginBonusGetDialog,
            PresentBoxDialog,
            MissionDialog,
            ShopDialog,
            CharacterDetailDialog,
            EquipmentListDialog,
            TalkDialog,
            MessageDialog,
            GachaDialog,
            GachaResultDialog,
            ReadyBattleDialog,
            BattleSkillListDialog,
            BattleListDialog,
            BattleMenuDialog,
            BoutWaveDialog,
            BattleFailDialog,
            BattleWinDialog,
            RegisterConfirmDialog,
            WebviewDialog,
            TutorialDialog,
            ContentsConfirmDialog,
            BlackScreen
        }
        public static CScene CurrentScene;
        public static Request CurrentSceneRequest;
        private List<CDialog> Dialogs = new List<CDialog>();
        public static void LoadScene(string name, Request req = null){
            App.Controller.CConnectingDialog.ToShow();
            CurrentScene.StartCoroutine(LoadSceneCoroutine(name, req));
        }
        public static IEnumerator LoadSceneCoroutine(string name, Request req = null){
            yield return new WaitForSeconds(0.1f);
            CurrentSceneRequest = req;
            UnityEngine.SceneManagement.SceneManager.LoadScene( name );
            Global.SceneManager.DestoryDialog();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        public IEnumerator ShowDialog(Prefabs prefab, Request req = null)
        {
            CDialog dialog = Dialogs.Find(_=>_.name == (prefab.ToString() + "(Clone)") && _ is CSingleDialog);
            if (dialog == null)
            {
                GameObject instance = LoadPrefab(prefab.ToString());
                instance.SetActive(true);
                dialog = instance.GetComponent<CDialog>();
                dialog.SetIndex();
                Dialogs.Add(dialog);
            }
            else
            {
                dialog.gameObject.SetActive(true);
            }
            yield return CurrentScene.StartCoroutine(dialog.OnLoad(req == null ? new Request() : req));
        }
        public GameObject LoadPrefab(string prefabName)
        {
            GameObject prefab = Resources.Load(string.Format("Prefabs/{0}", prefabName)) as GameObject;
            return CurrentScene.GetObject( prefab );
        }
        public bool DialogIsShow()
        {
            foreach(CDialog dialog in Dialogs){
                if(dialog.gameObject.activeSelf){
                    return true;
                }
            }
            return false;
        }
        public bool DialogIsShow(Prefabs prefab)
        {
            return FindDialog(prefab) != null;
            /*foreach(CDialog dialog in Dialogs){
                if(!dialog.gameObject.activeSelf){
                    continue;
                }
                if (dialog.name == (prefab.ToString() + "(Clone)"))
                {
                    return true;
                }
            }
            return false;*/
        }
        public CDialog FindDialog(Prefabs prefab)
        {
            foreach(CDialog dialog in Dialogs){
                if(!dialog.gameObject.activeSelf){
                    continue;
                }
                if (dialog.name == (prefab.ToString() + "(Clone)"))
                {
                    return dialog;
                }
            }
            return null;
        }
        public CDialog CurrentDialog
        {
            get{ 
                for (int i = Dialogs.Count - 1; i >= 0; i--)
                {
                    CDialog dialog = Dialogs[i];
                    if (dialog.gameObject.activeSelf)
                    {
                        return dialog;
                    }
                }
                return null;
            }
        }
        public void DestoryDialog(CDialog deleteDialog = null)
        {
            for (int i = Dialogs.Count - 1; i >= 0; i--)
            {
                CDialog dialog = Dialogs[i];
                if (deleteDialog == null)
                {
                    Dialogs.RemoveAt(i);
                }
                else if (deleteDialog.index == dialog.index)
                {
                    Dialogs.RemoveAt(i);
                    GameObject.Destroy(deleteDialog.gameObject);
                    break;
                }
            }
        }
	}
}