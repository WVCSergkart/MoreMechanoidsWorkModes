using RimWorld;
using UnityEngine;
using Verse;

namespace WVC_WorkModes
{
	public class PawnColumnWorker_RestrictShutdown : PawnColumnWorker_Icon
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
			return GraphicCache.Icon_RestrictShutdown.Texture;
		}

		protected override Color GetIconColor(Pawn pawn)
		{
			return Color.white;
		}

		protected override string GetIconTip(Pawn pawn)
		{
			return "WVC_WorkModes_RestrictZoneByGroupDesc".Translate();
		}

		protected override void ClickedIcon(Pawn pawn)
		{
			CompMechSettings compMechSettings = pawn.GetMechSettings();
			compMechSettings.restrictZoneByGroup = !compMechSettings.restrictZoneByGroup;
		}

	}

}
