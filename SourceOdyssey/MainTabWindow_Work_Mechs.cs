using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WVC_WorkModes.Odyssey;

namespace WVC_WorkModes
{

	public class MainTabWindow_Work_Mechs : MainTabWindow_Work
	{

		protected override PawnTableDef PawnTableDef
		{
			get
			{
				if (def is MainButtonDef mainButtonDef && mainButtonDef.pawnTableDef != null)
				{
					return mainButtonDef.pawnTableDef;
				}
				return  base.PawnTableDef;
			}
		}

		protected override IEnumerable<Pawn> Pawns => Find.CurrentMap.mapPawns.AllPawnsSpawned.Where((Pawn pawn) => pawn.IsColonyMech);

	}

}
