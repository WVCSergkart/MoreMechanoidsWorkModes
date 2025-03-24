using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace WVC_WorkModes
{

	public class CompProperties_MechSettings : CompProperties
	{

		public string uiIconZoneRestrict = "WVC/UI/WorkModes_General/Ui_RestrictZoneByGroup";

		public string uiIconAssign = "WVC/UI/WorkModes_General/EscortTargetAssign";

		public string uiIconReset = "WVC/UI/WorkModes_General/EscortTargetAssignReset";

		public CompProperties_MechSettings()
		{
			compClass = typeof(CompMechSettings);
		}

	}

	public class CompMechSettings : ThingComp
	{

		public bool restrictZoneByGroup = false;

		// public List<MechWorkModeDef> cachedShutdownModes;

		public CompProperties_MechSettings Props => (CompProperties_MechSettings)props;

		[Unsaved(false)]
		private Pawn overseer = null;

		public Pawn Overseer
        {
			get
            {
				if (overseer?.mechanitor == null)
                {
					overseer = Mech.GetOverseer();
				}
				return overseer;
            }
		}

		public Pawn Mech
		{
			get
			{
				return parent as Pawn; ;
			}
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (parent.Faction != Faction.OfPlayer)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "WVC_WorkModes_RestrictZoneByGroupLabel".Translate().Resolve() + ": " + "\n" + Zone_MechanoidShutdown.OnOrOff(restrictZoneByGroup),
				defaultDesc = "WVC_WorkModes_RestrictZoneByGroupDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get(Props.uiIconZoneRestrict),
				shrinkable = true,
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
			if (!WVC_MMWM.settings.enableSmartEscort)
			{
				yield break;
			}
			if (Overseer == null)
			{
				yield break;
			}
			if (cachedEscortModes == null)
			{
				cachedEscortModes = SmartEscortUtility.SmartEscortModesList();
			}
			if (!SmartEscortUtility.ShowAssignGizmo(Overseer, Mech, cachedEscortModes))
			{
				yield break;
			}
			if (escortTarget != null && escortTarget.Map == null)
			{
				Messages.Message("WVC_WorkModes_AssignToPawnEscortNoLongerEscort".Translate(Mech.LabelIndefinite().CapitalizeFirst(), escortTarget.LabelIndefinite().CapitalizeFirst()), escortTarget, MessageTypeDefOf.NeutralEvent, historical: false);
				escortTarget = null;
			}
			yield return new Command_Action
			{
				defaultLabel = "WVC_WorkModes_AssignToPawnEscortLabel".Translate(),
				defaultDesc = "WVC_WorkModes_AssignToPawnEscortDesc".Translate(Mech.LabelIndefinite().CapitalizeFirst(), GetEscortee(Overseer).LabelIndefinite().CapitalizeFirst()),
				icon = ContentFinder<Texture2D>.Get(Props.uiIconAssign),
				shrinkable = true,
				action = delegate
				{
					List<FloatMenuOption> list = new();
					List<Pawn> freeColonists = parent.Map.mapPawns.FreeColonists;
					for (int i = 0; i < freeColonists.Count; i++)
					{
						Pawn localPawn = freeColonists[i];
						if (localPawn != Overseer && (localPawn != escortTarget || Find.Selector.SelectedPawns.Count > 1))
						{
							list.Add(new FloatMenuOption(localPawn.Name.ToStringFull, delegate
							{
								foreach (Pawn item2 in Find.Selector.SelectedPawns.Where((Pawn p) => p.RaceProps.IsMechanoid))
								{
									CompMechSettings comp = item2.TryGetComp<CompMechSettings>();
									if (comp != null)
									{
										comp.escortTarget = localPawn;
										Messages.Message("WVC_WorkModes_AssignToPawnEscortAssigned".Translate(item2.LabelIndefinite().CapitalizeFirst(), localPawn.LabelIndefinite().CapitalizeFirst()), item2, MessageTypeDefOf.NeutralEvent, historical: false);
									}
								}
							}));
						}
					}
					if (list.Any())
					{
						Find.WindowStack.Add(new FloatMenu(list));
					}
				}
			};
			yield return new Command_Action
			{
				defaultLabel = "WVC_WorkModes_AssignToPawnEscortLabelReset".Translate(),
				defaultDesc = "WVC_WorkModes_AssignToPawnEscortDesc".Translate(Mech.LabelIndefinite().CapitalizeFirst(), GetEscortee(Overseer).LabelIndefinite().CapitalizeFirst()),
				icon = ContentFinder<Texture2D>.Get(Props.uiIconReset),
				shrinkable = true,
				action = delegate
				{
					foreach (Pawn item2 in Find.Selector.SelectedPawns.Where((Pawn p) => p.RaceProps.IsMechanoid))
					{
						CompMechSettings comp = item2.TryGetComp<CompMechSettings>();
						if (comp != null)
						{
							comp.escortTarget = null;
							Messages.Message("WVC_WorkModes_AssignToPawnEscortAssigned".Translate(item2.LabelIndefinite().CapitalizeFirst(), Overseer.LabelIndefinite().CapitalizeFirst()), item2, MessageTypeDefOf.NeutralEvent, historical: false);
						}
					}
				}
			};
		}

		public Pawn escortTarget = null;

		private List<MechWorkModeDef> cachedEscortModes = null;

		public Pawn GetEscortee(Pawn pawn)
		{
			if (escortTarget != null)
			{
				return escortTarget;
			}
			return pawn;
		}

		public bool autoRepairUpdated = false;

		public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if (respawningAfterLoad || autoRepairUpdated)
            {
                return;
            }
            if (WVC_MMWM.settings.enableAutoRepairByDefault && parent is Pawn mech)
            {
                CompMechRepairable compMechRepairable = mech?.TryGetComp<CompMechRepairable>();
                if (compMechRepairable != null)
                {
                    compMechRepairable.autoRepair = true;
                }
            }
            autoRepairUpdated = true;
        }

        public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new();
			bool addAppendLine = false;
			if (restrictZoneByGroup)
			{
				stringBuilder.Append(string.Format("{0}", "WVC_WorkModes_RestrictZoneByGroupInspect".Translate().Colorize(ColorLibrary.Orange)));
				addAppendLine = true;
			}
			if (escortTarget != null && SmartEscortUtility.ShowAssignGizmo(Overseer, Mech, cachedEscortModes))
			{
				if (addAppendLine)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("WVC_WorkModes_EscortsAssignTargetLabel".Translate(escortTarget.LabelIndefinite().CapitalizeFirst()).Colorize(ColorLibrary.Orange));
			}
			return stringBuilder.ToString();
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref restrictZoneByGroup, "restrictZoneByGroup", false);
			//Scribe_References.Look(ref overseer, "overseer");
			Scribe_References.Look(ref escortTarget, "escortTarget");
			Scribe_Values.Look(ref autoRepairUpdated, "autoRepairUpdated", false);
		}

	}

}
