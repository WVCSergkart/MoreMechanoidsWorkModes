using RimWorld;
using UnityEngine;
using Verse;

namespace WVC_WorkModes
{

    public class ThingDefByWeight
	{

		public ThingDef productDef;

		public IntRange productCount = new(1, 1);

		public float selectionWeight = 1.0f;

	}

	public static class MiscUtility
	{

		public static void SliderLabeledWithRef(this Listing_Standard ls, string label, ref int val, float min = 0f, float max = 1f, string tooltip = null)
		{
			Rect rect = ls.GetRect(Text.LineHeight);
			Rect rect2 = rect.LeftPart(0.5f).Rounded();
			Rect rect3 = rect.RightPart(0.62f).Rounded().LeftPart(0.97f).Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect2, label);
			val = (int)Widgets.HorizontalSlider_NewTemp(rect3, val, min, max, middleAlignment: true);
			Text.Anchor = TextAnchor.MiddleRight;
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			Text.Anchor = anchor;
			ls.Gap(ls.verticalSpacing);
		}

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
