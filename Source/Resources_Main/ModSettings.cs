using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace WVC_WorkModes
{

	public class WVC_MMWM_Settings : ModSettings
	{

		public bool firstModLaunch = true;

		public bool WVC_FindAndDestroy = true;
		public bool WVC_WaitEnemy = true;
		public bool WVC_WorkAndWaitEnemy = true;
		public bool WVC_DefendYourself = true;
		public bool WVC_Ambush = true;

		public bool WVC_WorkAndRecharge = true;
		public bool WVC_SafeWorkAndRecharge = true;
		public bool WVC_EscaortAndRecharge = true;
		public bool WVC_WorkRechargeEscort = true;
		public bool WVC_EscortIfEnemyOnMap = true;

		public bool WVC_EscortIfEnemyWorkAndRecharge = false;
		public bool WVC_EscortIfDraftedOrDowned = false;
		public bool WVC_HiveMindResearching = false;
		public bool WVC_Scavenging = false;
		public int WVC_Scavenging_ReqCells = 120;

		// Features
		public bool enable_GoToShutdownZoneJob = true;
		public bool enableEnemySearching = true;
		public bool enableSmartEscort = true;
		public bool useCustomShutdownBehavior = true;
		public bool dormantMode = false;
		public int mechBandwithCostInDormantMode = 0;
		public bool enableAutoRepairByDefault = true;

		public IEnumerable<string> GetEnabledSettings => from specificSetting in GetType().GetFields()
			where specificSetting.FieldType == typeof(bool) && (bool)specificSetting.GetValue(this)
			select specificSetting.Name;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref firstModLaunch, "firstModLaunch", defaultValue: true, forceSave: true);
			// Main
			Scribe_Values.Look(ref WVC_FindAndDestroy, "WVC_FindAndDestroy", defaultValue: true, forceSave: true);
			Scribe_Values.Look(ref WVC_WaitEnemy, "WVC_WaitEnemy", defaultValue: true, forceSave: true);
			Scribe_Values.Look(ref WVC_WorkAndWaitEnemy, "WVC_WorkAndWaitEnemy", defaultValue: true, forceSave: true);
			Scribe_Values.Look(ref WVC_DefendYourself, "WVC_DefendYourself", defaultValue: true, forceSave: true);
			Scribe_Values.Look(ref WVC_Ambush, "WVC_Ambush", defaultValue: true, forceSave: true);

			Scribe_Values.Look(ref WVC_WorkAndRecharge, "WVC_WorkAndRecharge", defaultValue: true, forceSave: true);
			Scribe_Values.Look(ref WVC_SafeWorkAndRecharge, "WVC_SafeWorkAndRecharge", defaultValue: true, forceSave: true);
			Scribe_Values.Look(ref WVC_EscaortAndRecharge, "WVC_EscaortAndRecharge", defaultValue: true, forceSave: true);
			Scribe_Values.Look(ref WVC_WorkRechargeEscort, "WVC_WorkRechargeEscort", defaultValue: true, forceSave: true);
			Scribe_Values.Look(ref WVC_EscortIfEnemyOnMap, "WVC_EscortIfEnemyOnMap", defaultValue: true, forceSave: true);

			Scribe_Values.Look(ref WVC_EscortIfEnemyWorkAndRecharge, "WVC_EscortIfEnemyWorkAndRecharge", defaultValue: false, forceSave: true);
			Scribe_Values.Look(ref WVC_EscortIfDraftedOrDowned, "WVC_EscortIfDraftedOrDowned", defaultValue: false, forceSave: true);
			Scribe_Values.Look(ref WVC_HiveMindResearching, "WVC_HiveMindResearching", defaultValue: false, forceSave: true);
			Scribe_Values.Look(ref WVC_Scavenging, "WVC_Scavenging", defaultValue: false, forceSave: true);
			Scribe_Values.Look(ref WVC_Scavenging_ReqCells, "WVC_Scavenging_ReqCells", defaultValue: 120, forceSave: true);

			// Mechanics
			Scribe_Values.Look(ref enable_GoToShutdownZoneJob, "enableShutdownSearching", defaultValue: true);
			Scribe_Values.Look(ref enableEnemySearching, "enableEnemySearching", defaultValue: true);
			Scribe_Values.Look(ref enableSmartEscort, "enableSmartEscort", defaultValue: true);
			Scribe_Values.Look(ref useCustomShutdownBehavior, "useCustomShutdownBehavior", defaultValue: true);
			Scribe_Values.Look(ref dormantMode, "dormantMode", defaultValue: false);
			Scribe_Values.Look(ref enableAutoRepairByDefault, "enableAutoRepairByDefault", defaultValue: true);
		}
	}

	public class WVC_MMWM : Mod
	{
		public static WVC_MMWM_Settings settings;

		// private bool mainMode;

		private static Vector2 scrollPosition = Vector2.zero;

		public WVC_MMWM(ModContentPack content) : base(content)
		{
			settings = GetSettings<WVC_MMWM_Settings>();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			Rect outRect = new(inRect.x, inRect.y, inRect.width, inRect.height);
			Rect rect = new(0f, 0f, inRect.width - 30f, inRect.height * 2);
			Widgets.BeginScrollView(outRect, ref scrollPosition, rect);
			Listing_Standard listingStandard = new();
			listingStandard.Begin(rect);

			// Main
			listingStandard.Label("WVC_Label_MainModeLabelSetting".Translate() + ": ");
			listingStandard.CheckboxLabeled("WVC_Label_DefendYourself".Translate(), ref settings.WVC_DefendYourself, "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_Ambush".Translate(), ref settings.WVC_Ambush, "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_FindAndDestroy".Translate(), ref settings.WVC_FindAndDestroy, "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_WaitEnemy".Translate(), ref settings.WVC_WaitEnemy, "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_WorkAndWaitEnemy".Translate(), ref settings.WVC_WorkAndWaitEnemy, "WVC_ToolTip_Setting".Translate());

			listingStandard.CheckboxLabeled("WVC_Label_WorkAndRecharge".Translate(), ref settings.WVC_WorkAndRecharge, "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_SafeWorkAndRecharge".Translate(), ref settings.WVC_SafeWorkAndRecharge, "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_EscaortAndRecharge".Translate(), ref settings.WVC_EscaortAndRecharge, "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_WorkRechargeEscort".Translate(), ref settings.WVC_WorkRechargeEscort, "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_EscortIfEnemyOnMap".Translate(), ref settings.WVC_EscortIfEnemyOnMap, "WVC_ToolTip_Setting".Translate());
			// Bonus
			listingStandard.Gap();
			listingStandard.Label("WVC_Label_BonusModeLabelSetting".Translate() + ": ");
			listingStandard.CheckboxLabeled("WVC_Label_EscortIfEnemyWorkAndRecharge".Translate(), ref settings.WVC_EscortIfEnemyWorkAndRecharge, "WVC_ToolTip_BonusSetting".Translate() + "\n\n" + "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_EscortIfDraftedOrDowned".Translate(), ref settings.WVC_EscortIfDraftedOrDowned, "WVC_ToolTip_BonusSetting".Translate() + "\n\n" + "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_HiveMindResearching".Translate(), ref settings.WVC_HiveMindResearching, "WVC_ToolTip_BonusSetting".Translate() + "\n\n" + "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_Scavenging".Translate(), ref settings.WVC_Scavenging, "WVC_ToolTip_BonusSetting".Translate() + "\n\n" + "WVC_ToolTip_Setting".Translate());
			listingStandard.SliderLabeledWithRef("WVC_Label_ScavenZoneCells".Translate(settings.WVC_Scavenging_ReqCells.ToString()), ref settings.WVC_Scavenging_ReqCells, 20, 1000);
			// Mechanics
			listingStandard.Gap();
			listingStandard.Label("WVC_Label_WorkModeMechanicsLabelSetting".Translate() + ": ");
			listingStandard.CheckboxLabeled("WVC_Label_enableShutdownSearching".Translate(), ref settings.enable_GoToShutdownZoneJob, "WVC_ToolTip_enableShutdownSearching".Translate() + "\n\n" + "WVC_ToolTip_enableSearchingWarning".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_useCustomShutdownBehavior".Translate(), ref settings.useCustomShutdownBehavior, "WVC_ToolTip_useCustomShutdownBehavior".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_enableEnemySearching".Translate(), ref settings.enableEnemySearching, "WVC_ToolTip_enableEnemySearching".Translate() + "\n\n" + "WVC_ToolTip_enableSearchingWarning".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_enableSmartEscort".Translate(), ref settings.enableSmartEscort, "WVC_ToolTip_enableShutdownForAllMechanoids".Translate() + "\n\n" + "WVC_ToolTip_enableSmartEscort".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_enableDormantMode".Translate(), ref settings.dormantMode, "WVC_ToolTip_enableDormantMode".Translate());
			listingStandard.SliderLabeledWithRef("WVC_Label_mechBandwithCostInDormantMode".Translate(settings.mechBandwithCostInDormantMode.ToString()), ref settings.mechBandwithCostInDormantMode, 0, 100);
			listingStandard.CheckboxLabeled("WVC_Label_enableAutoRepairByDefault".Translate(), ref settings.enableAutoRepairByDefault, "WVC_ToolTip_enableAutoRepairByDefault".Translate());
			// =============== Buttons ===============
			listingStandard.GapLine();
			if (listingStandard.ButtonText("WVC_WorkModes_ResetButton".Translate()))
			{
				Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("WVC_WorkModes_ResetButtonWarning".Translate(), delegate
				{
					ResetSettings_Default();
					Messages.Message("WVC_WorkModes_ResetButton_SettingsChanged".Translate(), MessageTypeDefOf.TaskCompletion, historical: false);
				});
				Find.WindowStack.Add(window);
			}
			listingStandard.End();
			Widgets.EndScrollView();
			// base.DoSettingsWindowContents(inRect);
		}

		public override string SettingsCategory()
		{
			return "WVC - " + "WVC_MMWM_Settings".Translate();
		}

		public static void ResetSettings_Default()
		{
			// =
			WVC_MMWM.settings.WVC_FindAndDestroy = true;
			WVC_MMWM.settings.WVC_WaitEnemy = true;
			WVC_MMWM.settings.WVC_WorkAndWaitEnemy = true;
			WVC_MMWM.settings.WVC_DefendYourself = true;
			WVC_MMWM.settings.WVC_Ambush = true;
			// =
			WVC_MMWM.settings.WVC_WorkAndRecharge = true;
			WVC_MMWM.settings.WVC_SafeWorkAndRecharge = true;
			WVC_MMWM.settings.WVC_EscaortAndRecharge = true;
			WVC_MMWM.settings.WVC_WorkRechargeEscort = true;
			WVC_MMWM.settings.WVC_EscortIfEnemyOnMap = true;
			// =
			WVC_MMWM.settings.WVC_EscortIfEnemyWorkAndRecharge = false;
			WVC_MMWM.settings.WVC_EscortIfDraftedOrDowned = false;
			WVC_MMWM.settings.WVC_HiveMindResearching = false;
			WVC_MMWM.settings.WVC_Scavenging = false;
			WVC_MMWM.settings.WVC_Scavenging_ReqCells = 120;
			// Features
			WVC_MMWM.settings.enable_GoToShutdownZoneJob = true;
			WVC_MMWM.settings.enableEnemySearching = true;
			WVC_MMWM.settings.enableSmartEscort = true;
			WVC_MMWM.settings.useCustomShutdownBehavior = true;
			WVC_MMWM.settings.dormantMode = false;
			WVC_MMWM.settings.mechBandwithCostInDormantMode = 0;
			WVC_MMWM.settings.enableAutoRepairByDefault = true;
			// Initial
			WVC_MMWM.settings.firstModLaunch = false;
			WVC_MMWM.settings.Write();
		}

	}
}
