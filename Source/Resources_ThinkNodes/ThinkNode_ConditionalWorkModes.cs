// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
    public class ThinkNode_ConditionalWorkModes : ThinkNode_Conditional
	{

		public List<MechWorkModeDef> workModes;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalWorkModes obj = (ThinkNode_ConditionalWorkModes)base.DeepCopy(resolve);
			obj.workModes = workModes;
			return obj;
		}

		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.RaceProps.IsMechanoid || pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			Pawn overseer = pawn.GetOverseer();
			if (overseer == null)
			{
				return false;
			}
			return workModes.Contains(overseer.mechanitor.GetControlGroup(pawn).WorkMode);
		}

	}
}
