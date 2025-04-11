﻿using System;
using Verse;
using RimWorld;
using HarmonyLib;
using UnityEngine;
using DarkLog;

namespace JobInBar
{
    public class JobInBarMod : Verse.Mod
    {
        public static JobInBarMod instance;
        public static JobInBarMod Instance => instance;
        public JobInBarMod(ModContentPack content) : base(content)
        {
            instance = this;
            LogPrefixed.modInst = this;

            GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            GetSettings<Settings>().DoWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "JobInBar_SettingsCategory".Translate();
        }
    }
}
