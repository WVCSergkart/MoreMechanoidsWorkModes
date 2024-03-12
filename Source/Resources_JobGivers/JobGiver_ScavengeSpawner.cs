// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
    public class JobGiver_ScavengeSpawner : ThinkNode_JobGiver
	{

		public List<ThingDefByWeight> productDefs;

		public string message = "MessageCompSpawnerSpawnedItem";

		public bool showMessageIfOwned = false;

		public bool spawnForbidden = false;

		public float chanceToSuccess = 0.05f;

		public StatDef statDef;

		protected override Job TryGiveJob(Pawn pawn)
		{
			List<Zone> mapZones = pawn.Map.zoneManager.AllZones;
			if (mapZones.NullOrEmpty() || !ScavengeUtility.AnyScavengeZone(mapZones))
			{
				return null;
			}
			if (!productDefs.NullOrEmpty() && statDef != null)
			{
				if (Rand.Chance(chanceToSuccess * pawn.GetStatValue(statDef)))
				{
					ThingDefByWeight thingDefByWeight = productDefs.RandomElementByWeight((ThingDefByWeight x) => x.selectionWeight);
					ThingDef productDef = thingDefByWeight.productDef;
					int productCount = thingDefByWeight.productCount.RandomInRange;
					MiscUtility.ScavengeSpawner(pawn, productDef, productCount, message, showMessageIfOwned, spawnForbidden);
				}
			}
			return null;
		}

	}
}
