using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Lunarse.Buffs;

namespace Lunarse.LunPlayer
{
	public class LunarsePlayer : ModPlayer
	{   
    public override void Initialize()
	{}
    public override void SaveData(TagCompound tag)
	{
	if (this.BattleCry)
	{
		tag.Add("BattleCry" + base.Player.name, true);
	}
	if (this.CalmingCry)
	{
		tag.Add("CalmingCry" + base.Player.name, true);
	}
    }
    public override void LoadData(TagCompound tag)
	{
        this.BattleCry = tag.ContainsKey("BattleCry" + base.Player.name);
		this.CalmingCry = tag.ContainsKey("CalmingCry" + base.Player.name);
    }
    public override void ResetEffects()
	{
        this.SummonBuff = false;
    }
    public override void PostUpdateEquips()
	{
	}
    internal void UpdatePotions(Player player)
	{
	}
    public bool SummonBuff;
    public bool rBrain;
    public bool customBurn;
    public bool UltBattler;
    public float SpawnRateMultiplier = 1f;
	public float MaxSpawnsMultiplier = 1f;
	public bool UltPeaceful;
	public bool BattleCry;
	public bool CalmingCry;
    }
}
