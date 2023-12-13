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
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void TriggerPatch(ref AudioClip ___mineTrigger)
        {
            AudioClip newSFX = CursedSoundMod.newSFX;
            ___mineTrigger = newSFX;
        }
    }
}
