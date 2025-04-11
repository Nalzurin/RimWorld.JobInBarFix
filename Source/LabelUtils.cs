using System;
using System.Linq;
using Verse;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;
using UnityEngine;
using HarmonyLib;
using DarkLog;
using System.Collections.Generic;

namespace JobInBar
{
    /// <summary>
    /// Class storing all of the user-set data for one pawn's label.
    /// </summary>
    public class LabelData : IExposable
    {
        public Pawn pawn;

        public bool ShowBackstory = true;

        public Color BackstoryColor = Color.white;

        public bool ShowRoyalTitle = true;

        public Color RoyalTitleColor = Color.white;

        public bool ShowIdeoRole = true;
        public Color IdeoRoleColor = Color.white;

        public LabelData(Pawn pawn)
        {
            this.pawn = pawn;
            if (pawn == null)
            {
                LogPrefixed.Error("Tried to initialize LabelData for null pawn.");
                return;
            }
            this.BackstoryColor = Settings.DefaultJobLabelColor;

            this.IdeoRoleColor = pawn.GetDefaultIdeoLabelColor();
            this.RoyalTitleColor = LabelUtils.imperialColor;
        }

        public void ExposeData()
        {
            Scribe_References.Look(ref pawn, "pawn");
            Scribe_Values.Look(ref ShowBackstory, "ShowBackstory");
            Scribe_Values.Look(ref BackstoryColor, "BackstoryColor");
            Scribe_Values.Look(ref ShowRoyalTitle, "ShowRoyalTitle");
            Scribe_Values.Look(ref RoyalTitleColor, "RoyalTitleColor");
            Scribe_Values.Look(ref ShowIdeoRole, "ShowIdeoRole");
            Scribe_Values.Look(ref IdeoRoleColor, "IdeoRoleColor");
        }
    }

    public static class LabelUtils
    {
        //private static PawnLabelCustomColors_WorldComponent labelsComp;

        public static Color imperialColor = new Color(0.85f, 0.85f, 0.75f);

        public static Rect GetLabelBGRect(Vector2 pos, float labelWidth) => new Rect(pos.x - labelWidth / 2f - 4f, pos.y, labelWidth + 8f, 12f);

        public static Rect GetLabelRect(Vector2 pos, float labelWidth)
        {
            Rect bgRect = GetLabelBGRect(pos, labelWidth);

            Text.Anchor = TextAnchor.UpperCenter;
            Rect rect = new Rect(bgRect.center.x - labelWidth / 2f, bgRect.y - 2f, labelWidth, 100f);

            return rect;
        }

        public static string TruncateLabel(string labelString, float truncateToWidth, GameFont font)
        {
            GameFont oldFont = Text.Font;
            Text.Font = font;
            labelString = labelString.Truncate(truncateToWidth);
            Text.Font = oldFont; // reset font

            return labelString;
        }
    }
}
