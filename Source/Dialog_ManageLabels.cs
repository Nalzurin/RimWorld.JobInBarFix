using ColourPicker;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace JobInBar
{
    public class Dialog_ManageLabels : Window
    {
        private Pawn pawn;
        private float innerMargin = 10f;
        private float rectHeight = 36f;
        private float textRatio = 0.5f;
        private float checkBoxRatio = 0.25f;
        public override Vector2 InitialSize => new Vector2(300, 250);
        public Dialog_ManageLabels(Pawn _pawn)
        {
            pawn = _pawn;
            doCloseButton = true;
        }
        public override void DoWindowContents(Rect inRect)
        {

            // Backstory/Custom title

            Rect backstoryRect = inRect;
            backstoryRect.xMin += innerMargin;
            backstoryRect.xMax -= innerMargin;
            backstoryRect.height = rectHeight;
            //Template
            Rect templateLabelRect = new Rect(backstoryRect.xMin, backstoryRect.yMin, backstoryRect.width * textRatio, rectHeight);
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(templateLabelRect, "JobInBar_Label".Translate());
            Rect templateDoDrawRect = new Rect(templateLabelRect.xMax + innerMargin, backstoryRect.yMin, backstoryRect.width * checkBoxRatio, rectHeight);
            Widgets.Label(templateDoDrawRect, "JobInBar_ShowJobLabelFor".Translate());
            Rect templateDoColorRect = new Rect(templateDoDrawRect.xMax + innerMargin, backstoryRect.yMin, backstoryRect.width * checkBoxRatio, rectHeight);
            Widgets.Label(templateDoColorRect, "JobInBar_Color".Translate());
            Text.Anchor = TextAnchor.UpperLeft;

            //Backstory
            Rect backstoryLabelRect = new Rect(backstoryRect.xMin, templateLabelRect.yMax + innerMargin, backstoryRect.width * textRatio, rectHeight);
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(backstoryLabelRect, "JobInBar_Dialog_ManageLabels_BackstoryOrCustomLabel".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            bool shouldDrawBackstory = pawn.GetShouldDrawJobLabel();
            Widgets.Checkbox(new Vector2(templateDoDrawRect.xMin + templateDoDrawRect.width / 2 - 12, backstoryLabelRect.yMin), ref shouldDrawBackstory);
            pawn.SetShouldDrawBackstoryLabel(shouldDrawBackstory);
            Color BackstoryCol = pawn.GetBackstoryLabelColor();
            Rect backstoryColorRect = new Rect(templateDoColorRect.xMin + templateDoDrawRect.width / 2 - 18, backstoryLabelRect.yMin, 36f, rectHeight);
            if (Widgets.ButtonInvisible(backstoryColorRect, true))
            {
                Find.WindowStack.Add(
                    new Dialog_ColourPicker(
                        BackstoryCol.ToOpaque(),
                        (newColor) => pawn.SetBackstoryLabelColor(newColor)
                    )
                );
            }
            Widgets.DrawBoxSolid(backstoryColorRect, BackstoryCol);

            // Royalty Title
            Rect royaltyLabelRect = new Rect(backstoryRect.xMin, backstoryLabelRect.yMax + innerMargin, backstoryRect.width * textRatio, rectHeight);
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(royaltyLabelRect, "JobInBar_Dialog_ManageLabels_RoyaltyTitleLabel".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            bool shouldDrawRoyalty = pawn.GetShouldDrawRoyalTitleLabel();
            Widgets.Checkbox(new Vector2(templateDoDrawRect.xMin + templateDoDrawRect.width / 2 - 12, royaltyLabelRect.yMin), ref shouldDrawRoyalty, disabled: pawn.royalty?.MainTitle() is null);
            pawn.SetShouldDrawRoyalTitleLabel(shouldDrawRoyalty);
            Color RoyaltyCol = pawn.GetRoyalTitleLabelColor();
            Rect RoyaltyColorRect = new Rect(templateDoColorRect.xMin + templateDoDrawRect.width / 2 - 18, royaltyLabelRect.yMin, 36f, rectHeight);
            if (Widgets.ButtonInvisible(RoyaltyColorRect, true))
            {
                Find.WindowStack.Add(
                    new Dialog_ColourPicker(
                        RoyaltyCol.ToOpaque(),
                        (newColor) => pawn.SetRoyalTitleLabelColor(newColor)
                    )
                );
            }
            Widgets.DrawBoxSolid(RoyaltyColorRect, RoyaltyCol);
            //Ideology title
            Rect ideologyLabelRect = new Rect(backstoryRect.xMin, royaltyLabelRect.yMax + innerMargin, backstoryRect.width * textRatio, rectHeight);
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(ideologyLabelRect, "JobInBar_Dialog_ManageLabels_IdeologyRoleLabel".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            bool shouldDrawIdeology = pawn.GetShouldDrawIdeoRoleLabel();
            Widgets.Checkbox(new Vector2(templateDoDrawRect.xMin + templateDoDrawRect.width / 2 - 12, ideologyLabelRect.yMin), ref shouldDrawIdeology, disabled: pawn.ideo?.Ideo?.GetRole(pawn) is null);
            pawn.SetShouldDrawIdeoRoleLabel(shouldDrawIdeology);
            Color ideologyCol = pawn.GetIdeoRoleLabelColor();
            Rect ideologyColorRect = new Rect(templateDoColorRect.xMin + templateDoDrawRect.width / 2 - 18, ideologyLabelRect.yMin, 36f, rectHeight);
            if (Widgets.ButtonInvisible(ideologyColorRect, true))
            {
                Find.WindowStack.Add(
                    new Dialog_ColourPicker(
                        ideologyCol.ToOpaque(),
                        (newColor) => pawn.SetIdeoRoleLabelColor(newColor)
                    )
                );
            }
            Widgets.DrawBoxSolid(ideologyColorRect, ideologyCol);

        }
    }
}
