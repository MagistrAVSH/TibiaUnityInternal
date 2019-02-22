using System.Collections.Generic;
using System.IO;
using SkyImmerseEngine.Loaders;
using UnityEngine;
using System;
using Assets;
using Assets.Tibia.DAO;
using System.Text;

namespace SkyImmerseEngine
{
    public class AtlasSpriteManager
    {
        public static bool IsReady;
        public static float SpriteSize = 32;

        public static event Action LoadingComplete;

        private static readonly Dictionary<int, Material> _atlasesMaterials = new Dictionary<int, Material>();
        
        
        private static readonly Dictionary<int, Tuple<Material, TextureLocation>> _thingItems = new Dictionary<int, Tuple<Material, TextureLocation>>();

        private static readonly Dictionary<int, Tuple<Material, TextureLocation>> _thingCreatures = new Dictionary<int, Tuple<Material, TextureLocation>>();

        private static readonly Dictionary<int, Tuple<Material, TextureLocation>> _thingEffects = new Dictionary<int, Tuple<Material, TextureLocation>>();

        internal static void OpenFile(Stream fileStream, string shaderCreature, string shaderCommon)
        {
            ParseFile(fileStream, shaderCreature, shaderCommon);
        }

        private static readonly Dictionary<int, Tuple<Material, TextureLocation>> _thingMissiles = new Dictionary<int, Tuple<Material, TextureLocation>>();


        public static void ParseFile(Stream fileStream, string shaderCreature, string shaderCommon)
        {
            IsReady = false;

            using (var bw = new BinaryReader(fileStream, Encoding.UTF8, true))
            {
                Config.ResoltionASPR = bw.ReadInt32();
                SpriteSize = Config.ResoltionASPR * 32;

                // atlases
                var countAtlases = bw.ReadInt32();

                for (int v = 0; v < countAtlases; v++)
                {
                    var atlasCategory = bw.ReadInt32();

                    var atlasLength = bw.ReadInt32();

                    var atlas = bw.ReadBytes(atlasLength);
                    var atlasId = v;

                    if (atlasCategory == 99 && atlasLength == 1) continue;

                    var texture2D = new Texture2D(1, 1, TextureFormat.DXT5, false);
                    texture2D.LoadImage(atlas);
                    texture2D.wrapMode = TextureWrapMode.Clamp;
                    texture2D.filterMode = FilterMode.Point;
                    texture2D.Apply();

                    var shader = Shader.Find(atlasCategory == 1 /* creature category */ ? shaderCreature : shaderCommon);
                    var material = new Material(shader);
                    material.enableInstancing = true;

                    material.mainTexture = texture2D;

                    material.SetTexture("_Layer1Tex", material.mainTexture);
                    material.SetTexture("_MaskTex", material.mainTexture);

                    material.SetTexture("_Addon1Tex", material.mainTexture);
                    material.SetTexture("_Addon2Tex", material.mainTexture);

                    material.SetTexture("_AddonMask1Tex", material.mainTexture);
                    material.SetTexture("_AddonMask2Tex", material.mainTexture);


                    _atlasesMaterials.Add(atlasId, material);

                }

                // read thing type locations in atlases
                var countLocations = bw.ReadInt32();

                for (int j = 0; j < countLocations; j++)
                {
                    var category = bw.ReadInt32();

                    var id = bw.ReadInt32();
                    var atlasId = bw.ReadInt32();

                    var x = bw.ReadSingle();
                    var y = bw.ReadSingle();

                    var width = bw.ReadSingle();
                    var height = bw.ReadSingle();

                    var tl = new TextureLocation()
                    {
                        id = id,
                        atlasId = atlasId,
                        x = x,
                        y = y,
                        width = width,
                        height = height
                    };

                    if (category == 0) // items
                    {
                        _thingItems.Add(id, new Tuple<Material, TextureLocation>(_atlasesMaterials[tl.atlasId], tl));
                    }
                    if (category == 1) // creatures
                    {
                        _thingCreatures.Add(id, new Tuple<Material, TextureLocation>(_atlasesMaterials[tl.atlasId], tl));
                    }
                    if (category == 2) // effects
                    {
                        _thingEffects.Add(id, new Tuple<Material, TextureLocation>(_atlasesMaterials[tl.atlasId], tl));
                    }
                    if (category == 3) // missiles
                    {
                        _thingMissiles.Add(id, new Tuple<Material, TextureLocation>(_atlasesMaterials[tl.atlasId], tl));
                    }
                }
                fileStream.Flush();
                fileStream.Close();

                IsReady = true;

                LoadingComplete?.Invoke();
            }
        }


        
        /// <summary>
        /// Get thing type atlas material and thing location
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        public static Tuple<Material, TextureLocation> GetThingType(int id, int category)
        {
            if (category == 0) // items
            {
                return _thingItems[id];
            }
            if (category == 1) // creatures
            {
                return _thingCreatures[id];
            }
            if (category == 2) // effects
            {
                return _thingEffects[id];
            }
            if (category == 3) // missiles
            {
                return _thingMissiles[id];
            }

            return null;
        }
    }
    
    
}