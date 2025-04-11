using DarkLog;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace JobInBar
{
    [StaticConstructorOnStartup]
    static class HarmonyStarter
    {
        static HarmonyStarter()
        {

            Harmony harmony = new Harmony("Dark.JobInBar");

            harmony.PatchAll();

            LogPrefixed.Message("Patching complete");
        }
    }
}
