using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Overlewd
{
    public static class MemoryOprimizer
    {
        private static int numChangeScreen = 0;

        private static void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void PrepareChangeScreen()
        {
            ResourceManager.MarkUnusedAssetBundles();
        }

        public static void ChangeScreen()
        {
            Resources.UnloadUnusedAssets();
            ResourceManager.UnloadUnusedAssetBundles();

            if (++numChangeScreen > 10)
            {
                numChangeScreen = 0;
                ForceGC();
            }
        }
    }

}
