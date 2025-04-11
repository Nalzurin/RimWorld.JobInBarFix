using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using RimWorld.Planet;
using HarmonyLib;
using UnityEngine;
using DarkLog;

namespace JobInBar
{
    /// <summary>
    /// "Singleton" class that keeps track of all pawns that the player has set a label for
    /// </summary>
    public class LabelsTracker_WorldComponent : WorldComponent
    {
        private Dictionary<Pawn, LabelData> TrackedPawns = new();
        private List<Pawn> pawns = new List<Pawn>();
        private List<LabelData> data = new List<LabelData>();
        public static LabelsTracker_WorldComponent instance;

        public LabelData GetPawnLabelData(Pawn pawn)
        {
            if(pawn == null)
            {
                LogPrefixed.Error("Pawn is null.");
                return null;
            }
            if (!TrackedPawns.ContainsKey(pawn))
            {
                AddTrackedPawn(pawn, out var labelData);
                return labelData;
            }
            return TrackedPawns[pawn];
        }
        private bool AddTrackedPawn(Pawn pawn, out LabelData labelData)
        {
            LabelData newData = new LabelData(pawn);
            TrackedPawns[pawn] = newData;
            labelData = newData;
            return true;
        }
        public bool TryAddTrackedPawn(Pawn pawn, out LabelData labelData)
        {
            if(pawn == null)
            {
                LogPrefixed.Error("Pawn is null.");
                labelData = null;
                return false;
            }
            if (TrackedPawns.ContainsKey(pawn))
            {
                LogPrefixed.Warning("Pawn is already being tracked.");
                labelData = GetPawnLabelData(pawn);
                return false;
            }
            return AddTrackedPawn(pawn, out labelData);
        }
        public bool TryRemoveTrackedPawn(Pawn pawn)
        {
            if(pawn == null)
            {
                LogPrefixed.Error("Pawn is null.");
                return false;
            }
            if (!TrackedPawns.ContainsKey(pawn))
            {
                LogPrefixed.Warning("Pawn is not being tracked.");
                return false;
            }
            TrackedPawns.Remove(pawn);
            return true;
        }
        public LabelsTracker_WorldComponent(World world) : base(world)
        {
            LabelsTracker_WorldComponent.instance = this;
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look<Pawn, LabelData>(ref TrackedPawns, "TrackedPawns", LookMode.Reference, LookMode.Deep, ref pawns, ref data);
        }
    }
}
