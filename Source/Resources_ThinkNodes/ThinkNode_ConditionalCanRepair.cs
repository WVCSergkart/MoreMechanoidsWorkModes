// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;
using WVC;

namespace WVC_WorkModes
{

	public class ThinkNode_ConditionalCanRepair : ThinkNode_Conditional
	{

		protected override bool Satisfied(Pawn pawn)
		{
			if (MechRepairUtility.CanRepair(pawn))
			{
				return true;
			}
			return false;
		}

	}

}
