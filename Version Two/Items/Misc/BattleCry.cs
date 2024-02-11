using System;
using System.Runtime.CompilerServices;
using Lunarse;
using Lunarse.LunPlayer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace Lunarse.Items.Misc
{
	public class BattleCry : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[base.Type] = 1;
		}
		public override void SetDefaults()
		{
			base.Item.width = 28;
			base.Item.height = 38;
			base.Item.value = Item.sellPrice(0, 0, 2, 0);
			base.Item.rare = ItemRarityID.Pink;
			base.Item.useAnimation = 30;
			base.Item.useTime = 30;
			base.Item.useStyle = ItemUseStyleID.Shoot;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		/*
		public static void GenerateText(bool isBattle, Player player, bool cry)
		{
			string cryToggled = isBattle ? "Battle" : "Calming";
			string toggle = cry ? "activated" : "deactivated";
			string punctuation = isBattle ? "!" : ".";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(10, 4);
			defaultInterpolatedStringHandler.AppendFormatted(cryToggled);
			defaultInterpolatedStringHandler.AppendLiteral(" Cry ");
			defaultInterpolatedStringHandler.AppendFormatted(toggle);
			defaultInterpolatedStringHandler.AppendLiteral(" for ");
			defaultInterpolatedStringHandler.AppendFormatted(player.name);
			defaultInterpolatedStringHandler.AppendFormatted(punctuation);
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			Color color = isBattle ? new Color(255, 0, 0) : new Color(0, 255, 255);
			LunarseUtils.PrintText(text, color);
		}
		*/
		public static void GenerateText(bool isBattle, Player player, bool cry)
		{
    		string cryToggled = isBattle ? "Battle" : "Calming";
    		string toggle = cry ? "activated" : "deactivated";
    		string punctuation = isBattle ? "!" : ".";
    
    		string text = $"{cryToggled} Cry {toggle} for {player.name}{punctuation}";

    		Color color = isBattle ? new Color(255, 0, 0) : new Color(0, 255, 255);
    		LunarseUtils.PrintText(text, color);
		}

		private void ToggleCry(bool isBattle, Player player, ref bool cry)
		{
			cry = !cry;
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				BattleCry.GenerateText(isBattle, player, cry);
				return;
			}
		}
		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				LunarsePlayer modPlayer = player.Lunarse();
				if (player.altFunctionUse == 2)
				{
					if (modPlayer.BattleCry)
					{
						this.ToggleCry(true, player, ref modPlayer.BattleCry);
					}
					this.ToggleCry(false, player, ref modPlayer.CalmingCry);
				}
				else
				{
					if (modPlayer.CalmingCry)
					{
						this.ToggleCry(false, player, ref modPlayer.CalmingCry);
					}
					this.ToggleCry(true, player, ref modPlayer.BattleCry);
				}
			}
			return new bool?(true);
		}
		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient(ItemID.BattlePotion, 15).AddTile(TileID.DemonAltar).Register();
		}
	}
}
