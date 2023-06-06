// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;

namespace WVC
{
	public class JobGiver_GetEnergy_SelfShutdown : JobGiver_GetEnergy
	{
		public int tickInterval = 3000;
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ShouldAutoRecharge(pawn))
			{
				return null;
			}
			if (RCellFinder.TryFindRandomMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out var result))
			{
				Job job = JobMaker.MakeJob(JobDefOf.SelfShutdown, result);
				job.checkOverrideOnExpire = true;
				job.expiryInterval = tickInterval;
				return job;
			}
			return null;
		}
	}
	public class JobGiver_GoToClosetShutdownSpot : ThinkNode_JobGiver
	{
		public ThingDef spotDefName;
		public ThingDef backupSpotDefName;
		public int minDistanceForSpot = 9;
		public int maxDistanceForSpot = 9999;

		public Building GetCloseShutdownSpot(Pawn mech)
        {
			return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(spotDefName), PathEndMode.OnCell, TraverseParms.For(mech), minDistanceForSpot, delegate (Thing t)
			{
				return !t.IsForbidden(mech);
			});
		}

        public Building GetClosestShutdownSpot(Pawn mech)
		{
			return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(spotDefName), PathEndMode.OnCell, TraverseParms.For(mech), maxDistanceForSpot, delegate (Thing t)
			{
				return !t.IsForbidden(mech);
			});
		}

		public Building GetCloseBackupShutdownSpot(Pawn mech)
		{
			return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(backupSpotDefName), PathEndMode.OnCell, TraverseParms.For(mech), minDistanceForSpot, delegate (Thing t)
			{
				return !t.IsForbidden(mech);
			});
		}

		public Building GetClosestBackupShutdownSpot(Pawn mech)
		{
			return (Building)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForDef(backupSpotDefName), PathEndMode.OnCell, TraverseParms.For(mech), maxDistanceForSpot, delegate (Thing t)
			{
				return !t.IsForbidden(mech);
			});
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Building spotIsClose = GetCloseShutdownSpot(pawn);
			if (spotIsClose != null)
			{
				return null;
			}
			Building spotIsFar = GetClosestShutdownSpot(pawn);
			if (spotIsFar != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Goto, spotIsFar);
				return job;
			}
			Building backupSpotIsClose = GetCloseBackupShutdownSpot(pawn);
			if (backupSpotIsClose != null)
			{
				return null;
			}
			Building backupSpotIsFar = GetClosestBackupShutdownSpot(pawn);
			if (backupSpotIsFar != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Goto, backupSpotIsFar);
				return job;
			}
			return null;
		}
	}
	public class ThinkNode_ConditionalEnemyOnMap : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn pawn)
		{
			float num = float.MaxValue;
			Thing thing = null;
			List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				IAttackTarget attackTarget = potentialTargetsFor[i];
				if (!attackTarget.ThreatDisabled(pawn) && AttackTargetFinder.IsAutoTargetable(attackTarget) && (!(attackTarget.Thing is Pawn pawn2) || pawn2.IsCombatant() || GenSight.LineOfSightToThing(pawn.Position, pawn2, pawn.Map)))
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
			return false;
		}
	}
	// public class ThinkNode_AllowedArea : ThinkNode_Conditional
	// {
		// protected override bool Satisfied(Pawn pawn)
		// {
			// if (!pawn.Position.IsForbidden(pawn))
			// {
				// return true;
			// }
			// return false;
		// }

	// }
	public class JobGiver_GetEnergy_Charger : JobGiver_GetEnergy
	{
		public int tickInterval = 625;

		public static Building_MechCharger GetClosestCharger(Pawn mech, Pawn carrier, bool forced)
		{
			Danger danger = (forced ? Danger.Deadly : Danger.Some);
			return (Building_MechCharger)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForGroup(ThingRequestGroup.MechCharger), PathEndMode.InteractionCell, TraverseParms.For(carrier, danger), 9999f, delegate(Thing t)
			{
				Building_MechCharger building_MechCharger = (Building_MechCharger)t;
				if (!carrier.CanReach(t, PathEndMode.InteractionCell, danger))
				{
					return false;
				}
				if (carrier != mech)
				{
					if (!forced && building_MechCharger.Map.reservationManager.ReservedBy(building_MechCharger, carrier))
					{
						return false;
					}
					if (forced && KeyBindingDefOf.QueueOrder.IsDownEvent && building_MechCharger.Map.reservationManager.ReservedBy(building_MechCharger, carrier))
					{
						return false;
					}
				}
				return !t.IsForbidden(carrier) && carrier.CanReserve(t, 1, -1, null, forced) && building_MechCharger.CanPawnChargeCurrently(mech);
			});
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			// if (!ShouldAutoRecharge(pawn))
			// {
				// return null;
			// }
			Building_MechCharger closestCharger = GetClosestCharger(pawn, pawn, forced: false);
			if (closestCharger != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.MechCharge, closestCharger, tickInterval);
				job.overrideFacing = Rot4.South;
				job.checkOverrideOnExpire = true;
				job.expiryInterval = tickInterval;
				return job;
			}
			return null;
		}
	}
}
