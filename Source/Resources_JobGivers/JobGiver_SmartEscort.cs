// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{

    public class JobGiver_SmartAIDefendOverseer : JobGiver_AIDefendOverseer
	{

		protected override Pawn GetDefendee(Pawn pawn)
		{
			return ShutdownUtility.GetAssignedPawnOnMap(pawn);
		}

		protected override float GetFlagRadius(Pawn pawn)
		{
			return 5f;
		}
	}

	public class JobGiver_SmartAIFollowOverseer : JobGiver_AIFollowOverseer
	{
		protected override int FollowJobExpireInterval => 200;

		protected override Pawn GetFollowee(Pawn pawn)
		{
			return ShutdownUtility.GetAssignedPawnOnMap(pawn);
		}

		protected override float GetRadius(Pawn pawn)
		{
			return 5f;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (ShutdownUtility.GetAssignedPawnOnMap(pawn) == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}
	}

	public class JobGiver_SmartAIWaitWithOverseer : JobGiver_AIWaitWithOverseer
	{
		public float randomCellNearRadius = 1.9f;

		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn overseer = ShutdownUtility.GetAssignedPawnOnMap(pawn);
			if (overseer == null || overseer.Awake())
			{
				return null;
			}
			IntVec3 intVec = (CanUseCell(pawn.Position, pawn) ? pawn.Position : GetWaitDest(overseer.Position, pawn));
			if (intVec.IsValid)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_WithSleeping, intVec, overseer);
				job.expiryInterval = 120;
				job.expireRequiresEnemiesNearby = true;
				return job;
			}
			return null;
		}

		private IntVec3 GetWaitDest(IntVec3 root, Pawn mech)
		{
			Map map = mech.Map;
			if (CellFinder.TryFindRandomReachableCellNear(root, map, randomCellNearRadius, TraverseParms.For(mech), (IntVec3 c) => CanUseCell(c, mech), null, out var result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		private bool CanUseCell(IntVec3 c, Pawn mech)
		{
			Map map = mech.Map;
			if (c.Standable(map) && mech.CanReach(c, PathEndMode.OnCell, Danger.Deadly) && mech.CanReserve(c))
			{
				return c.GetDoor(map) == null;
			}
			return false;
		}
	}

	public class JobGiver_SmartWanderOverseer : JobGiver_WanderOverseer
	{
		public JobGiver_SmartWanderOverseer()
		{
			wanderRadius = 3f;
			ticksBetweenWandersRange = new IntRange(125, 200);
		}

		private GlobalTargetInfo Target(Pawn pawn)
		{
			return ShutdownUtility.GetAssignedPawnOnMap(pawn);
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			GlobalTargetInfo globalTargetInfo = Target(pawn);
			if (globalTargetInfo.Map != pawn.Map)
			{
				return null;
			}
			Job job = base.TryGiveJob(pawn);
			job.reportStringOverride = "Escorting".Translate(globalTargetInfo.Thing.Named("TARGET"));
			return job;
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return Target(pawn).Cell;
		}

		protected override void DecorateGotoJob(Job job)
		{
			job.expiryInterval = 120;
			job.expireRequiresEnemiesNearby = true;
		}
	}

}
