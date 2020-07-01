using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalEnums;
using Modding;

namespace PaletteSwapper
{
    public class SaveSettings : ModSettings
    {

    }
    public class GlobalSettings : ModSettings
    {
        public void Setup()
        {
            if (Palette != null) return;

            Palette = new SerializableFloatDictionary();
            foreach (string zone in Enum.GetNames(typeof(MapZone)))
            {
                Palette[zone + ".r"] = 1f;
                Palette[zone + ".g"] = 1f;
                Palette[zone + ".b"] = 1f;
                Palette[zone + ".a"] = 1f;
            }

            RandomByMapZone = true;
            RandomByRoom = false;
            UsePaletteFromSettings = false;
            Disco = false;
            DiscoTimer = 0.75f;
            LighterColors = false;
            DarkerColors = false;
            PaletteSwapper.instance.SaveGlobalSettings();
        }

        public bool RandomByMapZone
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool RandomByRoom
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool UsePaletteFromSettings
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public SerializableFloatDictionary Palette;

        public bool Disco
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public float DiscoTimer
        {
            get => GetFloat(0.75f);
            set => SetFloat(value);
        }

        public bool LighterColors
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool DarkerColors
        {
            get => GetBool(false);
            set => SetBool(value);
        }

    }
}
