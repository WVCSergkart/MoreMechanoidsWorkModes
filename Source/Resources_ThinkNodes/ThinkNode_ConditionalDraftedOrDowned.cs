using RimWorld;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
    public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{

		protected override bool Satisfied(Pawn pawn)
		{

			Pawn overseer = ShutdownUtility.GetAssignedPawnOnMap(pawn);
			if (overseer != null)
			{
				if (overseer.Drafted && pawn.CanReach(overseer, PathEndMode.OnCell, Danger.Deadly))
				{
					return true;
				}
				if (overseer.mindState.lastJobTag == JobTag.Fieldwork && pawn.CanReach(overseer, PathEndMode.OnCell, Danger.Deadly))
				{
					return true;
				}
				// else
				// {
					// Pawn carriedBy = overseer.CarriedBy;
					// if (carriedBy != null && carriedBy.HostileTo(overseer) && pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly))
					// {
						// return true;
					// }
				// }
			}
			return false;
		}
	}
	public class ThinkNode_ConditionalDowned : ThinkNode_Conditional
	{

		protected override bool Satisfied(Pawn pawn)
		{

			Pawn overseer = ShutdownUtility.GetAssignedPawnOnMap(pawn);
			if (overseer != null)
			{
				if (overseer.Downed && pawn.CanReach(overseer, PathEndMode.OnCell, Danger.Deadly))
				{
					return true;
				}
				else
				{
					Pawn carriedBy = overseer.CarriedBy;
					if (carriedBy != null && carriedBy.HostileTo(overseer) && pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
