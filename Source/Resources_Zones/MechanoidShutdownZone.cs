// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace WVC_WorkModes
{
    [DefOf]
	public static class ConceptDefOf
	{
		[MayRequireBiotech]
		public static ConceptDef WVC_MMWM_BehaviorsGuide;

		static ConceptDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ConceptDefOf));
		}
	}

	public class Zone_MechanoidShutdown : Zone
	{

		public bool allowWorkers = true;
		public bool allowSafe = true;
		public bool allowCombatants = true;
		public bool allowAmbush = true;

		public Pawn owner;
		// public MechanitorControlGroup ownerGroup;
		public int ownerIndexGroup = 0;

		// Zone Color

		protected override Color NextZoneColor => MechanoidZoneColorUtility.NextMechanoidZoneColor();

		// Zone Color

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
			Scribe_Values.Look(ref ownerIndexGroup, "ownerIndexGroup", 0);
		}

		public Zone_MechanoidShutdown()
		{
		}

		public Zone_MechanoidShutdown(ZoneManager zoneManager)
			: base("WVC_MechanoidShutdownZone".Translate(), zoneManager)
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

		public static string OnOrOff(bool onOrOff)
		{
			if (onOrOff)
			{
				return "WVC_WorkModes_On".Translate().Colorize(ColorLibrary.Green);
			}
			return "WVC_WorkModes_Off".Translate().Colorize(ColorLibrary.RedReadable);
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			if (owner == null || ownerIndexGroup == 0)
			{
				yield return new Command_Action
				{
					defaultLabel = "WVC_ShutdownZone_AllowWorkers".Translate().Resolve() + ": " + "\n" + OnOrOff(allowWorkers),
					defaultDesc = "WVC_ShutdownZone_BasicDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("WVC/Spots/WorkerMechanoidShutdownSpot"),
					action = delegate
					{
						allowWorkers = !allowWorkers;
						if (allowWorkers)
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera();
						}
						else
						{
							SoundDefOf.Tick_Low.PlayOneShotOnCamera();
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "WVC_ShutdownZone_AllowSafe".Translate().Resolve() + ": " + "\n" + OnOrOff(allowSafe),
					defaultDesc = "WVC_ShutdownZone_BasicDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("WVC/Spots/EscapeMechanoidShutdownSpot"),
					action = delegate
					{
						allowSafe = !allowSafe;
						if (allowSafe)
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera();
						}
						else
						{
							SoundDefOf.Tick_Low.PlayOneShotOnCamera();
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "WVC_ShutdownZone_AllowCombatants".Translate().Resolve() + ": " + "\n" + OnOrOff(allowCombatants),
					defaultDesc = "WVC_ShutdownZone_BasicDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("WVC/Spots/WaitMechanoidShutdownSpot"),
					action = delegate
					{
						allowCombatants = !allowCombatants;
						if (allowCombatants)
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera();
						}
						else
						{
							SoundDefOf.Tick_Low.PlayOneShotOnCamera();
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "WVC_ShutdownZone_AllowAmbush".Translate().Resolve() + ": " + "\n" + OnOrOff(allowAmbush),
					defaultDesc = "WVC_ShutdownZone_BasicDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("WVC/Spots/AmbushMechanoidShutdownSpot"),
					action = delegate
					{
						allowAmbush = !allowAmbush;
						if (allowAmbush)
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
			yield return new Command_Action
			{
				defaultLabel = "WVC_WorkModes_ChosenMechanitorLabel".Translate(),
				defaultDesc = "WVC_WorkModes_ChosenMechanitorDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("WVC/UI/WorkModes_General/Ui_Overseer"),
				shrinkable = true,
				action = delegate
				{
					List<FloatMenuOption> list = new();
					List<Pawn> mechanitors = MechGroupUtility.GetAllMechanitors(Map);
					for (int i = 0; i < mechanitors.Count; i++)
					{
						Pawn localPawn = mechanitors[i];
						if (localPawn != owner)
						{
							list.Add(new FloatMenuOption(localPawn.Name.ToStringFull, delegate
							{
								owner = localPawn;
								Messages.Message("WVC_WorkModes_MechanitorIsChosen".Translate(localPawn.Name.ToStringFull, label.CapitalizeFirst()), localPawn, MessageTypeDefOf.NeutralEvent, historical: false);
							}));
						}
					}
					if (list.Any())
					{
						Find.WindowStack.Add(new FloatMenu(list));
					}
				}
			};
			if (owner == null)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "WVC_WorkModes_ChosenGroupLabel".Translate(),
				defaultDesc = "WVC_WorkModes_ChosenGroupDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get(MechGroupUtility.GetGroupIconPath(owner, ownerIndexGroup)),
				shrinkable = true,
				action = delegate
				{
					List<FloatMenuOption> list = new();
					List<MechanitorControlGroup> groups = owner.mechanitor.controlGroups;
					for (int i = 0; i < groups.Count; i++)
					{
						MechanitorControlGroup localGroup = groups[i];
						if (ownerIndexGroup == 0 || (localGroup.Index != ownerIndexGroup || localGroup.Tracker.Pawn != owner))
						{
							list.Add(new FloatMenuOption(localGroup.LabelIndexWithWorkMode, delegate
							{
								ownerIndexGroup = localGroup.Index;
								Messages.Message("WVC_WorkModes_GroupIsChosen".Translate(localGroup.Index.ToString(), label.CapitalizeFirst()), owner, MessageTypeDefOf.NeutralEvent, historical: false);
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
				defaultLabel = "WVC_WorkModes_ResetOwnerLabel".Translate(),
				defaultDesc = "WVC_WorkModes_ResetOwnerDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("WVC/UI/WorkModes_General/UiRandomize"),
				shrinkable = true,
				action = delegate
				{
					owner = null;
					ownerIndexGroup = 0;
					Messages.Message("WVC_WorkModes_ResetOwnerMessage".Translate(label.CapitalizeFirst()), null, MessageTypeDefOf.NeutralEvent, historical: false);
				}
			};
		}

		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_MechanoidShutdownZone_Expand>();
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new(base.GetInspectString());
			stringBuilder.AppendLine();
			if (owner == null || ownerIndexGroup == 0)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", "WVC_ShutdownZone_AllowWorkers".Translate().Resolve(), OnOrOff(allowWorkers)));
				stringBuilder.AppendLine(string.Format("{0}: {1}", "WVC_ShutdownZone_AllowSafe".Translate().Resolve(), OnOrOff(allowSafe)));
				stringBuilder.AppendLine(string.Format("{0}: {1}", "WVC_ShutdownZone_AllowCombatants".Translate().Resolve(), OnOrOff(allowCombatants)));
				stringBuilder.Append(string.Format("{0}: {1}", "WVC_ShutdownZone_AllowAmbush".Translate().Resolve(), OnOrOff(allowAmbush)));
			}
			else
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", "WVC_WorkModes_CurrentZoneGroup".Translate().Resolve(), MechGroupUtility.GetGroupFromIndex(owner, ownerIndexGroup).LabelIndexWithWorkMode.Colorize(ColorLibrary.LightBlue)));
				stringBuilder.Append(string.Format("{0}: {1}", "WVC_WorkModes_CurrentZoneOwner".Translate().Resolve(), owner.Name.ToStringFull.Colorize(ColoredText.NameColor)));
			}
			// stringBuilder.AppendLine(string.Format("{0}", "WVC_MechanoidShutdownZone_FreeCells".Translate(AllContainedThings.ToString()).Resolve()));
			return stringBuilder.ToString();
		}

	}

	public class Designator_MechanoidShutdownZone_Expand : Designator_MechanoidShutdownZone
	{
		public Designator_MechanoidShutdownZone_Expand()
		{
			defaultLabel = "DesignatorZoneExpand".Translate();
		}
	}

	public class Designator_MechanoidShutdownZone : Designator_ZoneAdd
	{
		protected override string NewZoneLabel => "WVC_MechanoidShutdownZone".Translate();

		public Designator_MechanoidShutdownZone()
		{
			zoneTypeToPlace = typeof(Zone_MechanoidShutdown);
			defaultLabel = "WVC_MechanoidShutdownZone".Translate();
			defaultDesc = "WVC_MechanoidShutdownZoneDesc".Translate();
			icon = ContentFinder<Texture2D>.Get("WVC/Spots/MechanoidShutdownZone");
			// hotKey = KeyBindingDefOf.Misc2;
			// tutorTag = "ZoneAdd_Growing";
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!base.CanDesignateCell(c).Accepted)
			{
				return false;
			}
			// Map map = zoneManager.map;
			Room room = c.GetRoom(Map);
			if (room != null && room.IsPrisonCell)
			{
				// Messages.Message("WVC_ShutdownZoneInPrisonCell".Translate(), null, MessageTypeDefOf.RejectInput, historical: false);
				return false;
			}
			if (!c.Standable(Map))
			{
				// Messages.Message("WVC_ShutdownZoneNotStandable".Translate(), null, MessageTypeDefOf.RejectInput, historical: false);
				return false;
			}
			return true;
		}

		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WVC_MMWM_BehaviorsGuide, KnowledgeAmount.Total);
			return new Zone_MechanoidShutdown(Find.CurrentMap.zoneManager);
		}
	}

}
