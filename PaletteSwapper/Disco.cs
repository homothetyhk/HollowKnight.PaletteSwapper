using Modding;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace PaletteSwapper
{
    public class Disco : MonoBehaviour
    {
        private static GameObject parent;
        private static Color color;

        public static void Setup()
        {
            parent = new GameObject();
            GameObject.DontDestroyOnLoad(parent);
            parent.AddComponent<Disco>().Start();
            
            On.SceneManager.SetLighting += OverrideSetLighting;
        }

        public void Start()
        {
            StartCoroutine(UpdateDiscoColor());
        }

        public static void Unload()
        {
            GameObject.Destroy(parent);
            On.SceneManager.SetLighting -= OverrideSetLighting;
        }

        private IEnumerator UpdateDiscoColor()
        {
            while (true)
            {
                color = PaletteSwapper.instance.RandomColor();
                yield return new WaitUntil(() => GameManager.instance != null && GameManager.instance.IsGameplayScene() && GameManager.instance.sm != null);
                SceneManager.SetLighting(color, SceneManager.AmbientIntesityMix);
                yield return new WaitForSeconds(PaletteSwapper.Settings.DiscoTimer);
            }
        }

        private static void OverrideSetLighting(On.SceneManager.orig_SetLighting orig, Color ambientLightColor, float ambientLightIntensity)
        {
            orig(color, ambientLightIntensity);
        }
    }
}
