// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;
using WVC;

namespace WVC_WorkModes
{

	public class ThinkNode_ConditionalStatValue : ThinkNode_Conditional
	{

		public float requiredStatValue = 1f;

		public StatDef statDef;

		protected override bool Satisfied(Pawn pawn)
		{
			if (statDef != null)
			{
				float mechStatValue = pawn.GetStatValue(statDef);
				if (requiredStatValue <= mechStatValue)
				{
					return true;
				}
			}
			return false;
		}

	}

}
