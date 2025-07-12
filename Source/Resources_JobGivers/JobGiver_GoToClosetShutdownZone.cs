// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
	public class JobGiver_GoToShutdownZone : ThinkNode_JobGiver
	{

		//[Obsolete]
		//public string spotDefName;
		//[Obsolete]
		//public string backupSpotDefName;
		//[Obsolete]
		//public int minDistanceForSpot = 9;
		//[Obsolete]
		//public int maxDistanceForSpot = 9999;

		public List<string> possibleRooms;

		private static List<RoomRoleDef> cachedPossibleRooms;

		public List<RoomRoleDef> PossibleRooms
        {
            get
            {
				if (cachedPossibleRooms == null)
                {
					cachedPossibleRooms = possibleRooms.ConvertToDefs();
				}
                return cachedPossibleRooms;
            }
        }

        public MechanoidWorkType workModeType = 0;

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (WVC_MMWM.settings.enable_GoToShutdownZoneJob)
			{
				if (GetZoneJob(pawn, out Job job))
                {
					return job;
				}
			}
			if (WVC_MMWM.settings.enable_GoToShutdownRoomJob)
			{
				if (GetRoomJob(pawn, out Job job))
				{
					return job;
				}
			}
			return null;
		}

		private bool GetRoomJob(Pawn pawn, out Job job)
		{
			job = null;
			if (ShutdownUtility.MechInShutdownZone(pawn, pawn.Position, PossibleRooms))
			{
				return false;
			}
			if (GetShutdownRoom(pawn, out job))
			{
				return job != null;
			}
			return false;
		}

		private bool GetZoneJob(Pawn pawn, out Job job)
		{
			job = null;
			List<Zone> mapZones = pawn.Map?.zoneManager?.AllZones;
			if (mapZones.NullOrEmpty() || !ShutdownUtility.AnyMechanoidZone(mapZones))
			{
				return false;
			}
			if (ShutdownUtility.MechInShutdownZone(pawn, pawn.Position, workModeType))
			{
				return false;
			}
			if (ShutdownUtility.TryFindRandomMechShutdownZone(mapZones, pawn, pawn.Map, workModeType, out var result))
			{
				if (pawn.Position == result)
				{
					return false;
				}
				job = JobMaker.MakeJob(JobDefOf.Goto, result);
				return true;
			}
			return false;
		}

		private bool GetShutdownRoom(Pawn pawn, out Job job)
		{
			job = null;
			if (possibleRooms == null)
			{
				return false;
			}
			foreach (Room room in pawn.Map.regionGrid.AllRooms)
			{
				if (PossibleRooms.Contains(room.Role))
				{
					if (ShutdownUtility.TryFindRandomMechShutdownZone(room.Cells.ToList(), pawn, pawn.Map, out var result))
					{
						if (pawn.Position == result)
						{
							return false;
						}
						job = JobMaker.MakeJob(JobDefOf.Goto, result);
						return true;
					}
				}
			}
			return false;
		}

	}

}
