using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace WVC_WorkModes
{

    public class WorldObjectCompProperties_Zones : WorldObjectCompProperties
    {



        public WorldObjectCompProperties_Zones()
        {
            compClass = typeof(WorldObjectComp_Zones);
        }

    }

    public class WorldObjectComp_Zones : WorldObjectComp
    {

		//public override void PostMapGenerate()
		//{
		//	base.PostMapGenerate();
		//	Log.Error("Map gen");
		//}

		//public override void PostPostRemove()
		//{
		//	base.PostPostRemove();
		//	Log.Error("Removed");
		//}

		public virtual void CopyZonesIntoMap(Gravship gravship, Map map, IntVec3 root)
		{
			CopyZonesIntoMap(savedZones, gravship, map, root);
			savedZones = null;
		}

		public static void CopyZonesIntoMap(List<MoveableShutdownZone> savedZones, Gravship gravship, Map map, IntVec3 root)
		{
			foreach (MoveableShutdownZone stockpile in savedZones)
			{
				stockpile.TryCreateShutdown(map.zoneManager, root);
			}
		}

		public List<MoveableShutdownZone> savedZones = new();

		public static List<MoveableShutdownZone> backupSavedZones = new();

		public virtual void TransferZones(Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors)
		{
			savedZones = new();
			SaveZonesFromMap(oldMap, origin, engineFloors, parent as Gravship, savedZones);
		}

		public static void SaveZonesFromMap(Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors, Gravship gravship, List<MoveableShutdownZone> savedZones)
		{
			for (int num = oldMap.zoneManager.AllZones.Count - 1; num >= 0; num--)
			{
				Zone zone = oldMap.zoneManager.AllZones[num];
				if (zone is Zone_MechanoidShutdown stutdown)
				{
					MoveableShutdownZone moveableShutdown = null;
					foreach (IntVec3 cell in stutdown.Cells)
					{
						if (engineFloors.Contains(cell))
						{
							if (moveableShutdown == null)
							{
								moveableShutdown = new MoveableShutdownZone(gravship, stutdown);
							}
							moveableShutdown.Add(cell - origin);
						}
					}
					if (moveableShutdown != null)
					{
						savedZones.Add(moveableShutdown);
						zone.Delete();
					}
				}
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look(ref savedZones, "savedZones", LookMode.Deep);
		}

	}

}
