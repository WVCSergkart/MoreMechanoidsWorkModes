using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;
using WVC_WorkModes.Odyssey;

namespace WVC_WorkModes
{
	internal static class HarmonyUtility
	{

		public static bool DisabledSkills(Pawn pawn)
		{
			return pawn.skills == null; //  || pawn.Ideo == null
		}


		private static bool initialized = false;


		private static bool nullRefFix_Patch = false;
		public static void HarmonyPatch()
		{
			if (nullRefFix_Patch)
			{
				return;
			}
			try
			{
				WVC_WorkModesOdyssey_Main.Harmony.Patch(AccessTools.Method(typeof(WidgetsWork), "DrawWorkBoxBackground"), prefix: new HarmonyMethod(typeof(HarmonyUtility).GetMethod(nameof(HarmonyUtility.Patch_WidgetsWork_DrawWorkBoxBackground))));
				WVC_WorkModesOdyssey_Main.Harmony.Patch(AccessTools.Method(typeof(WidgetsWork), "DrawWorkBoxFor"), prefix: new HarmonyMethod(typeof(HarmonyUtility).GetMethod(nameof(HarmonyUtility.Patch_WidgetsWork_DrawWorkBoxFor))));
				WVC_WorkModesOdyssey_Main.Harmony.Patch(AccessTools.Method(typeof(WidgetsWork), "TipForPawnWorker"), prefix: new HarmonyMethod(typeof(HarmonyUtility).GetMethod(nameof(HarmonyUtility.Patch_WidgetsWork_TipForPawnWorker))));
				MainButtonWorker_ToggleMechTab.enabled = true;
			}
			catch (Exception arg)
			{
				Log.Error("Failed init null ref checks. Reason: " + arg.Message);
				MainButtonWorker_ToggleMechTab.enabled = false;
			}
			nullRefFix_Patch = true;
		}

		public static bool Patch_WidgetsWork_DrawWorkBoxFor(float x, float y, Pawn p, WorkTypeDef wType, bool incapableBecauseOfCapacities)
		{
			if (DisabledSkills(p))
			{
				if (p.WorkTypeIsDisabled(wType))
				{
					return false;
				}
				Rect rect2 = new Rect(x, y, 25f, 25f);
				if (incapableBecauseOfCapacities)
				{
					GUI.color = new Color(1f, 0.3f, 0.3f);
				}
				Patch_WidgetsWork_DrawWorkBoxBackground(rect2, p, wType);
				GUI.color = Color.white;
				if (Find.PlaySettings.useWorkPriorities)
				{
					int priority = p.workSettings.GetPriority(wType);
					if (priority > 0)
					{
						Text.Anchor = TextAnchor.MiddleCenter;
						GUI.color = WidgetsWork.ColorOfPriority(priority);
						Widgets.Label(rect2.ContractedBy(-3f), priority.ToStringCached());
						GUI.color = Color.white;
						Text.Anchor = TextAnchor.UpperLeft;
					}
					if (Event.current.type != EventType.MouseDown || !Mouse.IsOver(rect2))
					{
						return false;
					}
					bool num = p.workSettings.WorkIsActive(wType);
					if (Event.current.button == 0)
					{
						int num2 = p.workSettings.GetPriority(wType) - 1;
						if (num2 < 0)
						{
							num2 = 4;
						}
						p.workSettings.SetPriority(wType, num2);
						SoundDefOf.DragSlider.PlayOneShotOnCamera();
					}
					if (Event.current.button == 1)
					{
						int num3 = p.workSettings.GetPriority(wType) + 1;
						if (num3 > 4)
						{
							num3 = 0;
						}
						p.workSettings.SetPriority(wType, num3);
						SoundDefOf.DragSlider.PlayOneShotOnCamera();
					}
					if (!num && p.workSettings.WorkIsActive(wType) && wType.relevantSkills.Any() && p.RaceProps.mechFixedSkillLevel <= 2f)
					{
						SoundDefOf.Crunch.PlayOneShotOnCamera();
					}
					//if (!num && p.workSettings.WorkIsActive(wType) && p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(wType))
					//{
					//	Messages.Message("MessageIdeoOpposedWorkTypeSelected".Translate(p, wType.gerundLabel), p, MessageTypeDefOf.CautionInput, historical: false);
					//	SoundDefOf.DislikedWorkTypeActivated.PlayOneShotOnCamera();
					//}
					Event.current.Use();
					return false;
				}
				if (p.workSettings.GetPriority(wType) > 0)
				{
					GUI.DrawTexture(rect2, WidgetsWork.WorkBoxCheckTex);
				}
				if (!Widgets.ButtonInvisible(rect2))
				{
					return false;
				}
				if (p.workSettings.GetPriority(wType) > 0)
				{
					p.workSettings.SetPriority(wType, 0);
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
				}
				else
				{
					p.workSettings.SetPriority(wType, 3);
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
					if (wType.relevantSkills.Any() && p.RaceProps.mechFixedSkillLevel <= 2f)
					{
						SoundDefOf.Crunch.PlayOneShotOnCamera();
					}
				}
				return false;
			}
			return true;
		}

