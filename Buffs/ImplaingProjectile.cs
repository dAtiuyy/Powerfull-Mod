using System;
using Terraria.ModLoader;

namespace med.Buffs
{
	public class ImplaingProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		public bool CanImpale;
		public int damagePerImpaler;
	}
}
