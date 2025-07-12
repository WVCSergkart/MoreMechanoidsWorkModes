// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace WVC_WorkModes
{
    public class MoveableShutdownZone : MoveableArea
	{

		private bool hidden;
		public Pawn owner;
		public int? ownerIndexGroup;
		public bool allowWorkers = true;
		public bool allowSafe = true;
		public bool allowCombatants = true;
		public bool allowAmbush = true;

		public MoveableShutdownZone()
		{
		}

		public MoveableShutdownZone(Gravship gravship, Zone_MechanoidShutdown stockpile)
			: base(gravship, stockpile.label, stockpile.RenamableLabel, stockpile.color, stockpile.ID)
		{
			hidden = stockpile.Hidden;
			owner = stockpile.owner;
			ownerIndexGroup = stockpile.ownerIndexGroup;
			allowWorkers = stockpile.allowWorkers;
			allowSafe = stockpile.allowSafe;
			allowCombatants = stockpile.allowCombatants;
			allowAmbush = stockpile.allowAmbush;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref allowWorkers, "allowWorkers", true);
			Scribe_Values.Look(ref allowSafe, "allowSafe", true);
			Scribe_Values.Look(ref allowCombatants, "allowCombatants", true);
			Scribe_Values.Look(ref allowAmbush, "allowAmbush", true);
			// Groups
			Scribe_References.Look(ref owner, "owner");
			// Scribe_Deep.Look(ref ownerGroup, "ownerGroup");
			Scribe_Values.Look(ref ownerIndexGroup, "ownerIndexGroup");
			Scribe_Values.Look(ref hidden, "hidden", defaultValue: false);
		}

		public void TryCreateStockpile(ZoneManager zoneManager, IntVec3 newOrigin)
		{
			Zone_Stockpile zone_Stockpile = new Zone_Stockpile(StorageSettingsPreset.DefaultStockpile, zoneManager)
			{
				label = label,
				Hidden = hidden,
				color = color,
				ID = id
			};
			zone_Stockpile.settings = new StorageSettings(zone_Stockpile);
			zone_Stockpile.settings.CopyFrom(settings);
			zoneManager.RegisterZone(zone_Stockpile);
			foreach (IntVec3 relativeCell in base.RelativeCells)
			{
				zone_Stockpile.AddCell(newOrigin + relativeCell);
			}
		}
	}

}
