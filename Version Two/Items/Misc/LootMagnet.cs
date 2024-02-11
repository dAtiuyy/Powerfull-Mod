using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Lunarse.Items.Misc
{
	public class LootMagnet : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			base.Item.width = 26;
			base.Item.height = 28;
			base.Item.rare = ItemRarityID.Red;
			base.Item.holdStyle = 2;
		}
		public override void HoldItem(Player player)
		{
			for (int i = 0; i < 400; i++)
			{
				Item val = Main.item[i];
				if (val.active && val.noGrabDelay == 0 && ItemLoader.CanPickup(val, player))
				{
					val.beingGrabbed = true;
					Vector2 val2 = player.Center - val.Center;
					Entity entity = val;
					Vector2 vector = val.velocity * 4f;
					Vector2 vector2 = val2;
					float num = 20f;
					Vector2 vector3 = val2;
					entity.velocity = (vector + vector2 * (num / vector3.Length())) * 0.2f;
				}
			}
		}
	}
}
