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
				//if(!pawn.RaceProps.IsMechanoid)
				//{
				//	return null;
				//}
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
			if (pawn?.Map?.IsPlayerHome == true)
			{
				return true;
			}
			return false;
		}

		public static bool ShowAssignGizmo(Pawn overseer, Pawn mech, List<MechWorkModeDef> list)
		{
			if (list.NullOrEmpty())
			{
				return false;
			}
			MechanitorControlGroup mechanitorControlGroup = overseer?.mechanitor?.GetControlGroup(mech);
			if (mechanitorControlGroup != null && mechanitorControlGroup.WorkMode != null && list.Contains(mechanitorControlGroup.WorkMode))
			{
				return true;
			}
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
