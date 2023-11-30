// RimWorld.JobGiver_GetEnergy_SelfShutdown
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{

    public class ThinkNode_ConditionalAnyScavengeZone : ThinkNode_Conditional
	{

		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.Map.IsPlayerHome)
			{
				return false;
			}
			List<Zone> mapZones = pawn.Map.zoneManager.AllZones;
			if (mapZones.NullOrEmpty() || !ScavengeUtility.AnyScavengeZone(mapZones))
			{
				return false;
			}
			return true;
		}
	}

}
