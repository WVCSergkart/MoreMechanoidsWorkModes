using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace WVC_WorkModes
{

	public class CompProperties_MechSettings : CompProperties
	{

		public string uiIconZoneRestrict = "WVC/UI/WorkModes_General/Ui_RestrictZoneByGroup";

		public CompProperties_MechSettings()
		{
			compClass = typeof(CompMechSettings);
		}

	}

	public class CompMechSettings : ThingComp
	{

		public bool restrictZoneByGroup = false;

		public CompProperties_MechSettings Props => (CompProperties_MechSettings)props;

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			base.CompGetGizmosExtra();
			Pawn pawn = parent as Pawn;
			Pawn overseer = pawn.GetOverseer();
			if (overseer == null)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "WVC_WorkModes_RestrictZoneByGroupLabel".Translate().Resolve() + ": " + "\n" + Zone_MechanoidShutdown.OnOrOff(restrictZoneByGroup),
				defaultDesc = "WVC_WorkModes_RestrictZoneByGroupDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get(Props.uiIconZoneRestrict),
				action = delegate
				{
					restrictZoneByGroup = !restrictZoneByGroup;
					if (restrictZoneByGroup)
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera();
					}
					else
					{
						SoundDefOf.Tick_Low.PlayOneShotOnCamera();
					}
				}
			};
		}

		public override string CompInspectStringExtra()
		{
			base.CompInspectStringExtra();
			if (restrictZoneByGroup)
			{
				return "WVC_WorkModes_RestrictZoneByGroupInspect".Translate().Colorize(ColorLibrary.Orange);
			}
			return null;
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref restrictZoneByGroup, "restrictZoneByGroup", false);
		}

	}

}
