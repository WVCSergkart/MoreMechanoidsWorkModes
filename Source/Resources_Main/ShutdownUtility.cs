using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{

	public enum MechanoidWorkType : byte
	{
		Work,
		Safe,
		Combat,
		Ambush
	}

	public static class ShutdownUtility
	{

		public static List<RoomRoleDef> ConvertToDefs(this List<string> strings)
		{
			List<RoomRoleDef> list = new();
			List<RoomRoleDef> dataBase = DefDatabase<RoomRoleDef>.AllDefsListForReading;
			foreach (RoomRoleDef room in dataBase)
            {
				if (strings.Contains(room.defName))
                {
					list.Add(room);
				}
            }
			return list;
		}

		public static bool CanRecharge(Pawn pawn, out Need_MechEnergy energy)
		{
			// energy = pawn?.needs?.energy;
			// if (energy == null)
			// {
				// return false;
			// }
			// return true;
			return (energy = pawn?.needs?.energy) != null;
		}

		public static bool InShutdownMode(this Pawn mech)
		{
			// if (CanRecharge(mech, out Need_MechEnergy energy) && energy.IsSelfShutdown)
			// {
				// return true;
			// }
			return CanRecharge(mech, out Need_MechEnergy energy) && energy.IsSelfShutdown;
		}

		public static Building GetClosestShutdownSpot(Pawn mech, ThingDef spotDefName, int maxDistance)
		{
			return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(spotDefName), PathEndMode.OnCell, TraverseParms.For(mech), maxDistance, delegate (Thing t)
			{
				return !t.IsForbidden(mech);
			});
		}

		public static bool AnyMechanoidZone(List<Zone> zones)
		{
			for (int i = 0; i < zones.Count; i++)
			{
				if (zones[i] is Zone_MechanoidShutdown)
				{
					return true;
				}
			}
			return false;
		}

		// Pawn Zone Check

		public static bool MechInShutdownZone(Pawn pawn, IntVec3 cell, MechanoidWorkType workModeType)
		{
			Zone zone = GridsUtility.GetZone(cell, pawn.Map);
			if (zone is Zone_MechanoidShutdown shutZone)
			{
				if (MechanoidWorkTypeIsCorrect(shutZone, workModeType, pawn))
				{
					return true;
				}
			}
			return false;
		}

		public static bool MechInShutdownZone(Pawn pawn, IntVec3 cell, List<RoomRoleDef> possibleRooms)
		{
			Room room = GridsUtility.GetRoom(cell, pawn.Map);
			if (possibleRooms.Contains(room.Role))
			{
				return true;
			}
			return false;
		}

		// Zone Searching

		public static bool TryFindRandomMechShutdownZone(List<Zone> zones, Pawn pawn, Map map, MechanoidWorkType workModeType, out IntVec3 result)
		{
			zones.Shuffle();
			for (int i = 0; i < zones.Count; i++)
			{
				if (zones[i] is Zone_MechanoidShutdown shutZone && MechanoidWorkTypeIsCorrect(shutZone, workModeType, pawn))
				{
					//List<IntVec3> cells = shutZone.Cells;
					//cells.Shuffle();
					//foreach (IntVec3 cell in cells)
					//{
					//	if (CanSelfShutdown(cell, pawn, map, false))
					//	{
					//		result = cell;
					//	}
					//}
					if (TryFindRandomMechShutdownZone(shutZone.Cells, pawn, map, out result))
					{
						return true;
					}
				}
			}
			// return TryFindAnyMechShutdownZone(zones, pawn, map, workModeType, out result);
			result = pawn.Position;
			return false;
		}

		public static bool TryFindRandomMechShutdownZone(List<IntVec3> targetCells, Pawn pawn, Map map, out IntVec3 result)
		{
			List<IntVec3> cells = targetCells;
			cells.Shuffle();
			foreach (IntVec3 cell in cells)
			{
				if (CanSelfShutdown(cell, pawn, map, false))
				{
					result = cell;
					return true;
				}
			}
			result = pawn.Position;
			return false;
		}

		// public static bool TryFindAnyMechShutdownZone(List<Zone> zones, Pawn pawn, Map map, MechanoidWorkType workModeType, out IntVec3 result)
		// {
		// for (int i = 0; i < zones.Count; i++)
		// {
		// if (zones[i] is Zone_MechanoidShutdown shutZone && MechanoidWorkTypeIsCorrect(shutZone, workModeType, pawn))
		// {
		// List<IntVec3> cells = shutZone.Cells;
		// for (int j = 0; j < cells.Count; j++)
		// {
		// if (CanSelfShutdown(cells[j], pawn, map, false))
		// {
		// result = cells[j];
		// return true;
		// }
		// }
		// }
		// }
		// result = pawn.Position;
		// return false;
		// }

		// Zone Check

		public static bool MechanoidWorkTypeIsCorrect(Zone_MechanoidShutdown shutZone, MechanoidWorkType workModeType, Pawn mech)
		{
			if ((shutZone.owner?.mechanitor == null || !shutZone.ownerIndexGroup.HasValue) && !MechZoneRestricted(mech))
			{
				return shutZone.MechanoidAllowed(workModeType);
			}
			else
			{
				Pawn overseer = mech.GetOverseer();
				if (overseer == shutZone.owner && shutZone.ownerIndexGroup.Value == overseer.mechanitor.GetControlGroup(mech).Index)
				{
					return true;
				}
			}
			return false;
		}

		public static bool MechZoneRestricted(Pawn mech)
		{
			return mech?.TryGetComp<CompMechSettings>()?.restrictZoneByGroup == true;
		}

		// Shutdown Check

		public static bool TryFindNearbyMechSelfShutdownSpot(IntVec3 root, Pawn pawn, Map map, out IntVec3 result, bool allowForbidden = false)
		{
			foreach (IntVec3 item in GenRadial.RadialCellsAround(root, GenRadial.MaxRadialPatternRadius - 1f, useCenter: true))
			{
				if (CanSelfShutdown(item, pawn, map, allowForbidden))
				{
					result = item;
					return true;
				}
			}
			return RCellFinder.TryFindRandomMechSelfShutdownSpot(root, pawn, map, out result, allowForbidden);
		}

		public static bool CanSelfShutdown(IntVec3 c, Pawn pawn, Map map, bool allowForbidden = false)
		{
			if (!c.Standable(map))
			{
				return false;
			}
			if (!pawn.CanReach(c, PathEndMode.OnCell, Danger.Some))
			{
				return false;
			}
			if (!pawn.CanReserve(c))
			{
				return false;
			}
			if (!allowForbidden && c.IsForbidden(pawn))
			{
				return false;
			}
			if (!WVC_MMWM.settings.useCustomShutdownBehavior)
			{
				// Stupidity Culprit
				if (c.GetFirstBuilding(map) != null)
				{
					return false;
				}
			}
			Room room = c.GetRoom(map);
			if (room != null && room.IsPrisonCell)
			{
				return false;
			}
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				List<Thing> thingList = (c + GenAdj.CardinalDirections[i]).GetThingList(map);
				for (int j = 0; j < thingList.Count; j++)
				{
					if (thingList[j].def.hasInteractionCell && thingList[j].InteractionCell == c)
					{
						return false;
					}
				}
			}
			return true;
		}

	}

}