		public static bool Patch_WidgetsWork_DrawWorkBoxBackground(Rect rect, Pawn p, WorkTypeDef workDef)
		{
			if (DisabledSkills(p))
			{
				float num = p.RaceProps.mechFixedSkillLevel;
				Texture2D image;
				float a;
				image = WidgetsWork.WorkBoxBGTex_Mid;
				a = (num - 4f) / 10f;
				GUI.DrawTexture(rect, image);
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, a);
				GUI.DrawTexture(rect, image);
				GUI.color = Color.white;
				return false;
			}
			return true;
		}

		public static bool Patch_WidgetsWork_TipForPawnWorker(ref string __result, Pawn p, WorkTypeDef wDef, bool incapableBecauseOfCapacities)
		{
			if (DisabledSkills(p))
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = wDef.gerundLabel.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor);
				int priority = p.workSettings.GetPriority(wDef);
				text = text + ": " + ((string)("Priority" + priority).Translate()).Colorize(WidgetsWork.ColorOfPriority(priority));
				stringBuilder.AppendLine(text);
				if (p.WorkTypeIsDisabled(wDef))
				{
					string text2 = "CannotDoThisWork".Translate(p.LabelShort, p);
					List<string> reasonsForDisabledWorkType = p.GetReasonsForDisabledWorkType(wDef);
					if (!reasonsForDisabledWorkType.NullOrEmpty())
					{
						string text3 = "\n\n" + string.Join(". ", reasonsForDisabledWorkType);
						if (reasonsForDisabledWorkType.Count == 1)
						{
							text3 += ".";
						}
						text2 += text3.Colorize(ColorLibrary.RedReadable);
					}
					stringBuilder.Append(text2);
				}
				else
				{
					float num = p.RaceProps.mechFixedSkillLevel;
					if (wDef.relevantSkills.Any())
					{
						string text4 = "";
						foreach (SkillDef relevantSkill in wDef.relevantSkills)
						{
							text4 = text4 + relevantSkill.skillLabel.CapitalizeFirst() + ", ";
						}
						text4 = text4.Substring(0, text4.Length - 2);
						stringBuilder.AppendLine("RelevantSkills".Translate(text4, num.ToString("0.#"), 20));
					}
					stringBuilder.AppendLine();
					stringBuilder.Append(wDef.description);
					if (incapableBecauseOfCapacities)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						stringBuilder.Append("IncapableOfWorkTypeBecauseOfCapacities".Translate());
					}
				}
				__result = stringBuilder.ToString();
				return false;
			}
			return true;
		}

		public static bool Init(PawnTableDef pawnTableDef)
		{
			if (initialized)
			{
				return false;
			}
			initialized = true;
			HarmonyPatch();
			PawnTableDef workTable = pawnTableDef;
			foreach (PawnColumnDef item in DefDatabase<PawnColumnDef>.AllDefsListForReading)
			{
				if (item.Worker is PawnColumnWorker_WorkPriority)
				{
					workTable.columns.Insert(workTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_CopyPasteWorkPriorities) + 1, item);
				}
			}
			return true;
		}

	}
}