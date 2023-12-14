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

		public static bool CanRecharge(Pawn pawn, out Need_MechEnergy energy)
		{
			energy = pawn?.needs?.energy;
			if (energy == null)
			{
				return false;
			}
			return true;
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

		// Zone Searching

		public static bool TryFindRandomMechShutdownZone(List<Zone> zones, Pawn pawn, Map map, MechanoidWorkType workModeType, out IntVec3 result)
		{
			for (int i = 0; i < zones.Count; i++)
			{
				if (zones[i] is Zone_MechanoidShutdown shutZone && MechanoidWorkTypeIsCorrect(shutZone, workModeType, pawn))
				{
					// foreach (IntVec3 cell in shutZone.Cells)
					// {
						// if (CanSelfShutdown(cell, pawn, map, false))
						// {
							// result = cell;
							// return true;
						// }
					// }
					// List<IntVec3> cells = shutZone.Cells;
					IntVec3 cell = shutZone.Cells.RandomElement();
					if (CanSelfShutdown(cell, pawn, map, false))
					{
						result = cell;
						return true;
					}
				}
			}
			// result = pawn.Position;
			return TryFindAnyMechShutdownZone(zones, pawn, map, workModeType, out result);
		}

		public static bool TryFindAnyMechShutdownZone(List<Zone> zones, Pawn pawn, Map map, MechanoidWorkType workModeType, out IntVec3 result)
		{
			for (int i = 0; i < zones.Count; i++)
			{
				if (zones[i] is Zone_MechanoidShutdown shutZone && MechanoidWorkTypeIsCorrect(shutZone, workModeType, pawn))
				{
					List<IntVec3> cells = shutZone.Cells;
					for (int j = 0; j < cells.Count; j++)
					{
						if (CanSelfShutdown(cells[j], pawn, map, false))
						{
							result = cells[j];
							return true;
						}
					}
				}
			}
			result = pawn.Position;
			return false;
		}

		// Zone Check

		public static bool MechanoidWorkTypeIsCorrect(Zone_MechanoidShutdown shutZone, MechanoidWorkType workModeType, Pawn mech)
		{
			if ((shutZone.owner == null || shutZone.ownerIndexGroup == 0) && !MechZoneRestricted(mech))
			{
				if ((shutZone.allowWorkers && workModeType == MechanoidWorkType.Work) || (shutZone.allowSafe && workModeType == MechanoidWorkType.Safe) || (shutZone.allowCombatants && workModeType == MechanoidWorkType.Combat) || (shutZone.allowAmbush && workModeType == MechanoidWorkType.Ambush))
				{
					return true;
				}
			}
			else
			{
				Pawn overseer = mech.GetOverseer();
				if (overseer == shutZone.owner && shutZone.ownerIndexGroup == overseer.mechanitor.GetControlGroup(mech).Index)
				{
					return true;
				}
			}
			return false;
		}

		public static bool MechZoneRestricted(Pawn mech)
		{
			CompMechSettings comp = mech.TryGetComp<CompMechSettings>();
			if(comp != null && comp.restrictZoneByGroup)
			{
				return true;
			}
			return false;
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
