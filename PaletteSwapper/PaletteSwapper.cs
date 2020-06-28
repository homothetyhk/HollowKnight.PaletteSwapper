using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalEnums;
using Modding;
using UnityEngine;
using Random = System.Random;

namespace PaletteSwapper
{
    public class PaletteSwapper : Mod, ITogglableMod
    {
        Random rand;
        Dictionary<MapZone, Color> palette;
        Color defaultColor;

        public override void Initialize()
        {
            rand = new Random();
            palette = new Dictionary<MapZone, Color>();
            foreach (MapZone zone in Enum.GetValues(typeof(MapZone)))
            {
                palette.Add(zone, RandomColor());
            }
            defaultColor = RandomColor();

            On.SceneManager.SetLighting += OverrideSetLighting;
        }

        public void Unload()
        {
            On.SceneManager.SetLighting -= OverrideSetLighting;
        }

        public override string GetVersion() => "1.0";

        private void OverrideSetLighting(On.SceneManager.orig_SetLighting orig, Color ambientLightColor, float ambientLightIntensity)
        {
            if (GameManager.instance == null || GameManager.instance.sm == null || !palette.TryGetValue(GameManager.instance.sm.mapZone, out ambientLightColor))
            {
                ambientLightColor = defaultColor;
            }
            
            orig(ambientLightColor, ambientLightIntensity);
        }

        private Color RandomColor()
        {
            return new Color
            {
                r = (float)rand.NextDouble(),
                g = (float)rand.NextDouble(),
                b = (float)rand.NextDouble(),
                a = 1
            };
        }
    }
}
