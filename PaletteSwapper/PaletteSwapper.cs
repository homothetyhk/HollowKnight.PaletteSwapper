using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using GlobalEnums;
using Modding;
using UnityEngine;
using Random = System.Random;

namespace PaletteSwapper
{
    public partial class PaletteSwapper : Mod, ITogglableMod, IGlobalSettings<GlobalSettings>
    {
        public static PaletteSwapper instance;
        public static GlobalSettings Settings { get; private set; } = new GlobalSettings();

        private readonly static Random rand = new Random();
        private readonly static Dictionary<string, Color> palette = new Dictionary<string, Color>();
        Color defaultColor;

        public override void Initialize()
        {
            instance = this;
            palette.Clear();
            defaultColor = RandomColor();

            if (Settings.Disco)
            {
                Disco.Setup();
            }
            else if (Settings.RandomByMapZone)
            {
                On.SceneManager.SetLighting += OverrideSetLightingByZone;
            }
            else if (Settings.RandomByRoom)
            {
                On.SceneManager.SetLighting += OverrideSetLightingByScene;
            }
            else if (Settings.UsePaletteFromSettings)
            {
                foreach (var kvp in Settings.Palette)
                {
                    palette[kvp.Key] = kvp.Value;
                }
                On.SceneManager.SetLighting += OverrideSetLightingByZone;
            }
        }

        public static void Reload()
        {
            instance.Unload();
            instance.Initialize();
        }

        public void Unload()
        {
            On.SceneManager.SetLighting -= OverrideSetLightingByZone;
            On.SceneManager.SetLighting -= OverrideSetLightingByScene;
            Disco.Unload();
        }

        public override string GetVersion() => "1.1";

        private void OverrideSetLightingByZone(On.SceneManager.orig_SetLighting orig, Color ambientLightColor, float ambientLightIntensity)
        {
            if (GameManager.instance == null || GameManager.instance.sm == null || !Enum.IsDefined(typeof(MapZone), GameManager.instance.sm.mapZone))
            {
                ambientLightColor = defaultColor;
            }

            else if (!palette.TryGetValue(GameManager.instance.sm.mapZone.ToString(), out ambientLightColor))
            {
                palette[GameManager.instance.sm.mapZone.ToString()] = ambientLightColor = RandomColor();
            }
            
            orig(ambientLightColor, ambientLightIntensity);
        }

        private void OverrideSetLightingByScene(On.SceneManager.orig_SetLighting orig, Color ambientLightColor, float ambientLightIntensity)
        {
            if (GameManager.instance == null || !GameManager.instance.IsGameplayScene() || string.IsNullOrEmpty(GameManager.instance.sceneName))
            {
                ambientLightColor = defaultColor;
            }

            else if (!palette.TryGetValue(GameManager.instance.sceneName, out ambientLightColor))
            {
                palette[GameManager.instance.sceneName] = ambientLightColor = RandomColor();
            }

            orig(ambientLightColor, ambientLightIntensity);
        }

        public Color RandomColor()
        {
            if (Settings.LighterColors)
            {
                return new Color
                {
                    r = 0.5f + (float)rand.NextDouble() / 2,
                    g = 0.5f + (float)rand.NextDouble() / 2,
                    b = 0.5f + (float)rand.NextDouble() / 2,
                    a = 0.5f
                };
            }

            else if (Settings.DarkerColors)
            {
                return new Color
                {
                    r = (float)rand.NextDouble() / 2,
                    g = (float)rand.NextDouble() / 2,
                    b = (float)rand.NextDouble() / 2,
                    a = 1
                };
            }

            else
            {
                return new Color
                {
                    r = (float)rand.NextDouble(),
                    g = (float)rand.NextDouble(),
                    b = (float)rand.NextDouble(),
                    a = 0.5f + (float)rand.NextDouble() / 2
                };
            }
        }

        public void OnLoadGlobal(GlobalSettings s)
        {
            Settings = s;
        }

        public GlobalSettings OnSaveGlobal()
        {
            return Settings;
        }

        private void LiftSaveGlobalSettings()
        {
            base.SaveGlobalSettings();
        }

        new public static void SaveGlobalSettings()
        {
            instance.LiftSaveGlobalSettings();
        }
    }
}
