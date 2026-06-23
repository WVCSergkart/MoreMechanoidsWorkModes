using RimWorld;
using RimWorld.Planet;
using System;
using UnityEngine;
using Verse;
using Verse.Sound;
using static RimWorld.MechClusterSketch;

namespace WVC_WorkModes
{

	public class PawnColumnWorker_AssignEscort : PawnColumnWorker
	{

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Action pasteAction = null;
			CompMechSettings compMechSettings = pawn.GetMechSettings();
			if (compMechSettings.escortTarget != null)
			{
				pasteAction = delegate
				{
					ResetButton(compMechSettings);
				};
			}
			DoButtons(new Rect(rect.x, rect.y, 36f, 30f), delegate
			{
				AssignButton(compMechSettings);
			}, pasteAction, compMechSettings);
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), GetMinWidth(table));
		}

		protected void AssignButton(CompMechSettings compMechSettings)
		{
			compMechSettings.DoAssign(true);
		}

		protected void ResetButton(CompMechSettings compMechSettings)
		{
			compMechSettings.escortTarget = null;
		}

		public static void DoButtons(Rect rect, Action copyAction, Action pasteAction, CompMechSettings compMechSettings)
		{
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height / 2f - 12f), 18f, 24f);
			if (Widgets.ButtonImage(rect2, GraphicCache.Icon_EscortTargetAssign.Texture))
			{
				copyAction();
				SoundDefOf.Tick_High.PlayOneShotOnCamera();
			}
			string key = "WVC_WorkModes_AssignToPawnEscortDesc".Translate(compMechSettings.Mech.LabelIndefinite().CapitalizeFirst(), compMechSettings.GetEscortee(compMechSettings.Overseer).LabelIndefinite().CapitalizeFirst()).ToString();
			TooltipHandler.TipRegionByKey(rect2, key);
			if (pasteAction != null)
			{
				Rect rect3 = rect2;
				rect3.x = rect2.xMax;
				if (Widgets.ButtonImage(rect3, GraphicCache.Icon_EscortTargetAssignReset.Texture))
				{
					pasteAction();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera();
				}
				TooltipHandler.TipRegionByKey(rect3, key);
			}
		}

	}

}
