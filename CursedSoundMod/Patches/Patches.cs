using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace CursedSoundMod.Patches
{
    [HarmonyPatch(typeof(Landmine))]
    internal class LandmineTriggerPatch
    {
        [HarmonyPatch("SetOffMineAnimation")]
        [HarmonyPrefix]
        private static void TriggerPatch(ref AudioSource ___mineAudio)
        {
            AudioClip newSFX = CursedSoundMod.newSFX;
            ___mineAudio.PlayOneShot(newSFX, 1f);
        }
    }

    [HarmonyPatch(typeof(AnimatedObjectTrigger))]
    internal class RecordPlayerPatch
    {
        [HarmonyPatch("PlayAudio")]
        [HarmonyPrefix]
        private static void RecPatch(ref InteractTrigger __instance, ref AudioClip ___playWhileTrue, bool boolVal, bool playSecondaryAudios = false)
        {
            AudioClip newSFX = CursedSoundMod.erika;
            try
            {
                bool flag = false;
                Component[] componentsInParent = ((Component)__instance).gameObject.GetComponentsInParent(typeof(Component));
                foreach (Component val in componentsInParent)
                {
                    if ((val).name.Contains("RecordPlayer"))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && newSFX != null)
                {
                    ___playWhileTrue = newSFX;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}