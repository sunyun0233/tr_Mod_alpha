
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
    public class HUXING2 : ModProjectile //�����˶���ѧ
    {   
        Player player => Main.player[Projectile.owner];
        //�������ɵ�Ļʱ�����owner������Ӧ�����
        public override void SetStaticDefaults()//������ÿ�μ���ģ��ʱִ��һ�Σ����ڷ��侲̬����
        {
            Main.projFrames[Type] = 1;//���֡ͼ�ж���֡�������
            ProjectileID.Sets.TrailingMode[Type] = 2;//��һ�ֵ2���Լ�¼�˶��켣�ͷ�������������β��
            ProjectileID.Sets.TrailCacheLength[Type] = 10;//��һ������¼�Ĺ켣�����׷�ݵ�����֡��ǰ(ע�����ֵȡ����)
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;//��һ�����Ļ������Ļ����پ������ڿ��Ի���
                                                                //���ڳ����ε�Ļ����
            
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;//����Ϊ������鳤��
            //ע��ϸ���ε�Ļǧ�����պ�«��ư�ѳ�����ͼ������Ϊ��ײ���ǹ̶��ģ�����������ͼ����ת����ת
            Projectile.friendly = true;//�ѷ���Ļ                                          
            Projectile.tileCollide = false;//false����������ǽ
            Projectile.timeLeft = 1220;//��ɢʱ��
            Projectile.aiStyle = -1;//��ʹ��ԭ��AI
            Projectile.DamageType = DamageClass.Ranged;//ħ���˺�
            Projectile.penetrate = 1;//��ʾ�ܴ�͸����
            Projectile.ignoreWater = true;//����Һ��
            base.SetDefaults();
        }
        Vector2 coord = Vector2.Zero;//����һ�������ֶ�
        float timer = 0;//����һ����ʱ���ֶ�
        float startVel = 0;//Ԥ���ٶȷ����ֶ�
        public override void AI()//�����ص�����AI
        {
            //AI�ڵĶ����ڷ���ͣ״̬�£�ÿִ֡��һ�Σ��ǵ�Ļ������Ϊ�Ļ���
            Projectile.rotation = Projectile.velocity.ToRotation();
            //�����������˶�,�����κ�AI������£���Ļ�ٶ��ǲ����ģ����������˶�����Ҫ�޸��ٶ�
            if (coord == Vector2.Zero)//��Ļ���ɵ���һ�̣�����ֶ�Ĭ��Ϊ0���������Ǹ�������Ļ��ʱ������
            {
                startVel = Projectile.velocity.ToRotation();//��Ԥ������ٶȷ���
                Projectile.velocity *= 0;//�������ǲ����õ�Ļ�г��ٶ���
                coord = Projectile.Center;
            }
            else //֮��coord�Ѿ������赯Ļ�ĳ�ʼ�����ˣ���ʱ�Ϳ����õ�Ļ��������
            {
                timer++;//���ǻ�����Ҫһ����ʱ���ģ��������ƽǶ�
                float rotation = timer * 0.15f;//�����Ը�С����Ϊ�Ƕ�ʹ��
                float Range = timer * 5; //Բ�İ뾶
                Vector2 originPos = player.Center;//�����������Ϊԭ��Ϊ����
                Vector2 circlePos = rotation.ToRotationVector2() * Range;
                originPos += circlePos;
                Projectile.velocity = originPos - Projectile.Center;
                //���ˣ������˶��Ͷ���������
                //���棬�ҽ�������Ϸ�и��㷺ʹ�õ�һЩ�˶�����
            }
        }


        public override bool PreDraw(ref Color lightColor)//predraw����false���ɽ���ԭ�����
        {
            //ͬʱ����Ҫ���еĻ�����������д�ͺ�
            
            Texture2D texture = TextureAssets.Projectile[Type].Value;//��������Ļ�Ĳ���
            Rectangle rectangle = new Rectangle(//��Ϊ�ֶ�������Ҫ�Լ���д֡ͼ��,����Ҫ�������
                0,//���������Ͻǵ�ˮƽ����(��0�ͺ�)
                texture.Height / Main.projFrames[Type] * Projectile.frame,//������Ͻǵ��������� 
                texture.Width, //��Ŀ��(���ʿ�ȼ���)
                texture.Height / Main.projFrames[Type]//��ĸ߶ȣ��ò��ʸ߶ȳ���֡���õ���֡�߶ȣ�
                );

            //Ҫ������β������Ҫ����һ��forѭ����䣬��0һֱ�ߵ��켣ĩ��
            //�������ǽ���һ���ܲ����������ӻ��Ƶİ취��A=0��
            Color MyColor = Color.White; MyColor.A = 0;//��A=0��Ϊ����ֱ�ӵ�����ɫ
            for (int i = 0;i < ProjectileID.Sets.TrailCacheLength[Type]; i++)//ѭ������С�ڹ켣����
            {
                float factor = 1 - (float)i / ProjectileID.Sets.TrailCacheLength[Type];
                //����һ�����µ�����1�𽥼��ٵ�0�ı���������i = 0ʱ��factor = 1
                Vector2 oldcenter = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                //���ڹ켣ֻ�ܼ�¼��Ļ��ײ�����Ͻ�λ�ã�����Ҫ�ֶ����ϵ�Ļ���һ������ȡ����
                Main.EntitySpriteDraw(texture, oldcenter, rectangle, MyColor * factor,//��ɫ�𽥱䵭
                    Projectile.oldRot[i],//��Ļ�켣�ϵ������ķ���
                    new Vector2(texture.Width / 2, texture.Height / 2 / Main.projFrames[Type]),
                     new Vector2(1, 1),
                     SpriteEffects.None, 0);
            }
            //����tr��������ִ�е��Ȼ��ƣ�����Ҫ���Ӱ�����ǵ��������棬��Ҫ��д��Ӱ����

            Main.EntitySpriteDraw(  //entityspritedraw�ǵ�Ļ��NPC�ȳ��õĻ��Ʒ���
                texture,//��һ�������ǲ���
                Projectile.Center - Main.screenPosition,//ע�⣬����ʱ��λ��������Ļ���Ͻ�Ϊ0��
                //���Ҫ�õ�Ļ���������ȥ��Ļ���Ͻǵ�����
                rectangle,//��������������֡ͼѡ����
                Color.White,//���ĸ���������ɫ�������������Դ���lightcolor�������ܵ���Ȼ����Ӱ��
                Projectile.rotation,//�������������ͼ��ת����
                new Vector2(texture.Width / 2, texture.Height / 2 / Main.projFrames[Type]),
                //��������������ͼ����ԭ������꣬����дΪ��ͼ��֡���������꣬������ת�����Ŷ���Χ������
                new Vector2(1, 1),//���߸����������ţ�X��ˮƽ���ʣ�Y����ֱ����
                SpriteEffects.None,
                //�ڰ˸�����������ͼƬ��תЧ������Ҫ�ֶ��ж�������spriteeffects
                0//�ھŸ������ǻ��Ʋ㼶������0�����ˣ���̫��ʹ
                );
            
            return false;//return false��ֹ�Զ�����
        }
    }
   
}