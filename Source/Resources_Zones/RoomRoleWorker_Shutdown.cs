// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace WVC_WorkModes
{
    public class RoomRoleWorker_Shutdown : RoomRoleWorker
	{

		//private static List<Thing> tmpAnchors = new();

		public override float GetScore(Room room)
		{
			//tmpAnchors.Clear();
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			int num = 0;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i].def.GetCompProperties<CompProperties_ShutdownRoom>() != null || containedAndAdjacentThings[i] is Building_MechCharger)
				{
					num++;
					//tmpAnchors.Add(containedAndAdjacentThings[i]);
				}
			}
			if (num == 0)
			{
				//tmpAnchors.Clear();
				return 0f;
			}
			return 900000f;
		}

		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.building == null || buildingDef.GetCompProperties<CompProperties_ShutdownRoom>() != null || !buildingDef.thingClass.IsAssignableFrom(typeof(Building_MechCharger)))
			{
				return 0f;
			}
			return 900000f;
		}
	}

}
