using System;
using System.Linq;
using Verse;
using RimWorld;
using HarmonyLib;
using UnityEngine;
using ColourPicker;
using System.Collections.Generic;

namespace JobInBar
{

    [HarmonyPatch(typeof(Dialog_NamePawn))]
    [HarmonyPatch("DoWindowContents")]
    public class Dialog_NamePawn_DoWindowContents_Patch
    {
        public static void Postfix(ref Pawn ___pawn, Rect inRect)
        {
            Pawn pawn = ___pawn;
            if (pawn == null)
            {
                return;
            }
            Rect rectManageLabels = new Rect(inRect.width - 140f, inRect.yMin, 100f, 36f);

            if (Widgets.ButtonText(rectManageLabels, "JobInBar_ManageLabels".Translate()))
            {
                Find.WindowStack.Add(new Dialog_ManageLabels(pawn));
            }

        }
    }
}