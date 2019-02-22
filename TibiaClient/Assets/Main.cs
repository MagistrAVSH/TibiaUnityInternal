using Assets.Tibia.ClassicNetwork;
using Assets.Tibia.DAO;
using Assets.Tibia.UI.Login_Interface;
using Game.DAO;
using Game.Graphics;
using GameClient;
using GameClient.Network;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using SkyImmerseEngine;
using SkyImmerseEngine.Graphics;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Assets
{
    public class Main : MonoBehaviour
    {
        public CanvasGroup CompatibilityScreen;

        public void ContinueAfterTest()
        {
            CompatibilityScreen.alpha = 0;
            CompatibilityScreen.interactable = false;
            CompatibilityScreen.blocksRaycasts = false;

            ResetRequiredSettings.Reset();

            Config.Load();
            FeatureManager.Init();

            MapRenderer.InitCameras();
            ThingTypeRender.Init();
            ThingTypeManager.Init();
            ThingTypeManager.LoadingComplete += ThingTypeManager_LoadingComplete;
            AtlasSpriteManager.LoadingComplete += AtlasSpriteManager_LoadingComplete;

            LoadingCircle.Global.Visible = true;


            ThingTypeManager.OpenThingsFile(File.OpenRead(Path.Combine(Application.streamingAssetsPath, "Things", Config.ClientVersion.ToString(), "Tibia.dat")));


            using (System.IO.FileStream fs = new FileStream(Path.Combine(Application.streamingAssetsPath, "Things", Config.ClientVersion.ToString(), "Tibia.aspr"), FileMode.Open, FileAccess.Read))
            using (GZipStream decompressionStream = new GZipStream(fs, CompressionMode.Decompress, true))
            {
                AtlasSpriteManager.OpenFile(decompressionStream, "Custom/Creature", "Custom/ItemUnlit");
            }
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
        }

        public void Exit()
        {
            Application.Quit();
        }
        public void Start()
        {

            if(!SystemInfoTest.Test())
            {
                CompatibilityScreen.alpha = 1;
                CompatibilityScreen.interactable = true;
                CompatibilityScreen.blocksRaycasts = true;
                return;
            }

            ContinueAfterTest();

        }

        private static void AtlasSpriteManager_LoadingComplete()
        {
            GC.Collect();
            LoadingCircle.Global.Visible = false;
            Object.FindObjectOfType<UILoginController>().Visible = true;
            GameObject.Find("StartBackground")?.SetActive(false);
            UIInterfaceVisibility.ShowAllLoginInterface();
        }

        private static void ThingTypeManager_LoadingComplete()
        {
            GC.Collect();
            Debug.Log("Things loaded");
        }
    }
}
