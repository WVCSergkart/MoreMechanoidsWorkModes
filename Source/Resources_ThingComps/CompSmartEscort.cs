using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace WVC_WorkModes
{

	public class CompProperties_SmartEscort : CompProperties
	{

		public string uiIconPath = "WVC/UI/MechWork/EscortTargetAssign";

		// public float uiIconSize = 1.0f;

		public CompProperties_SmartEscort()
		{
			compClass = typeof(CompSmartEscort);
		}
	}

	public class CompSmartEscort : ThingComp
	{

		public Pawn overseer = null;

		public Pawn escortTarget = null;

		// public int tickCounter = 1500;

		public CompProperties_SmartEscort Props => (CompProperties_SmartEscort)props;

		public Pawn GetEscortee(Pawn pawn)
		{
			if (escortTarget != null)
			{
				return escortTarget;
			}
			return pawn;
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			base.CompGetGizmosExtra();
			Pawn pawn = parent as Pawn;
			overseer = pawn.GetOverseer();
			if (escortTarget == null)
			{
				escortTarget = overseer;
			}
			if (overseer == null)
			{
				yield break;
			}
			if (escortTarget != overseer && !ShutdownUtility.AssignedPawnAtHome(escortTarget))
			{
				Messages.Message("WVC_WorkModes_AssignToPawnEscortNoLongerEscort".Translate(pawn.LabelIndefinite().CapitalizeFirst(), escortTarget.LabelIndefinite().CapitalizeFirst()), escortTarget, MessageTypeDefOf.NeutralEvent, historical: false);
				escortTarget = overseer;
			}
			Command_Action command_Action = new()
			{
				defaultLabel = "WVC_WorkModes_AssignToPawnEscortLabel".Translate(),
				defaultDesc = "WVC_WorkModes_AssignToPawnEscortDesc".Translate(pawn.LabelIndefinite().CapitalizeFirst(), GetEscortee(overseer).LabelIndefinite().CapitalizeFirst()),
				icon = ContentFinder<Texture2D>.Get(Props.uiIconPath),
				shrinkable = true,
				action = delegate
				{
					List<FloatMenuOption> list = new();
					List<Pawn> freeColonists = parent.Map.mapPawns.FreeColonists;
					for (int i = 0; i < freeColonists.Count; i++)
					{
						Pawn localPawn = freeColonists[i];
						list.Add(new FloatMenuOption(localPawn.LabelShortCap, delegate
						{
							foreach (Pawn item2 in Find.Selector.SelectedPawns.Where((Pawn p) => p.RaceProps.IsMechanoid))
							{
								CompSmartEscort comp = item2.TryGetComp<CompSmartEscort>();
								if (comp != null)
								{
									comp.escortTarget = localPawn;
									Messages.Message("WVC_WorkModes_AssignToPawnEscortAssigned".Translate(item2.LabelIndefinite().CapitalizeFirst(), localPawn.LabelIndefinite().CapitalizeFirst()), item2, MessageTypeDefOf.NeutralEvent, historical: false);
								}
							}
						}));
					}
					if (list.Any())
					{
						Find.WindowStack.Add(new FloatMenu(list));
					}
				}
			};
			yield return command_Action;
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look(ref overseer, "overseer");
			Scribe_References.Look(ref escortTarget, "escortTarget");
		}
	}

}
