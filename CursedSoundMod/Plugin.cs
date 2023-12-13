using System.IO;
using BepInEx;
using BepInEx.Logging;
using CursedSoundMod.Patches;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;


namespace CursedSoundMod
{

    [BepInPlugin(modGUID, modName, modVersion)]
    public class CursedSoundMod : BaseUnityPlugin
    {
        private const string modGUID = "toemmsen.CursedSoundMod";
        private const string modName = "Cursed Trigger Sound";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static string audioFile = "mineTrigger.mp3";

        private static string uPath = Path.Combine(Paths.PluginPath + "\\Booombitch\\");

        private static CursedSoundMod instance;

        internal ManualLogSource mls;

        internal static AudioClip newSFX;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }


            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo((object)"Cursed Mine Sound loading...");
            newSFX = LoadAudioClip(uPath + audioFile);
            if ((object)newSFX == null)
            {
                mls.LogError((object)"Failed to load audio clip");
            }
            
            harmony.PatchAll(typeof(CursedSoundMod));
            harmony.PatchAll(typeof(LandmineTriggerPatch));

            mls.LogInfo((object)"Cursed Mine sound loaded!");
        }

        private static AudioClip LoadAudioClip(string filepath)
        {
            //IL_006c: Unknown result type (might be due to invalid IL or missing references)
            //IL_0072: Invalid comparison between Unknown and I4
            ManualLogSource val = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            val.LogInfo((object)("Loading audio clip from " + filepath));
            UnityWebRequest audioClip = UnityWebRequestMultimedia.GetAudioClip(filepath, (AudioType)13);
            audioClip.SendWebRequest();
            while (!audioClip.isDone)
            {
            }
            if (audioClip.error != null)
            {
                val.LogError((object)"Failed to load audio clip");
            }
            AudioClip content = DownloadHandlerAudioClip.GetContent(audioClip);
            if (content != null && (int)content.loadState == 2)
            {
                val.LogInfo((object)"Loaded audio clip successfully!");
                return content;
            }
            return null;
        }
    }
}