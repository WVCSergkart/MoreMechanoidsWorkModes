using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;

namespace WVC
{
	public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{

		protected override bool Satisfied(Pawn pawn)
		{

			Pawn overseer = pawn.GetOverseer();
			if (overseer != null)
			{
				if (overseer.Drafted)
				{
					return true;
				}
			}
			return false;
		}
	}
	public class ThinkNode_ConditionalDowned : ThinkNode_Conditional
	{

		protected override bool Satisfied(Pawn pawn)
		{

			Pawn overseer = pawn.GetOverseer();
			if (overseer != null)
			{
				if (overseer.Downed)
				{
					return true;
				}
			}
			return false;
		}
	}
}
