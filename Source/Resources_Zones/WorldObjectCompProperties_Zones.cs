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

		public virtual void CopyZonesIntoMap(Gravship gravship, Map map, IntVec3 root)
		{
			ShutdownUtility.CopyZonesIntoMap(ref savedZones, gravship, map, root);
		}

		public List<MoveableShutdownZone> savedZones = new();

		public virtual void TransferZones(Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors)
		{
			ShutdownUtility.SaveZonesFromMap(oldMap, origin, engineFloors, parent as Gravship, ref savedZones);
		}

		public void ExposeData()
		{
			Scribe_Collections.Look(ref savedZones, "savedZones", LookMode.Deep);
		}

	}

	public static class StaticCollectionsClass
	{

		public static List<MoveableShutdownZone> backupSavedZones = new();

	}

}
