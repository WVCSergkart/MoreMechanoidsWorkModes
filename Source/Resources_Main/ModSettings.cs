using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace WVC
{
    public class WVC_MMWM_Settings : ModSettings
	{
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

		// Bonus
		public bool WVC_EscortIfEnemyWorkAndRecharge = false;
		public bool WVC_EscortIfDraftedOrDowned = false;
		public bool WVC_HiveMindResearching = false;

		// Features
		public bool enableShutdownSearching = true;
		public bool enableEnemySearching = true;
		public bool enableSmartEscort = true;

		// Mechanoid Idle Optimization
		public bool enableSmartChargingForAllMechanoids = false;
		public bool enableShutdownForAllMechanoids = false;

		// Zones by default
		public bool shutdownModeZonesOrSpots = true;
		public bool useCustomShutdownBehavior = true;

		// public bool WVC_CustomWorkMode = false;
			// public bool customMode_ShouldFight = false;
			// public bool customMode_EnemyOnMap_ShouldFollowOverseer = false;
			// public bool customMode_EnemyOnMap_ShouldGotoNearestHostile = false;
			// public bool customMode_MechanoidShouldWork = false;
			// public bool customMode_MechanoidShouldSmartCharging = false;
			// public bool customMode_MechanoidShouldSearchingSpot = false;
			// public bool customMode_MechanoidShouldShutdown = false;

		public IEnumerable<string> GetEnabledSettings => from specificSetting in GetType().GetFields()
			where specificSetting.FieldType == typeof(bool) && (bool)specificSetting.GetValue(this)
			select specificSetting.Name;

		public override void ExposeData()
		{
			Scribe_Values.Look(ref WVC_FindAndDestroy, "WVC_FindAndDestroy", defaultValue: true);
			Scribe_Values.Look(ref WVC_WaitEnemy, "WVC_WaitEnemy", defaultValue: true);
			Scribe_Values.Look(ref WVC_WorkAndWaitEnemy, "WVC_WorkAndWaitEnemy", defaultValue: true);
			Scribe_Values.Look(ref WVC_DefendYourself, "WVC_DefendYourself", defaultValue: true);
			Scribe_Values.Look(ref WVC_Ambush, "WVC_Ambush", defaultValue: true);

			Scribe_Values.Look(ref WVC_WorkAndRecharge, "WVC_WorkAndRecharge", defaultValue: true);
			Scribe_Values.Look(ref WVC_SafeWorkAndRecharge, "WVC_SafeWorkAndRecharge", defaultValue: true);
			Scribe_Values.Look(ref WVC_EscaortAndRecharge, "WVC_EscaortAndRecharge", defaultValue: true);
			Scribe_Values.Look(ref WVC_WorkRechargeEscort, "WVC_WorkRechargeEscort", defaultValue: true);
			Scribe_Values.Look(ref WVC_EscortIfEnemyOnMap, "WVC_EscortIfEnemyOnMap", defaultValue: true);

			Scribe_Values.Look(ref WVC_EscortIfEnemyWorkAndRecharge, "WVC_EscortIfEnemyWorkAndRecharge", defaultValue: false);
			Scribe_Values.Look(ref WVC_EscortIfDraftedOrDowned, "WVC_EscortIfDraftedOrDowned", defaultValue: false);
			Scribe_Values.Look(ref WVC_HiveMindResearching, "WVC_HiveMindResearching", defaultValue: false);

			Scribe_Values.Look(ref enableShutdownSearching, "enableShutdownSearching", defaultValue: true);
			Scribe_Values.Look(ref enableEnemySearching, "enableEnemySearching", defaultValue: true);
			Scribe_Values.Look(ref enableSmartEscort, "enableSmartEscort", defaultValue: true);

			Scribe_Values.Look(ref enableSmartChargingForAllMechanoids, "enableSmartChargingForAllMechanoids", defaultValue: false);
			Scribe_Values.Look(ref enableShutdownForAllMechanoids, "enableShutdownForAllMechanoids", defaultValue: false);

			Scribe_Values.Look(ref shutdownModeZonesOrSpots, "shutdownModeZonesOrSpots", defaultValue: true);
			Scribe_Values.Look(ref useCustomShutdownBehavior, "useCustomShutdownBehavior", defaultValue: true);

			// Scribe_Values.Look(ref WVC_CustomWorkMode, "WVC_CustomWorkMode", defaultValue: false, true);
				// Scribe_Values.Look(ref customMode_ShouldFight, "customMode_ShouldFight", defaultValue: false, true);
				// Scribe_Values.Look(ref customMode_EnemyOnMap_ShouldFollowOverseer, "customMode_EnemyOnMap_ShouldFollowOverseer", defaultValue: false, true);
				// Scribe_Values.Look(ref customMode_EnemyOnMap_ShouldGotoNearestHostile, "customMode_EnemyOnMap_ShouldGotoNearestHostile", defaultValue: false, true);
				// Scribe_Values.Look(ref customMode_MechanoidShouldWork, "customMode_MechanoidShouldWork", defaultValue: false, true);
				// Scribe_Values.Look(ref customMode_MechanoidShouldSmartCharging, "customMode_MechanoidShouldSmartCharging", defaultValue: false, true);
				// Scribe_Values.Look(ref customMode_MechanoidShouldSearchingSpot, "customMode_MechanoidShouldSearchingSpot", defaultValue: false, true);
				// Scribe_Values.Look(ref customMode_MechanoidShouldShutdown, "customMode_MechanoidShouldShutdown", defaultValue: false, true);
			base.ExposeData();
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
			// Rect rect = new(0f, 0f, inRect.width, inRect.height);
			Rect rect = new(0f, 0f, inRect.width - 30f, inRect.height * 2);
			// var labelRect = new Rect(rect.x + 5, rect.y, 60, 24);
			// var resetRect = new Rect(labelRect.x, labelRect.yMax + 5, 265, 24f);
			Widgets.BeginScrollView(outRect, ref scrollPosition, rect);
			Listing_Standard listingStandard = new();
			listingStandard.Begin(rect);
			// CheckboxTemplate(listingStandard, "WVC_Label_DefendYourself", settings.WVC_DefendYourself);
			// CheckboxTemplate(listingStandard, "WVC_Label_Ambush", settings.WVC_Ambush);
			// CheckboxTemplate(listingStandard, "WVC_Label_FindAndDestroy", settings.WVC_FindAndDestroy);
			// CheckboxTemplate(listingStandard, "WVC_Label_WaitEnemy", settings.WVC_WaitEnemy);
			// CheckboxTemplate(listingStandard, "WVC_Label_WorkAndWaitEnemy", settings.WVC_WorkAndWaitEnemy);
			// CheckboxTemplate(listingStandard, "WVC_Label_WorkAndRecharge", settings.WVC_WorkAndRecharge);
			// CheckboxTemplate(listingStandard, "WVC_Label_SafeWorkAndRecharge", settings.WVC_SafeWorkAndRecharge);
			// CheckboxTemplate(listingStandard, "WVC_Label_EscaortAndRecharge", settings.WVC_EscaortAndRecharge);
			// CheckboxTemplate(listingStandard, "WVC_Label_WorkRechargeEscort", settings.WVC_WorkRechargeEscort);
			// CheckboxTemplate(listingStandard, "WVC_Label_EscortIfEnemyOnMap", settings.WVC_EscortIfEnemyOnMap);
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

			listingStandard.Gap();
			listingStandard.Label("WVC_Label_BonusModeLabelSetting".Translate() + ": ");
			listingStandard.CheckboxLabeled("WVC_Label_EscortIfEnemyWorkAndRecharge".Translate(), ref settings.WVC_EscortIfEnemyWorkAndRecharge, "WVC_ToolTip_BonusSetting".Translate() + "\n\n" + "WVC_ToolTip_Setting".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_EscortIfDraftedOrDowned".Translate(), ref settings.WVC_EscortIfDraftedOrDowned, "WVC_ToolTip_BonusSetting".Translate() + "\n\n" + "WVC_ToolTip_Setting".Translate());
			// CheckboxTemplate(listingStandard, "WVC_Label_EscortIfEnemyWorkAndRecharge", settings.WVC_EscortIfEnemyWorkAndRecharge, false);
			listingStandard.CheckboxLabeled("WVC_Label_HiveMindResearching".Translate(), ref settings.WVC_HiveMindResearching, "WVC_ToolTip_BonusSetting".Translate() + "\n\n" + "WVC_ToolTip_Setting".Translate());

			listingStandard.Gap();
			listingStandard.CheckboxLabeled("WVC_Label_enableSmartChargingForAllMechanoids".Translate(), ref settings.enableSmartChargingForAllMechanoids, "WVC_ToolTip_enableSmartChargingForAllMechanoids".Translate() + "\n\n" + "WVC_ToolTip_VanillaModeOptimizationWarning".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_enableShutdownForAllMechanoids".Translate(), ref settings.enableShutdownForAllMechanoids, "WVC_ToolTip_enableShutdownForAllMechanoids".Translate() + "\n\n" + "WVC_ToolTip_VanillaModeOptimizationWarning".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_enableSmartEscort".Translate(), ref settings.enableSmartEscort, "WVC_ToolTip_enableShutdownForAllMechanoids".Translate() + "\n\n" + "WVC_ToolTip_enableSmartEscort".Translate());

			listingStandard.Gap();
			listingStandard.CheckboxLabeled("WVC_Label_enableShutdownSearching".Translate(), ref settings.enableShutdownSearching, "WVC_ToolTip_enableShutdownSearching".Translate() + "\n\n" + "WVC_ToolTip_enableSearchingWarning".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_shutdownModeZonesOrSpots".Translate(), ref settings.shutdownModeZonesOrSpots, "WVC_ToolTip_shutdownModeZonesOrSpots".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_useCustomShutdownBehavior".Translate(), ref settings.useCustomShutdownBehavior, "WVC_ToolTip_useCustomShutdownBehavior".Translate());
			listingStandard.CheckboxLabeled("WVC_Label_enableEnemySearching".Translate(), ref settings.enableEnemySearching, "WVC_ToolTip_enableEnemySearching".Translate() + "\n\n" + "WVC_ToolTip_enableSearchingWarning".Translate());

			// listingStandard.Label("");
			// listingStandard.Label("WVC_Label_CustomModeLabelSetting".Translate() + ": ");
			// listingStandard.CheckboxLabeled("WVC_Label_CustomWorkMode".Translate(), ref settings.WVC_CustomWorkMode, "WVC_ToolTip_CustomWorkMode".Translate() + "\n\n" + "WVC_ToolTip_Setting".Translate());
				// listingStandard.CheckboxLabeled("WVC_Label_CustomWorkMode_ShouldFight".Translate(), ref settings.customMode_ShouldFight, "WVC_ToolTip_CustomWorkMode_ShouldFight".Translate());
				// listingStandard.CheckboxLabeled("WVC_Label_CustomWorkMode_ShouldEscort".Translate(), ref settings.customMode_EnemyOnMap_ShouldFollowOverseer, "WVC_ToolTip_CustomWorkMode_ShouldEscort".Translate());
				// listingStandard.CheckboxLabeled("WVC_Label_CustomWorkMode_ShouldGoToHostile".Translate(), ref settings.customMode_EnemyOnMap_ShouldGotoNearestHostile, "WVC_ToolTip_CustomWorkMode_ShouldGoToHostile".Translate());
				// listingStandard.CheckboxLabeled("WVC_Label_CustomWorkMode_ShouldWork".Translate(), ref settings.customMode_MechanoidShouldWork, "WVC_ToolTip_CustomWorkMode_ShouldWork".Translate());
				// listingStandard.CheckboxLabeled("WVC_Label_CustomWorkMode_ShouldSmartCharging".Translate(), ref settings.customMode_MechanoidShouldSmartCharging, "WVC_ToolTip_CustomWorkMode_ShouldSmartCharging".Translate());
				// listingStandard.CheckboxLabeled("WVC_Label_CustomWorkMode_ShouldSearchingSpot".Translate(), ref settings.customMode_MechanoidShouldSearchingSpot, "WVC_ToolTip_CustomWorkMode_ShouldSearchingSpot".Translate());
				// listingStandard.CheckboxLabeled("WVC_Label_CustomWorkMode_ShouldShouldShutdown".Translate(), ref settings.customMode_MechanoidShouldShutdown, "WVC_ToolTip_CustomWorkMode_ShouldShouldShutdown".Translate());

			listingStandard.End();
			Widgets.EndScrollView();
			// base.DoSettingsWindowContents(inRect);
		}

		public override string SettingsCategory()
		{
			return "WVC - " + "WVC_MMWM_Settings".Translate();
		}

		// private void CheckboxTemplate(Listing_Standard listingStandard, string labelName, bool settingName, bool mainMode = true)
		// {
			// if (mainMode == true)
			// {
				// listingStandard.CheckboxLabeled(labelName.Translate(), ref settingName, "WVC_ToolTip_Setting".Translate());
			// }
			// else
			// {
				// listingStandard.CheckboxLabeled("WVC_Label_BonusModeLabelSetting".Translate() + ": " + labelName.Translate(), ref settingName, "WVC_ToolTip_Setting".Translate());
			// }
		// }
	}
}
