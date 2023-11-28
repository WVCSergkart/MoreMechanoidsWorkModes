// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using WVC;

namespace WVC_WorkModes
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
			if (WVC_MMWM.settings.useCustomShutdownBehavior)
			{
				if (ShutdownUtility.TryFindNearbyMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out var result))
				{
					Job job = JobMaker.MakeJob(JobDefOf.SelfShutdown, result);
					job.checkOverrideOnExpire = true;
					job.expiryInterval = tickInterval;
					return job;
				}
			}
			else
			{
				// Vanilla
				if (RCellFinder.TryFindNearbyMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out var result))
				{
					Job job = JobMaker.MakeJob(JobDefOf.SelfShutdown, result);
					job.checkOverrideOnExpire = true;
					job.expiryInterval = tickInterval;
					return job;
				}
			}
			return null;
		}
	}

	public class JobGiver_GetEnergy_Charger : JobGiver_GetEnergy
	{
		public int tickInterval = 625;

		protected override Job TryGiveJob(Pawn pawn)
		{
			float maxRechargeLimit = GetMaxRechargeLimit(pawn);
			// if (maxRechargeLimit < 97f)
			// {
				// return null;
			// }
			Need_MechEnergy energy = pawn.needs.energy;
			if (energy.CurLevel + 0.1f < maxRechargeLimit - 5f)
			{
				Building_MechCharger closestCharger = GetClosestCharger(pawn, pawn, forced: false);
				if (closestCharger != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.MechCharge, closestCharger);
					job.overrideFacing = Rot4.South;
					job.checkOverrideOnExpire = true;
					job.expiryInterval = tickInterval;
					return job;
				}
			}
			return null;
		}

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
	}
}
