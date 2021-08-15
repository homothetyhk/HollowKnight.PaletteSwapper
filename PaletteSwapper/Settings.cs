using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalEnums;
using Modding;
using UnityEngine;

namespace PaletteSwapper
{
    [Serializable]
    public class GlobalSettings
    {
        public bool RandomByMapZone = true;
        public bool RandomByRoom = false;
        public bool Disco = false;
        public float DiscoTimer = 0.75f;
        public bool LighterColors = false;
        public bool DarkerColors = false;
        public bool UsePaletteFromSettings = false;
        public Dictionary<string, SerializableColor> Palette = Enum.GetNames(typeof(MapZone)).ToDictionary(zone => zone, zone => new SerializableColor(1f, 1f, 1f, 1f));
    }

    public readonly struct SerializableColor
    {
        public readonly float r;
        public readonly float g;
        public readonly float b;
        public readonly float a;

        [Newtonsoft.Json.JsonConstructor]
        public SerializableColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static implicit operator Color(SerializableColor color)
        {
            return new Color(color.r, color.g, color.b, color.a);
        }

    }

}
