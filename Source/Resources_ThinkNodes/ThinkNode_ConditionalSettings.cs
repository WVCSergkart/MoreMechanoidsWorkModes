// RimWorld.JobGiver_GetEnergy_SelfShutdown
using System.Linq;
using Verse;
using Verse.AI;
using WVC;

namespace WVC_WorkModes
{
    public class ThinkNode_ConditionalSettings : ThinkNode_Conditional
	{
		public string settingName;

		// public override ThinkNode DeepCopy(bool resolve = true)
		// {
			// ThinkNode_ConditionalSettings obj = (ThinkNode_ConditionalSettings)base.DeepCopy(resolve);
			// obj.invert = invert;
			// obj.settingName = settingName;
			// return obj;
		// }

		protected override bool Satisfied(Pawn pawn)
		{

			if (WVC_MMWM.settings.GetEnabledSettings.Contains(settingName) == true)
			{
				return true;
			}
			return false;
		}
	}
}
