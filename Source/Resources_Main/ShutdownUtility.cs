using System.Collections.Generic;
using Verse;
using RimWorld;
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

		public static bool TryFindNearbyMechShutdownZone(List<Zone> zones, Pawn pawn, Map map, MechanoidWorkType workModeType, out IntVec3 result)
		{
			for (int i = 0; i < zones.Count; i++)
			{
				if (zones[i] is Zone_MechanoidShutdown shutZone && MechanoidWorkTypeIsCorrect(shutZone, workModeType))
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

		public static bool MechanoidWorkTypeIsCorrect(Zone_MechanoidShutdown shutZone, MechanoidWorkType workModeType)
		{
			if ((shutZone.allowWorkers && workModeType == MechanoidWorkType.Work) || (shutZone.allowSafe && workModeType == MechanoidWorkType.Safe) || (shutZone.allowCombatants && workModeType == MechanoidWorkType.Combat) || (shutZone.allowAmbush && workModeType == MechanoidWorkType.Ambush))
			{
				return true;
			}
			return false;
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

}
