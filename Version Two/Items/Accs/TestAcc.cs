using System;
using Lunarse;
using Lunarse.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using System.Timers;

namespace Lunarse.Items.Accs
{
	public class TestAcc : ModItem, ILocalizedModType, IModType
	{
		public static readonly int AdditiveDamageBonus = 25;
		public static readonly int MultiplicativeDamageBonus = 10;
		public static readonly int BaseDamageBonus = 25;
		public static readonly int FlatDamageBonus = 25;
		public static readonly int MeleeCritBonus = 10;
		public static readonly int AttackSpeedBonus = 10;
		public static readonly int ArmorPenetration = 500;
		public static readonly int AdditiveCritDamageBonus = 10;
		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(AdditiveDamageBonus, MultiplicativeDamageBonus, BaseDamageBonus, FlatDamageBonus, MeleeCritBonus, AttackSpeedBonus, ArmorPenetration, AdditiveCritDamageBonus);
        public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(6, 12, false));
			ItemID.Sets.AnimatesAsSoul[base.Type] = true;
		}

		public override void SetDefaults()
		{
            base.Item.width = (base.Item.height = 32);
			base.Item.value = 10000;
			base.Item.rare = ItemRarityID.Orange;
			base.Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			IEntitySource source = player.GetSource_Accessory(base.Item, null);
			int damage = player.statLifeMax2 / 10;
			if (Main.GameUpdateCount % 600 == 0) {
				Projectile.NewProjectile(source, player.Center, player.Center * 1.2f, ModContent.ProjectileType<DProj>(), 200, 10f, player.whoAmI, 0f, 0f , 0f);
			}
			// Increase max health for each 100 max health
            player.statLifeMax2 += (int)(player.statLifeMax2 / 100) * 10;
			if ((float)player.statLife >= (float)player.statLifeMax2 * 0.50f)
			{
				player.luck += 1.0f;
				player.manaCost -= 0.55f;
				player.maxRunSpeed += 1.7f;
				player.runAcceleration *= 1.5f;
				player.runSlowdown = 0.2f;
				Player.jumpHeight += 8;
				player.jumpSpeedBoost += 1.3f;
				player.GetDamage(DamageClass.Generic) += AdditiveDamageBonus / 100f;
				player.GetDamage(DamageClass.Generic) *= 1 + MultiplicativeDamageBonus / 100f;
				player.GetDamage(DamageClass.Generic).Base += BaseDamageBonus;
				player.GetCritChance(DamageClass.Generic) += MeleeCritBonus;
				player.GetDamage(DamageClass.Generic).Flat += FlatDamageBonus;
				player.GetAttackSpeed(DamageClass.Generic) += AttackSpeedBonus / 100f;
				player.GetArmorPenetration(DamageClass.Generic) += ArmorPenetration;
				player.maxTurrets += 5;
				player.maxMinions += 10;
				player.magicCuffs = true;
				player.manaFlower = true;
				player.manaMagnet = true;
				player.noKnockback = true;
				player.manaRegen += 25;
				player.lifeSteal += 10;
				player.buffImmune[146] = true;
				player.buffImmune[86] = true;
				player.buffImmune[13] = true;
				player.buffImmune[157] = true;
				player.buffImmune[106] = true;
			}
		}
	}
}
