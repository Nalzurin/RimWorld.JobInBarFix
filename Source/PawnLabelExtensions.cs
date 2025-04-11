using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld.Planet;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using HarmonyLib;
using DarkLog;

namespace JobInBar
{
    public static class PawnLabelExtensions
    {
        public static bool GetShouldDrawPermanentLabels(this Pawn colonist, Rect rect)
        {
            if (Settings.ModEnabled == false || DoPlaySettingsGlobalControls_ShowLabelsToggle.drawLabels == false)
                return false;

            if (Settings.DrawLabelOnlyOnHover && !Mouse.IsOver(rect))
                return false;

            return true;
        }

        public static bool GetShouldDrawJobLabel(this Pawn colonist)
        {
            if (colonist == null)
            {
                return false;
            }
            if (!Settings.DrawJobTitle)
            {
                return false;
            }
            if (LabelsTracker_WorldComponent.instance != null)
            {
                LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(colonist);
                return data.ShowBackstory;
            }
            if (!Settings.OnlyDrawJobIfCustom)
            {
                return true;
            }
            return colonist.story.title != null;
        }

        public static bool GetShouldDrawIdeoRoleLabel(this Pawn colonist)
        {
            if (colonist == null)
            {
                return false;
            }
            if (!Settings.DrawIdeoRoles)
            {
                return false;
            }
            if (LabelsTracker_WorldComponent.instance != null)
            {
                LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(colonist);
                if (!data.ShowIdeoRole)
                {
                    return false;
                }

                return colonist.ideo?.Ideo?.GetRole(colonist) is Precept_Role;
            }
            return false;
        }

        public static bool GetShouldDrawRoyalTitleLabel(this Pawn colonist)
        {
            if (colonist == null)
            {
                return false;
            }
            if (!Settings.DrawRoyalTitles)
            {
                return false;
            }
            if (LabelsTracker_WorldComponent.instance != null)
            {
                LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(colonist);
                if (!data.ShowRoyalTitle)
                {
                    return false;
                }

                return colonist.royalty?.MainTitle() is RoyalTitleDef;
            }
            return false;
        }

        public static Color GetBackstoryLabelColor(this Pawn pawn)
        {
            if (pawn == null)
            {
                return Color.clear;
            }
            if (LabelsTracker_WorldComponent.instance == null)
            {
                return Settings.DefaultJobLabelColor;

            }
            LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(pawn);
            return data.BackstoryColor;

        }
        public static bool SetShouldDrawBackstoryLabel(this Pawn pawn, bool shouldDraw)
        {
            if (pawn == null)
            {
                return false;
            }
            if (LabelsTracker_WorldComponent.instance == null)
            {
                return false;
            }
            LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(pawn);
            data.ShowBackstory = shouldDraw;
            return true;
        }
        public static bool SetBackstoryLabelColor(this Pawn pawn, Color newColor)
        {
            if (pawn == null)
            {
                return false;
            }
            if (LabelsTracker_WorldComponent.instance == null)
            {
                return false;
            }
            LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(pawn);
            data.BackstoryColor = newColor;
            return true;
        }
        public static Color GetIdeoRoleLabelColor(this Pawn pawn)
        {
            if (pawn == null)
            {
                return Color.clear;
            }
            if (LabelsTracker_WorldComponent.instance == null)
            {
                return pawn.GetDefaultIdeoLabelColor();

            }
            LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(pawn);
            return data.IdeoRoleColor;

        }
        public static bool SetShouldDrawIdeoRoleLabel(this Pawn pawn, bool shouldDraw)
        {
            if (pawn == null)
            {
                return false;
            }
            if (LabelsTracker_WorldComponent.instance == null)
            {
                return false;
            }
            LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(pawn);
            data.ShowIdeoRole = shouldDraw;
            return true;
        }
        public static bool SetIdeoRoleLabelColor(this Pawn pawn, Color newColor)
        {
            if (pawn == null)
            {
                return false;
            }
            if (LabelsTracker_WorldComponent.instance == null)
            {
                return false;
            }
            LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(pawn);
            data.IdeoRoleColor = newColor;
            return true;

        }
        public static Color GetRoyalTitleLabelColor(this Pawn pawn)
        {
            if (pawn == null)
            {
                return Color.clear;
            }
            if (LabelsTracker_WorldComponent.instance == null)
            {
                return LabelUtils.imperialColor;

            }
            LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(pawn);
            return data.RoyalTitleColor;

        }
        public static bool SetShouldDrawRoyalTitleLabel(this Pawn pawn, bool shouldDraw)
        {
            if (pawn == null)
            {
                return false;
            }
            if (LabelsTracker_WorldComponent.instance == null)
            {
                return false;
            }
            LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(pawn);
            data.ShowRoyalTitle = shouldDraw;
            return true;
        }
        public static bool SetRoyalTitleLabelColor(this Pawn pawn, Color newColor)
        {
            if (pawn == null)
            {
                return false;
            }
            if (LabelsTracker_WorldComponent.instance == null)
            {
                return false;
            }
            LabelData data = LabelsTracker_WorldComponent.instance.GetPawnLabelData(pawn);
            data.RoyalTitleColor = newColor;
            return true;
        }

