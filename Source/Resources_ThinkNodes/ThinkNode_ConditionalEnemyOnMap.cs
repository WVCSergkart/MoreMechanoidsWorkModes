// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
    public class ThinkNode_ConditionalEnemyOnMap : ThinkNode_Conditional
	{

		protected override bool Satisfied(Pawn pawn)
		{
			if (WVC_MMWM.settings.enableEnemySearching)
			{
				return AnyEnemy(pawn);
			}
			return false;
		}

		private int delayTicks = -1;
		private bool? cachedAnswer;

		private bool AnyEnemy(Pawn pawn)
		{
			//float num = float.MaxValue;
			//Thing thing = null;
			//List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			//for (int i = 0; i < potentialTargetsFor.Count; i++)
			//{
			//	IAttackTarget attackTarget = potentialTargetsFor[i];
			//	if (!attackTarget.ThreatDisabled(pawn) && AttackTargetFinder.IsAutoTargetable(attackTarget) && (attackTarget.Thing is not Pawn pawn2 || pawn2.IsCombatant() || GenSight.LineOfSightToThing(pawn.Position, pawn2, pawn.Map)))
			//	{
			//		Thing thing2 = (Thing)attackTarget;
			//		int num2 = thing2.Position.DistanceToSquared(pawn.Position);
			//		if ((float)num2 < num && pawn.CanReach(thing2, PathEndMode.OnCell, Danger.Deadly) && !thing2.IsForbidden(pawn))
			//		{
			//			num = num2;
			//			thing = thing2;
			//		}
			//	}
			//}
			//if (thing != null)
			//{
			//	return true;
			//},
			// Small delay for better performance
			if (cachedAnswer == null || delayTicks < Find.TickManager.TicksGame)
			{
				cachedAnswer = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn).Any(target => target is Thing thing && pawn.CanReach(thing, PathEndMode.OnCell, Danger.Deadly) && !thing.IsForbidden(pawn));
				delayTicks = Find.TickManager.TicksGame + Delay;
			}
			return cachedAnswer.Value;
		}

		// Hook
		public static int Delay => WVC_MMWM.settings.enemyCheckDelay;

	}
}
