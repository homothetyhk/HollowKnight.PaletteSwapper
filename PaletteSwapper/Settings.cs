using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalEnums;
using Modding;

namespace PaletteSwapper
{
    [Serializable]
    public class GlobalSettings : ModSettings
    {
        public void Setup()
        {
            if (Palette != null) return;

            RandomByMapZone = true;
            RandomByRoom = false;
            UsePaletteFromSettings = false;
            Disco = false;
            DiscoTimer = 0.75f;
            LighterColors = false;
            DarkerColors = false;
            Palette = new SerializableFloatDictionary();
            foreach (string zone in Enum.GetNames(typeof(MapZone)))
            {
                Palette[zone + ".r"] = 1f;
                Palette[zone + ".g"] = 1f;
                Palette[zone + ".b"] = 1f;
                Palette[zone + ".a"] = 1f;
            }
            PaletteSwapper.instance.GlobalSettings = this;
        }

        public bool RandomByMapZone;
        public bool RandomByRoom;
        public bool Disco;
        public float DiscoTimer;
        public bool LighterColors;
        public bool DarkerColors;
        public bool UsePaletteFromSettings;
        public SerializableFloatDictionary Palette;
    }
}
