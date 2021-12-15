using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class GameLocalResources
    {
        //FTUE
        public static Dictionary<string, Dictionary<string, string>> sexMainAnimPath = new Dictionary<string, Dictionary<string, string>>
        {
            ["sexMainScene1"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/UlviSexScene1/MainScene/back_SkeletonData",
                ["idle"] = "FTUE/UlviSexScene1/MainScene/idle01_SkeletonData"
            },

        };

        public static Dictionary<string, Dictionary<string, string>> sexCutInAnimPath = new Dictionary<string, Dictionary<string, string>>
        {
            ["sexCutIn1"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/UlviSexScene1/Cut_in2/back_SkeletonData",
                ["idle"] = "FTUE/UlviSexScene1/Cut_in2/idle01_SkeletonData"
            },

        };

        public static Dictionary<string, Dictionary<string, string>> dialogCutInAnimPath = new Dictionary<string, Dictionary<string, string>>
        {
            ["dialogCutIn1"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/UlviSexScene1/Cut_in2/back_SkeletonData",
                ["idle"] = "FTUE/UlviSexScene1/Cut_in2/idle01_SkeletonData"
            },

        };

        public static Dictionary<string, Dictionary<string, string>> emotionsAnimPath = new Dictionary<string, Dictionary<string, string>>
        {
            [AdminBRO.DialogCharacterKey.Overlord] = new Dictionary<string, string>
            {
                [AdminBRO.DialogCharacterAnimation.Angry] = null,
                [AdminBRO.DialogCharacterAnimation.Happy] = null,
                [AdminBRO.DialogCharacterAnimation.Idle] = null,
                [AdminBRO.DialogCharacterAnimation.Love] = null,
                [AdminBRO.DialogCharacterAnimation.Surprised] = null,
            },
            [AdminBRO.DialogCharacterKey.Ulvi] = new Dictionary<string, string>
            {
                [AdminBRO.DialogCharacterAnimation.Angry] = null,
                [AdminBRO.DialogCharacterAnimation.Happy] = null,
                [AdminBRO.DialogCharacterAnimation.Idle] = null,
                [AdminBRO.DialogCharacterAnimation.Love] = null,
                [AdminBRO.DialogCharacterAnimation.Surprised] = null,
            },
            [AdminBRO.DialogCharacterKey.UlviWolf] = new Dictionary<string, string>
            {
                [AdminBRO.DialogCharacterAnimation.Angry] = "FTUE/Emotions/UlviFurry/angry_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Happy] = "FTUE/Emotions/UlviFurry/happy_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Idle] = "FTUE/Emotions/UlviFurry/idle_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Love] = "FTUE/Emotions/UlviFurry/love_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Surprised] = "FTUE/Emotions/UlviFurry/surprised_SkeletonData",
            },
            [AdminBRO.DialogCharacterKey.Faye] = new Dictionary<string, string>
            {
                [AdminBRO.DialogCharacterAnimation.Angry] = null,
                [AdminBRO.DialogCharacterAnimation.Happy] = null,
                [AdminBRO.DialogCharacterAnimation.Idle] = null,
                [AdminBRO.DialogCharacterAnimation.Love] = null,
                [AdminBRO.DialogCharacterAnimation.Surprised] = null,
            },
            [AdminBRO.DialogCharacterKey.Adriel] = new Dictionary<string, string>
            {
                [AdminBRO.DialogCharacterAnimation.Angry] = null,
                [AdminBRO.DialogCharacterAnimation.Happy] = null,
                [AdminBRO.DialogCharacterAnimation.Idle] = null,
                [AdminBRO.DialogCharacterAnimation.Love] = null,
                [AdminBRO.DialogCharacterAnimation.Surprised] = null,
            },
        };
        //
    }

}
