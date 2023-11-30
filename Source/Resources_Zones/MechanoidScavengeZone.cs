// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace WVC_WorkModes
{

    public class Zone_MechanoidScavenge : Zone
	{

		private readonly int reqCells = WVC_MMWM.settings.WVC_Scavenging_ReqCells;

		public bool ZoneIsActive()
		{
			if (CellCount >= reqCells)
			{
				return true;
			}
			return false;
		}

		protected override Color NextZoneColor => MechanoidZoneColorUtility.NextMechanoidZoneColor();

		public override void ExposeData()
		{
			base.ExposeData();
		}

		public Zone_MechanoidScavenge()
		{
		}

		public Zone_MechanoidScavenge(ZoneManager zoneManager)
			: base("WVC_MechanoidScavengeZone".Translate(), zoneManager)
		{
		}

		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
		}

		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
		}

		public override void PostDeregister()
		{
			base.PostDeregister();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
		}

		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_MechanoidScavengeZone_Expand>();
		}

		public override string GetInspectString()
		{
			// StringBuilder stringBuilder = new(base.GetInspectString());
			// StringBuilder stringBuilder = new();
			// stringBuilder.AppendLine();
			// if (ZoneIsActive())
			// {
				// stringBuilder.AppendLine(string.Format("{0}", "WVC_MechanoidScavengeZone_CellsCount".Translate(CellCount, "25").CapitalizeFirst()));
			// }
			// return stringBuilder.ToString();
			return string.Format("{0}", "WVC_MechanoidScavengeZone_CellsCount".Translate(CellCount, reqCells.ToString()).CapitalizeFirst());
		}

	}

	public class Designator_MechanoidScavengeZone_Expand : Designator_MechanoidScavengeZone
	{
		public Designator_MechanoidScavengeZone_Expand()
		{
			defaultLabel = "DesignatorZoneExpand".Translate();
		}
	}

	public class Designator_MechanoidScavengeZone : Designator_ZoneAdd
	{
		protected override string NewZoneLabel => "WVC_MechanoidScavengeZone".Translate();

		public Designator_MechanoidScavengeZone()
		{
			zoneTypeToPlace = typeof(Zone_MechanoidScavenge);
			defaultLabel = "WVC_MechanoidScavengeZone".Translate();
			defaultDesc = "WVC_MechanoidScavengeZoneDesc".Translate();
			icon = ContentFinder<Texture2D>.Get("WVC/Spots/ScavengeZone");
			// hotKey = KeyBindingDefOf.Misc2;
			// tutorTag = "ZoneAdd_Growing";
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!base.CanDesignateCell(c).Accepted)
			{
				return false;
			}
			// Log.Error("1");
			TerrainDef terrainDef = Map.terrainGrid.TerrainAt(c);
			// Log.Error("2");
			if (terrainDef == null || !terrainDef.natural || terrainDef.layerable)
			{
				return false;
			}
			// Log.Error("3");
			// if ((!terrainDef.natural || terrainDef.layerable) && !terrainDef.IsSoil)
			// {
				// return false;
			// }
			// if ((!terrainDef.natural || terrainDef.layerable) && !terrainDef.IsSoil)
			// {
				// return false;
			// }
			if (!terrainDef.affordances.Contains(TerrainAffordanceDefOf.Diggable))
			{
				return false;
			}
			// Log.Error("4");
			return true;
		}

		protected override Zone MakeNewZone()
		{
			// PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WVC_MMWM_BehaviorsGuide, KnowledgeAmount.Total);
			return new Zone_MechanoidScavenge(Find.CurrentMap.zoneManager);
		}
	}

}
