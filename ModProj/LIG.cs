using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MyMod.ModProj
{
    public class LIG : ModProjectile
    {
		public int length = 15;//个人推荐15，太大了不好，太小了也不好
		public override void SetDefaults()
		{
			Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 20;
            Projectile.friendly = true;
			Projectile.light = 0.5f;
			Projectile.penetrate = -1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = length;
			ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 11451;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.friendly = false;
		}
		public float s = 0;//控制闪电的宽度的
		public override void AI()
		{
			NPC npc = Main.npc[(int)Projectile.ai[0]];//获取npc
			Projectile.Center = npc.Center - Projectile.velocity.RotatedByRandom(0.7f) * length * 100;//随机旋转向量，让闪电起始点在不同位置，如果你不要就把RotatedByRandom(0.7f)删了
			s = MathHelper.Lerp(s, 1f, 0.4f);
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				float n = (float)(i * (i - Projectile.oldPos.Length + 1));//这样写可以让起始点和末端不偏移，同时又能让中间部分偏移
				if (Projectile.oldPos[length - 1] == Vector2.Zero)//取点部分
				{
					Projectile.oldPos[i] = Projectile.Center + (npc.Center - Projectile.Center).SafeNormalize(Vector2.One) * (float)(i + 1) * ((npc.Center - Projectile.Center).Length() / length) + new Vector2(-Projectile.velocity.Y, Projectile.velocity.X) * Utils.NextFloatDirection(Main.rand) * n;
				}
			}
			if(Projectile.oldPos[length - 1] != Vector2.Zero)
            {
				Projectile.Center = Projectile.oldPos[length - 1];//让坐标移动，省的重写碰撞箱
			}
		}
		//没啥好说的
		public override bool PreDraw(ref Color lightColor)
		{
            Texture2D Tex2D = BloodSoulUtils.GetTexture("Extra_196").Value;
            List<CustomVertexInfo> bars = new List<CustomVertexInfo>();
            for (int i = 1; i < Projectile.oldPos.Length; i++)
			{
				if (Projectile.oldPos[i] == Vector2.Zero) break;
				Vector2 normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
				normalDir = new Vector2(-normalDir.Y, normalDir.X).SafeNormalize(Vector2.One);
				float w = 1.2f;
				float width = 30f;
				bars.Add(new CustomVertexInfo(Projectile.oldPos[i] + normalDir * width * s - Main.screenPosition, new Color(100, 255, 255, 0) * (Projectile.timeLeft / 20f), new Vector3((float)i / Projectile.oldPos.Length, 1f, w)));
				bars.Add(new CustomVertexInfo(Projectile.oldPos[i] + normalDir * -width * s - Main.screenPosition, new Color(100, 255, 255, 0) * (Projectile.timeLeft / 20f), new Vector3((float)i / Projectile.oldPos.Length, 0f, w)));
			}
			List<CustomVertexInfo> Vx = new List<CustomVertexInfo>();
			if (bars.Count > 2)
			{
				Vx.Add(bars[0]);
				Vx.Add(bars[1]);
				Vx.Add(bars[2]);
				for (int i2 = 0; i2 < bars.Count - 2; i2 += 2)
				{
					Vx.Add(bars[i2]);
					Vx.Add(bars[i2 + 2]);
					Vx.Add(bars[i2 + 1]);

					Vx.Add(bars[i2 + 1]);
					Vx.Add(bars[i2 + 2]);
					Vx.Add(bars[i2 + 3]);
				}
			}
			Main.graphics.GraphicsDevice.Textures[0] = Tex2D;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
			return false;
        }
    }
}
