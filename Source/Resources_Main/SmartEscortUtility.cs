using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{

	public static class SmartEscortUtility
	{

		public static Pawn GetAssignedPawnOnMap(Pawn pawn)
		{
			if (WVC_MMWM.settings.enableSmartEscort)
			{
				if(!pawn.RaceProps.IsMechanoid)
				{
					return null;
				}
				CompSmartEscort comp = pawn.TryGetComp<CompSmartEscort>();
				if(comp != null && AssignedPawnAtHome(comp.escortTarget))
				{
					return comp.escortTarget;
				}
			}
			return pawn.GetOverseer();
		}

		public static bool AssignedPawnAtHome(Pawn pawn)
		{
			if (pawn != null && pawn.Map != null && pawn.Map.IsPlayerHome)
			{
				return true;
			}
			return false;
		}

		public static bool ShowAssignGizmo(Pawn overseer, Pawn mech, List<MechWorkModeDef> list)
		{
			// Log.Error("1");
			if (list.NullOrEmpty())
			{
				// Log.Error("1.1");
				return true;
			}
			// Log.Error("2");
			MechanitorControlGroup mechanitorControlGroup = overseer.mechanitor.GetControlGroup(mech);
			// Log.Error("3");
			if (mechanitorControlGroup != null && mechanitorControlGroup.WorkMode != null && list.Contains(mechanitorControlGroup.WorkMode))
			{
				// Log.Error("3.1");
				return true;
			}
			// Log.Error("4");
			return false;
		}

		public static List<MechWorkModeDef> SmartEscortModesList()
		{
			List<MechWorkModeDef> list = new();
			foreach (MechWorkModeDef item in DefDatabase<MechWorkModeDef>.AllDefsListForReading)
			{
				WorkModeExtension_SmartEscort modExtension = item?.GetModExtension<WorkModeExtension_SmartEscort>();
				if (modExtension != null && !modExtension.hideAssignGizmo)
				{
					list.Add(item);
				}
			}
			return list;
		}

	}

}
