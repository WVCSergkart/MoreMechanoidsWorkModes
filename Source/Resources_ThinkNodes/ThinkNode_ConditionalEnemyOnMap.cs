// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
    public class ThinkNode_ConditionalEnemyOnMap : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn pawn)
		{
			if (WVC_MMWM.settings.enableEnemySearching == true)
			{
				float num = float.MaxValue;
				Thing thing = null;
				List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
				for (int i = 0; i < potentialTargetsFor.Count; i++)
				{
					IAttackTarget attackTarget = potentialTargetsFor[i];
					if (!attackTarget.ThreatDisabled(pawn) && AttackTargetFinder.IsAutoTargetable(attackTarget) && (attackTarget.Thing is not Pawn pawn2 || pawn2.IsCombatant() || GenSight.LineOfSightToThing(pawn.Position, pawn2, pawn.Map)))
					{
						Thing thing2 = (Thing)attackTarget;
						int num2 = thing2.Position.DistanceToSquared(pawn.Position);
						if ((float)num2 < num && pawn.CanReach(thing2, PathEndMode.OnCell, Danger.Deadly) && !thing2.IsForbidden(pawn))
						{
							// if (!thing2.IsForbidden(pawn))
							// {
							num = num2;
							thing = thing2;
							// }
							// else
							// {
								// return false;
							// }
						}
					}
				}
				if (thing != null)
				{
					return true;
				}
			}
			return false;
		}
	}
}
