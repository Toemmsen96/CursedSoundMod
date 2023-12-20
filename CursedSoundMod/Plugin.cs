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
        private const string modName = "ToemmsensCursedSoundMod";
        private const string modVersion = "1.0.4";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static string audioFile = "mineTrigger.mp3";
        private static string audioFile2 = "skibidi.mp3";
        private static string audioFile3 = "erika.mp3";


        private static string uPath = null;

        private static CursedSoundMod instance;

        internal ManualLogSource mls;

        internal static AudioClip newSFX;
        internal static AudioClip skibidi;
        internal static AudioClip erika;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo((object)"Cursed Sounds loading...");

            // Dynamically find the Booombitch folder
            uPath = FindBooombitchFolder();

            // Check if the folder is found
            if (uPath == null)
            {
                mls.LogError((object)"Booombitch folder not found!");
                return;
            }

            newSFX = LoadAudioClip(uPath + "\\" + audioFile);
            skibidi = LoadAudioClip(uPath + "\\" + audioFile2);
            erika = LoadAudioClip(uPath + "\\" + audioFile3);

            if ((object)newSFX == null || (object)skibidi == null || (object)erika == null)
            {
                mls.LogError((object)"Failed to load an audio clip");
            }

            harmony.PatchAll(typeof(CursedSoundMod));
            harmony.PatchAll(typeof(LandmineTriggerPatch));
            harmony.PatchAll(typeof(RecordPlayerPatch));

            mls.LogInfo((object)"Cursed Mine sound loaded!");
        }

        private string FindBooombitchFolder()
        {
            // Get the root directory of the application
            string rootDirectory = Paths.PluginPath;

            // Search for Booombitch folder
            string[] matchingFolders = Directory.GetDirectories(rootDirectory, "Booombitch", SearchOption.AllDirectories);

            // Check if Booombitch folder is found
            if (matchingFolders.Length > 0)
            {
                return matchingFolders[0]; // Return the first found Booombitch folder
            }

            return null; // Booombitch folder not found
        }

        private static AudioClip LoadAudioClip(string filepath)
        {
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