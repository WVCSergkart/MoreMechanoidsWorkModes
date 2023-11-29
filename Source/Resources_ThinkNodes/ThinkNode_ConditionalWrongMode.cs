// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;
using WVC;

namespace WVC_WorkModes
{

	public class ThinkNode_ConditionalWrongMode : ThinkNode_Conditional
	{

		protected override bool Satisfied(Pawn pawn)
		{
			Pawn overseer = pawn.GetOverseer();
			if (overseer != null)
			{
				overseer.mechanitor.
				return true;
			}
			return false;
		}
	}

}
