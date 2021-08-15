using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modding;

namespace PaletteSwapper
{
    public partial class PaletteSwapper : IMenuMod
    {
        public bool ToggleButtonInsideMenu => true;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            IMenuMod.MenuEntry toggleMod = new IMenuMod.MenuEntry(toggleButtonEntry.Value.Name, toggleButtonEntry.Value.Values, "Toggle the PaletteSwapper mod", toggleButtonEntry.Value.Saver, toggleButtonEntry.Value.Loader);
            return new List<IMenuMod.MenuEntry>
            {
                toggleMod,
                new IMenuMod.MenuEntry("Lighter Colors", bools, "Random colors are more likely to be lighter.", ToggleLighter, GetLighter),
                new IMenuMod.MenuEntry("Darker Colors", bools, "Random color are more likely to be darker.", ToggleDarker, GetDarker),
                new IMenuMod.MenuEntry("Disco", bools, "Randomizes color tint along a recurring time interval", ToggleDisco, GetDisco),
                new IMenuMod.MenuEntry("Disco Timer", timeStrings, "Increment between color randomizations if Disco is enabled.", IncrementDisco, GetDiscoTimer),
                new IMenuMod.MenuEntry("Random By Map Zone", bools, "Randomizes color tint, but uses the same color for rooms in the same map zone.", ToggleRandomByMapZone, GetRandomByMapZone),
                new IMenuMod.MenuEntry("Random By Room", bools, "Randomizes color tint, using a different color for each room.", ToggleRandomByRoom, GetRandomByRoom),
                new IMenuMod.MenuEntry("Use Palette From Settings", bools, "Uses a fixed palette of colors for each map zone, loaded from the global settings file.", ToggleUsePalette, GetUsePalette),
            };
        }

        public readonly string[] bools = new string[] { "False", "True" };

        public static void ToggleLighter(int i)
        {
            if (i == 1)
            {
                Settings.LighterColors = true;
                Settings.DarkerColors = false;
            }
            else
            {
                Settings.LighterColors = false;
            }

            SaveGlobalSettings();
            Reload();
        }

        public static int GetLighter()
        {
            return Settings.LighterColors ? 1 : 0;
        }

        public static void ToggleDarker(int i)
        {
            if (i == 1)
            {
                Settings.DarkerColors = true;
                Settings.LighterColors = false;
            }
            else
            {
                Settings.DarkerColors = false;
            }

            SaveGlobalSettings();
            Reload();
        }

        public static int GetDarker()
        {
            return Settings.DarkerColors ? 1 : 0;
        }


        public static void ToggleDisco(int i)
        {
            if (i == 1)
            {
                Settings.Disco = true;
                Settings.RandomByMapZone = false;
                Settings.RandomByRoom = false;
                Settings.UsePaletteFromSettings = false;
            }
            else
            {
                Settings.Disco = false;
            }

            SaveGlobalSettings();
            Reload();
        }

        public static int GetDisco()
        {
            return Settings.Disco ? 1 : 0;
        }

        public static readonly float[] times = new float[] { 0.75f, 1f, 1.5f, 2f, 3f, 5f, 10f, 0.25f, 0.5f };
        public static readonly string[] timeStrings = new string[] { "0.75", "1.0", "1.5", "2", "3", "5", "10", "0.25", "0.5", "Custom" };

        public static void IncrementDisco(int i)
        {
            if (0 <= i && i < times.Length)
            {
                Settings.DiscoTimer = times[i];
            }

            SaveGlobalSettings();
        }

        public static int GetDiscoTimer()
        {
            int index = Array.IndexOf(times, Settings.DiscoTimer);
            if (0 <= index && index < times.Length) return index;
            return timeStrings.Length - 1;
        }


        public static void ToggleRandomByMapZone(int i)
        {
            if (i == 1)
            {
                Settings.Disco = false;
                Settings.RandomByMapZone = true;
                Settings.RandomByRoom = false;
                Settings.UsePaletteFromSettings = false;
            }
            else
            {
                Settings.RandomByMapZone = false;
            }

            SaveGlobalSettings();
            Reload();
        }

        public static int GetRandomByMapZone()
        {
            return Settings.RandomByMapZone ? 1 : 0;
        }

        public static void ToggleRandomByRoom(int i)
        {
            if (i == 1)
            {
                Settings.Disco = false;
                Settings.RandomByMapZone = false;
                Settings.RandomByRoom = true;
                Settings.UsePaletteFromSettings = false;
            }
            else
            {
                Settings.RandomByRoom = false;
            }

            SaveGlobalSettings();
            Reload();
        }

        public static int GetRandomByRoom()
        {
            return Settings.RandomByRoom ? 1 : 0;
        }

        public static void ToggleUsePalette(int i)
        {
            if (i == 1)
            {
                Settings.Disco = false;
                Settings.RandomByMapZone = false;
                Settings.RandomByRoom = false;
                Settings.UsePaletteFromSettings = true;
            }
            else
            {
                Settings.UsePaletteFromSettings = false;
            }

            SaveGlobalSettings();
            Reload();
        }

        public static int GetUsePalette()
        {
            return Settings.UsePaletteFromSettings ? 1 : 0;
        }

    }
}
