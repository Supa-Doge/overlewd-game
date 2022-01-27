using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Overlewd
{
    public static class SoundManager
    {
        public class SoundPath
        {
            public class UI
            {
                public const string CastleScreenButtons = "event:/UI/Buttons/Basic_menu_click";
                public const string CastleWindowSlideOn = "event:/UI/Windows/Window_slide_on";
                public const string CastleWindowSlideOff = "event:/UI/Windows/Window_slide_off";
                public const string SidebarOverlayOn = "event:/UI/Windows/Deeds_slide_on";
                public const string SidebarOverlayOff = "event:/UI/Windows/Deeds_slide_off";
            }

            public class Animations
            {
                public const string CutInCumshot = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/add_cumshot";
                public const string CutInLick = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/add_lick";
                public const string MainScene = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/main_scene";
            }
        }

        private static EventInstance music;
        private static EventInstance animationSound;
        private static EventInstance cutInSound;

        private static void StopInstance(EventInstance instance, bool allowFade)
        {
            if (allowFade)
            {
                instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                instance.release();
                return;
            }

            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }

        public static void Initialize()
        {
            
        }
        
        public static void StopAllInstance(bool allowFade)
        {
            StopCutInSound(allowFade);
            StopAnimationSound(allowFade);
        }
        
        //Sound
        public static void PlayUISound(string soundEventPath)
        {
            RuntimeManager.PlayOneShot(soundEventPath);
        }
        
        //Music
        public static void InstantiateMusic(string eventPath)
        {
            music = RuntimeManager.CreateInstance(eventPath);
        }

        public static void PlayMusic()
        {
            music.start();
        }

        public static void StopMusic(bool allowFade)
        {
            StopInstance(music, allowFade);
        }

        public static void PauseMusic()
        {
            music.setPaused(true);
        }

        public static void UnpauseMusic()
        {
            music.setPaused(false);
        }
        
        //Animations
        public static void PlayAnimationSound(string soundName)
        {
            animationSound = RuntimeManager.CreateInstance(soundName);
            animationSound.start();
        }

        public static void PauseAnimationSound()
        {
            animationSound.setPaused(true);
        }

        public static void UnpauseAnimationSound()
        {
            animationSound.setPaused(false);
        }

        public static void StopAnimationSound(bool allowFade)
        {
            StopInstance(animationSound, allowFade);
        }
        
        //CutIn
        public static void PlayCutInSound(string soundName)
        {
            cutInSound = RuntimeManager.CreateInstance(soundName);
            cutInSound.start();
        }

        public static void StopCutInSound(bool allowFade)
        {
            StopInstance(cutInSound, allowFade);
        }

    }
}