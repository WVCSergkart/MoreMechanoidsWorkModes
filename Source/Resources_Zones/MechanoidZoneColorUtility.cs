// RimWorld.JobGiver_GetEnergy_SelfShutdown
using System.Collections.Generic;
using UnityEngine;

namespace WVC_WorkModes
{

    public static class MechanoidZoneColorUtility
	{

		public static List<Color> mechanoidZoneColors;

		private static int nextMechanoidZoneColorIndex;

		static MechanoidZoneColorUtility()
		{
			mechanoidZoneColors = new List<Color>();
			nextMechanoidZoneColorIndex = 0;
			foreach (Color item4 in MechanoidZoneColors())
			{
				Color item2 = new(item4.r, item4.g, item4.b, 0.09f);
				mechanoidZoneColors.Add(item2);
			}
		}

		public static Color NextMechanoidZoneColor()
		{
			Color result = mechanoidZoneColors[nextMechanoidZoneColorIndex];
			nextMechanoidZoneColorIndex++;
			if (nextMechanoidZoneColorIndex >= mechanoidZoneColors.Count)
			{
				nextMechanoidZoneColorIndex = 0;
			}
			return result;
		}

		private static IEnumerable<Color> MechanoidZoneColors()
		{
			// yield return Color.Lerp(new Color(105, 0, 0), Color.gray, 0.5f);
			// yield return Color.Lerp(new Color(129, 120, 102), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0, 125, 125), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(125, 125, 0), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(125, 0, 125), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(125, 125, 125), Color.gray, 0.5f);
			// yield return Color.Lerp(new Color(138, 111, 58), Color.gray, 0.5f);
			// yield return Color.Lerp(new Color(153, 102, 102), Color.gray, 0.5f);
			// yield return Color.Lerp(new Color(102, 102, 102), Color.gray, 0.5f);
		}

	}

}
