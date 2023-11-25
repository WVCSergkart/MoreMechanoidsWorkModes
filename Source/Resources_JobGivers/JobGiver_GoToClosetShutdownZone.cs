// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using WVC;
using System.Linq;

namespace WVC_WorkModes
{
	public class JobGiver_GoToShutdownZone : ThinkNode_JobGiver
	{

		public MechanoidWorkType workModeType = 0;

		public enum MechanoidWorkType : byte
		{
			Work,
			Safe,
			Combat,
			Ambush
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (WVC_MMWM.settings.enableShutdownSearching)
			{
				if (!pawn.Map.IsPlayerHome)
				{
					return null;
				}
				List<Zone> mapZones = pawn.Map.zoneManager.AllZones;
				if (mapZones.NullOrEmpty() || !AnyMechanoidZone(mapZones))
				{
					return null;
				}
				if (TryFindNearbyMechShutdownZone(mapZones, pawn, pawn.Map, workModeType, out var result))
				{
					if (pawn.Position == result)
					{
						return null;
					}
					Job job = JobMaker.MakeJob(JobDefOf.Goto, result);
					return job;
				}
				// if (TryFindNearbyMechSelfShutdownZone(pawn.Position, pawn, pawn.Map, workModeType, out var result))
				// {
					// if (pawn.Position == result)
					// {
						// return null;
					// }
					// Job job = JobMaker.MakeJob(JobDefOf.Goto, result);
					// return job;
				// }
			}
			return null;
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

		public static bool TryFindNearbyMechShutdownZone(List<Zone> zones, Pawn pawn, Map map, MechanoidWorkType workModeType, out IntVec3 result)
		{
			// foreach (Zone zone in zones)
			// {
				// foreach (IntVec3 cell in zone.Cells)
				// {
					// if (CanSelfShutdown(cell, zone, pawn, map, workModeType, false))
					// {
						// result = cell;
						// return true;
					// }
				// }
			// }
			for (int i = 0; i < zones.Count; i++)
			{
				List<IntVec3> cells = zones[i].Cells;
				for (int j = 0; j < cells.Count; j++)
				{
					if (CanSelfShutdown(cells[j], zones[i], pawn, map, workModeType, false))
					{
						result = cells[j];
						return true;
					}
				}
			}
			result = pawn.Position;
			return false;
		}


		// public static bool TryFindNearbyMechSelfShutdownZone(IntVec3 root, Pawn pawn, Map map, MechanoidWorkType workModeType, out IntVec3 result)
		// {
			// foreach (IntVec3 item in GenRadial.RadialCellsAround(root, GenRadial.MaxRadialPatternRadius - 1f, useCenter: true))
			// {
				// if (CanSelfShutdown(item, pawn, map, workModeType, false))
				// {
					// result = item;
					// return true;
				// }
			// }
			// result = root;
			// return false;
		// }

		public static bool CanSelfShutdown(IntVec3 c, Zone zone, Pawn pawn, Map map, MechanoidWorkType workModeType, bool allowForbidden = false)
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
			// Zone zone = GridsUtility.GetZone(c, map);
			if (zone is Zone_MechanoidShutdown shutZone)
			{
				if ((shutZone.allowWorkers && workModeType == MechanoidWorkType.Work) || (shutZone.allowSafe && workModeType == MechanoidWorkType.Safe) || (shutZone.allowCombatants && workModeType == MechanoidWorkType.Combat) || (shutZone.allowAmbush && workModeType == MechanoidWorkType.Ambush))
				{
					if (c.GetFirstBuilding(map) != null)
					{
						return false;
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
			return false;
		}
	}

	// public static bool InAllowedArea(this IntVec3 c)
	// {
		// Area effectiveAreaRestrictionInPawnCurrentMap = forPawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
		// if (effectiveAreaRestrictionInPawnCurrentMap != null && effectiveAreaRestrictionInPawnCurrentMap.TrueCount > 0 && !effectiveAreaRestrictionInPawnCurrentMap[c])
		// {
			// return false;
		// }
		// return true;
	// }
}
