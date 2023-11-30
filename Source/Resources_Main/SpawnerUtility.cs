using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using WVC;

namespace WVC_WorkModes
{

	public class ThingDefByWeight
	{

		public ThingDef productDef;

		public IntRange productCount = new(1, 1);

		public float selectionWeight = 1.0f;

	}

	public static class SpawnerUtility
	{

		public static void ScavengeSpawner(Pawn parent, ThingDef productDef, int productCount, string spawnMessage = "MessageCompSpawnerSpawnedItem", bool spawnForbidden = false, bool showMessageIfOwned = false, EffecterDef effecterDef = null)
		{
			if (productDef != null)
			{
				Thing thing = ThingMaker.MakeThing(productDef);
				thing.stackCount = productCount;
				GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near, out var lastResultingThing, null, default);
				if (spawnForbidden || parent.Faction != Faction.OfPlayer)
				{
					lastResultingThing.SetForbidden(value: true);
				}
				if (showMessageIfOwned && parent.Faction == Faction.OfPlayer)
				{
					Messages.Message(spawnMessage.Translate(productDef.LabelCap), thing, MessageTypeDefOf.PositiveEvent);
				}
				if (effecterDef == null)
				{
					effecterDef = EffecterDefOf.Mine;
				}
				effecterDef.Spawn().Trigger(parent, thing);
			}
		}

	}

}
