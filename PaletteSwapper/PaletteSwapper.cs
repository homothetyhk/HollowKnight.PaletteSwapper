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
#pragma warning disable CS0618
    public class PaletteSwapper : Mod<SaveSettings,GlobalSettings>, ITogglableMod
    {
        public static PaletteSwapper instance;

        Random rand;
        Dictionary<string, Color> palette;
        Color defaultColor;

        public override void Initialize()
        {
            instance = this;
            GlobalSettings.Setup();

            rand = new Random();
            palette = new Dictionary<string, Color>();
            defaultColor = RandomColor();

            if (GlobalSettings.Disco)
            {
                Disco.Setup();
            }
            else if (GlobalSettings.RandomByMapZone)
            {
                On.SceneManager.SetLighting += OverrideSetLightingByZone;
            }
            else if (GlobalSettings.RandomByRoom)
            {
                On.SceneManager.SetLighting += OverrideSetLightingByScene;
            }
            else if (GlobalSettings.UsePaletteFromSettings)
            {
                foreach (var kvp in GlobalSettings.Palette)
                {
                    string zone = kvp.Key.Split('.')[0];
                    if (!palette.ContainsKey(zone))
                    {
                        try
                        {
                            palette[zone] = new Color
                            {
                                r = GlobalSettings.Palette.FirstOrDefault(_kvp => _kvp.Key.Split('.')[1] == "r").Value,
                                g = GlobalSettings.Palette.FirstOrDefault(_kvp => _kvp.Key.Split('.')[1] == "g").Value,
                                b = GlobalSettings.Palette.FirstOrDefault(_kvp => _kvp.Key.Split('.')[1] == "b").Value,
                                a = GlobalSettings.Palette.FirstOrDefault(_kvp => _kvp.Key.Split('.')[1] == "a").Value,
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
            if (GlobalSettings.LighterColors)
            {
                return new Color
                {
                    r = 0.5f + (float)rand.NextDouble() / 2,
                    g = 0.5f + (float)rand.NextDouble() / 2,
                    b = 0.5f + (float)rand.NextDouble() / 2,
                    a = 0.5f
                };
            }

            else if (GlobalSettings.DarkerColors)
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
