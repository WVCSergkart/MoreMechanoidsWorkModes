// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;
using WVC;

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