        // Looks up the pawn's ideology and returns the rgb color associated with that ideology, adjusting it for readability
        public static Color GetDefaultIdeoLabelColor(this Pawn pawn)
        {
            if (Settings.UseIdeoColorForRole && pawn?.ideo?.Ideo?.GetRole(pawn) is Precept_Role role)
            {
                bool abilityReady = pawn?.ideo?.Ideo?.GetRole(pawn)?.AbilitiesFor(pawn)[0]?.CanCast ?? false;
                if (!Settings.RoleColorOnlyIfAbilityAvailable || (Settings.RoleColorOnlyIfAbilityAvailable && abilityReady))
                {
                    // Brighten ideo colors so dark ones are readable
                    // Magic number: 0.35f for the lerp to white, to brighten every ideo color by a set amount. 
                    // tested with black to assure minimum readability
                    return Color.Lerp(role.ideo.colorDef.color, Color.white, 0.35f);
                }
            }

            return PawnNameColorUtility.PawnNameColorOf(pawn);
        }

        public static string GetJobLabel(this Pawn colonist) => colonist?.story?.TitleShortCap;

        public static string GetIdeoRoleLabel(this Pawn colonist) => colonist?.ideo?.Ideo?.GetRole(colonist)?.LabelForPawn(colonist);

        public static string GetRoyalTitleLabel(this Pawn colonist) => colonist?.royalty?.MainTitle()?.GetLabelCapFor(colonist);

        //TODO: Replace this with something smarter instead of just checking settings
        public static float GetLabelPositionOffset(this Pawn colonist)
        {
            var equipment = colonist.equipment;
            if (
                    (
                        Prefs.ShowWeaponsUnderPortraitMode == ShowWeaponsUnderPortraitMode.Always ||
                        (Prefs.ShowWeaponsUnderPortraitMode == ShowWeaponsUnderPortraitMode.WhileDrafted && colonist.Drafted)
                    ) &&
                    colonist?.equipment?.Primary is ThingWithComps equipped && equipped.def.IsWeapon
                )
            {
                return ColonistBar.BaseSize.y * Find.ColonistBar.Scale * 0.75f;
            }

            return 0f;
        }

        public static Caravan GetCaravan(this Pawn pawn) => pawn.ParentHolder as Caravan;

        public static string GetJobDescription(this Pawn pawn)
        {
            string text = "";
            Pawn_JobTracker jobs = pawn.jobs;
            if (pawn?.jobs?.curDriver is JobDriver curDriver)
            {
                text = curDriver.GetReport();
            }
            else
            {
                bool inCaravan = pawn.GetCaravan() != null;
                if (inCaravan)
                {
                    text = pawn.GetCaravan().LabelCap + ": " + pawn.GetCaravan().GetInspectString();
                    if (text.Contains('\n'))
                    {
                        text = text.Substring(0, text.IndexOf('\n'));
                    }
                }
            }
            return text.CapitalizeFirst();
        }
    }
}
