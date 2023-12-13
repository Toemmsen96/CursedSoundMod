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
}
