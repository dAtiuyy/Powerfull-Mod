using System;
using Terraria;
using Terraria.ModLoader;

namespace med.Buffs;

public class ForkTag : ModBuff
{
    public override void SetStaticDefaults()
    {
        base.DisplayName.SetDefault("Missile Procing");
        base.Description.SetDefault("Minions will cause missiles to fire.");
        Main.debuff[base.Type] = true;
    }
}