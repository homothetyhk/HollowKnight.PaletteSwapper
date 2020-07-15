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
    public class PaletteSwapper : Mod, ITogglableMod
    {
        public static PaletteSwapper instance;

        public GlobalSettings Settings = new GlobalSettings();

        public override ModSettings GlobalSettings 
        { 
            get => Settings; 
            set => Settings = value as GlobalSettings; 
        }

        Random rand;
        Dictionary<string, Color> palette;
        Color defaultColor;

        public override void Initialize()
        {
            instance = this;
            Settings.Setup();

            rand = new Random();
            palette = new Dictionary<string, Color>();
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
                bool CheckIfWellFormatted(string key, string part0, string part1)
                {
                    string[] parts = key.Split('.');
                    return parts[0] == part0 && parts[1] == part1;
                }

                foreach (var kvp in Settings.Palette)
                {
                    string zone = kvp.Key.Split('.')[0];
                    if (!palette.ContainsKey(zone))
                    {
                        try
                        {
                            palette[zone] = new Color
                            {
                                r = Settings.Palette.FirstOrDefault(_kvp => CheckIfWellFormatted(_kvp.Key, zone, "r")).Value,
                                g = Settings.Palette.FirstOrDefault(_kvp => CheckIfWellFormatted(_kvp.Key, zone, "g")).Value,
                                b = Settings.Palette.FirstOrDefault(_kvp => CheckIfWellFormatted(_kvp.Key, zone, "b")).Value,
                                a = Settings.Palette.FirstOrDefault(_kvp => CheckIfWellFormatted(_kvp.Key, zone, "a")).Value,
                            };
                        }
                        catch (Exception e)
                        {
                            LogError($"Error loading color data from settings palette. " +
                                $"Check that keys are formatted as \"MapZone.r\", \"MapZone.g\", \"MapZone.b\", and values are floating point numbers between 0 and 1.\n {e}");
                        }
                    }
                }
                On.SceneManager.SetLighting += OverrideSetLightingByZone;
            }
        }

        public void Unload()
        {
            On.SceneManager.SetLighting -= OverrideSetLightingByZone;
            On.SceneManager.SetLighting -= OverrideSetLightingByScene;
            Disco.Unload();
        }

        public override string GetVersion() => "1.0";

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
    }
}
