using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{

	public static class MechGroupUtility
	{

		public static List<Pawn> GetAllMechanitors(Map map)
		{
			List<Pawn> list = new();
			List<Pawn> colonists = map.mapPawns.FreeColonists;
			if (!colonists.NullOrEmpty())
			{
				foreach (Pawn colonist in colonists)
				{
					if (MechanitorUtility.IsMechanitor(colonist))
					{
						list.Add(colonist);
					}
				}
			}
			return list;
		}

		[Obsolete]
		public static List<MechWorkModeDef> GetAllShutdownModes(Pawn mech)
		{
			List<MechWorkModeDef> list = new();
			foreach (ThinkNode thinkNode in mech.RaceProps.thinkTreeMain.thinkRoot.ChildrenRecursive.ToList())
			{
				if (thinkNode is ThinkNode_ConditionalWorkMode thinkWorkMode)
				{
					MechWorkModeDef mechWorkModeDef = thinkWorkMode.workMode;
					foreach (ThinkNode thinkNode2 in thinkWorkMode.ChildrenRecursive.ToList())
					{
						if (thinkNode2 is JobGiver_GoToShutdownZone)
						{
							list.Add(mechWorkModeDef);
						}
					}
				}
			}
			return list;
		}

		public static string GetGroupIconPath(Pawn overseer, int? groupIndex)
		{
			if (groupIndex.HasValue && TryGetGroupFromIndex(overseer, groupIndex, out MechanitorControlGroup localGroup))
			{
				return localGroup?.WorkMode?.iconPath;
			}
			return "WVC/UI/WorkModes_General/UI_MechGroup";
		}

		[Obsolete]
		public static MechWorkModeDef GetGroupWorkMode(Pawn overseer, int groupIndex)
		{
			if (TryGetGroupFromIndex(overseer, groupIndex, out MechanitorControlGroup localGroup))
			{
				return localGroup?.WorkMode;
			}
			return null;
		}

		public static bool TryGetGroupFromIndex(Pawn overseer, int? groupIndex, out MechanitorControlGroup mechGroup)
		{
			mechGroup = GetGroupFromIndex(overseer, groupIndex);
			if (mechGroup != null)
			{
				return true;
			}
			return false;
		}

		public static MechanitorControlGroup GetGroupFromIndex(Pawn overseer, int? groupIndex)
		{
			if (overseer?.mechanitor != null && groupIndex.HasValue)
			{
				List<MechanitorControlGroup> groups = overseer?.mechanitor?.controlGroups;
				if (groups.NullOrEmpty())
				{
					return null;
				}
				for (int i = 0; i < groups.Count; i++)
				{
					MechanitorControlGroup localGroup = groups[i];
					if (groupIndex.Value == localGroup.Index)
					{
						return localGroup;
					}
				}
			}
			return null;
		}

	}

}
