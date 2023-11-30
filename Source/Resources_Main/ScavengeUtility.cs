using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using WVC;

namespace WVC_WorkModes
{

	public static class ScavengeUtility
	{

		public static IntVec3 GetScavengeWanderRoot(Pawn pawn)
		{
			Zone zone = GridsUtility.GetZone(pawn.Position, pawn.Map);
			return zone.Cells.RandomElement();
		}

		public static bool AnyScavengeZone(List<Zone> zones)
		{
			for (int i = 0; i < zones.Count; i++)
			{
                if (zones[i] is Zone_MechanoidScavenge scavZone)
                {
                    if (scavZone.ZoneIsActive())
                    {
                        return true;
                    }
                }
            }
			return false;
		}

		public static bool TryFindFirstMechScavengeZone(List<Zone> zones, Pawn pawn, out IntVec3 result)
		{
			for (int i = 0; i < zones.Count; i++)
			{
				if (zones[i] is Zone_MechanoidScavenge zone)
				{
					IntVec3 cell = zone.Cells.RandomElement();
					if (CanReach(cell, pawn, false))
					{
						result = cell;
						return true;
					}
				}
			}
			result = pawn.Position;
			return false;
		}

		public static bool MechInScavengeZone(Pawn pawn, IntVec3 cell)
		{
			Zone zone = GridsUtility.GetZone(cell, pawn.Map);
			if (zone is Zone_MechanoidScavenge)
			{
				return true;
			}
			return false;
		}

		public static bool CanReach(IntVec3 c, Pawn pawn, bool allowForbidden = false)
		{
			if (!pawn.CanReach(c, PathEndMode.OnCell, Danger.Some))
			{
				return false;
			}
			if (!allowForbidden && c.IsForbidden(pawn))
			{
				return false;
			}
			return true;
		}

	}

}
