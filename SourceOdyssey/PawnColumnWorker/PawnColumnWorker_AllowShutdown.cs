using RimWorld;
using UnityEngine;
using Verse;

namespace WVC_WorkModes
{
	public class PawnColumnWorker_AllowShutdown : PawnColumnWorker_Icon
	{
		public override bool VisibleCurrently
		{
			get
			{
				return true;
			}
		}

		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.GetMechSettings()?.AllowShutdownIcon;
		}

		protected override Color GetIconColor(Pawn pawn)
		{
			return Color.white;
		}

		protected override string GetIconTip(Pawn pawn)
		{
			return "WVC_WorkModes_AllowShutdownDesc".Translate();
		}

		protected override void ClickedIcon(Pawn pawn)
		{
			CompMechSettings compMechSettings = pawn.GetMechSettings();
			compMechSettings.allowShutdown = !compMechSettings.allowShutdown;
		}
	}

}
