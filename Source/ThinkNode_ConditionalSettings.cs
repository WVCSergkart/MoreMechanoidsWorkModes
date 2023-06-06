// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Linq;

namespace WVC
{
	public class ThinkNode_ConditionalSettings : ThinkNode_Conditional
	{
		public string settingName;

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
