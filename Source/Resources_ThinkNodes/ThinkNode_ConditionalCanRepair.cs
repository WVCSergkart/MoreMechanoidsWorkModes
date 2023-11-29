// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;

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
