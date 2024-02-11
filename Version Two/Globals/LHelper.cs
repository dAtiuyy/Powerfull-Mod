using System;
using System.Globalization;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Lunarse.Globals
{
	// Token: 0x0200021B RID: 539
	public static class LHelper
	{
		public static Vector2 PolarVector(float radius, float theta)
		{
			return new Vector2((float)Math.Cos((double)theta), (float)Math.Sin((double)theta)) * radius;
		}
    }
}