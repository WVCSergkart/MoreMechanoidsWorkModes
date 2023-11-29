// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;
using WVC;

namespace WVC_WorkModes
{

	public class ThinkNode_ConditionalEnabledWorkTypes : ThinkNode_Conditional
	{

		public List<WorkTypeDef> allOfMechEnabledWorkTypes;

		public List<WorkTypeDef> anyOfMechEnabledWorkTypes;

		// public override ThinkNode DeepCopy(bool resolve = true)
		// {
			// ThinkNode_ConditionalEnabledWorkTypes obj = (ThinkNode_ConditionalEnabledWorkTypes)base.DeepCopy(resolve);
			// obj.invert = invert;
			// obj.allOfMechEnabledWorkTypes = allOfMechEnabledWorkTypes;
			// obj.anyOfMechEnabledWorkTypes = anyOfMechEnabledWorkTypes;
			// return obj;
		// }

		protected override bool Satisfied(Pawn pawn)
		{
			List<WorkTypeDef> mechWorkTypes = pawn.def.race.mechEnabledWorkTypes;
			if (AllWorkTypes(mechWorkTypes, allOfMechEnabledWorkTypes) || AnyWorkTypes(mechWorkTypes, anyOfMechEnabledWorkTypes))
			{
				return true;
			}
			return false;
		}

		public static bool AllWorkTypes(List<WorkTypeDef> mechWorkTypes, List<WorkTypeDef> workTypes)
		{
			if (!workTypes.NullOrEmpty() && !mechWorkTypes.NullOrEmpty())
			{
				for (int i = 0; i < workTypes?.Count; i++)
				{
					if (!mechWorkTypes.Contains(workTypes[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static bool AnyWorkTypes(List<WorkTypeDef> mechWorkTypes, List<WorkTypeDef> workTypes)
		{
			if (!workTypes.NullOrEmpty() && !mechWorkTypes.NullOrEmpty())
			{
				for (int i = 0; i < workTypes?.Count; i++)
				{
					if (mechWorkTypes.Contains(workTypes[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

	}

}
