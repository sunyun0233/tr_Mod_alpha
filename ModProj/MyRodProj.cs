
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
    public class MyRodProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Terrarian;
        //���п��Խ���Ļ�Ĳ�������ָ��ԭ���Ӧ·������ͼ���ǳ����ã������Ͳ���׼��ͼƬ��
        //����·����ʽ��Terraria/Images/Item_ �ȵȣ�������ο������������ͼ������
        Player player => Main.player[Projectile.owner];
        //�������ɵ�Ļʱ�����owner������Ӧ�����
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;//����Ϊ������鳤��
            //ע��ϸ���ε�Ļǧ�����պ�«��ư�ѳ�����ͼ������Ϊ��ײ���ǹ̶��ģ�����������ͼ����ת����ת
            Projectile.friendly = true;//�ѷ���Ļ                                          
            Projectile.tileCollide = false;//false����������ǽ
            Projectile.timeLeft = 20;//��ɢʱ��
            Projectile.aiStyle = -1;//��ʹ��ԭ��AI
            Projectile.DamageType = DamageClass.Magic;//ħ���˺�
            Projectile.penetrate = 1;//��ʾ�ܴ�͸����
            Projectile.ignoreWater = true;//����Һ��
            base.SetDefaults();
        }
        public override void AI()
        {
            if (player.channel && Projectile.ai[0] == 0)//ֻ����ҳ�����ס���ʱ����ai0û���ĵ�ʱ����
            {
                Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero)//���ǻ�ȡ��Ļ�����ĵ�λ���� 
                    * MathHelper.Min(Vector2.Distance(Projectile.Center, Main.MouseWorld), 15f);
                //ÿ֡���豾��Ļһ����������15fÿ֡���ٶ�,�������ڿ������ʱ������ô����ٶ��߻᲻ͣ�鴤��
                //������Ҫ��min(����ȡ��С)��������������ѹ������
                player.itemAnimation = player.itemTime = 10; //�̶����ʹ��ʱ�䣬�����ɿ����10֮֡�����ʹ�����
                Projectile.timeLeft = 182;//�̶���Ļ��ɢ����ʱ�������ɿ�����Ļ��������180֡Ҳ����3��

                //������ʹ����ҵķ��ȷ�����ŵ�Ļ��Ч��������tr������ַ����ж������Ǳ���Ҫ���������
                if(player.direction == 1)//�����ҳ����ұ�
                {
                    player.itemRotation = (Projectile.Center - player.Center).ToRotation();//��ȡ��ҵ���Ļ�����ķ���
                }
                else
                {
                    player.itemRotation = (Projectile.Center - player.Center).ToRotation() + 3.1415926f;//��֮��Ҫ+��Ȧ
                }
            }
            else
            {
                Projectile.ai[0] = 1;//�����������ˣ��Ǿ͸ĵ�ai0����������ִ�б���ס��AI��ת������ֱ���˶���ȥ
                if (Projectile.velocity.Length() < 1) Projectile.velocity = (Projectile.Center - player.Center).SafeNormalize(
                    Vector2.Zero) * 10;//�����Ļ�ٶ�̫С����ô������ֱ�ӷ����ȥ����
                Projectile.velocity *= 1.055f;//���ϼ���
            }
            //�������ֳֵ�Ļ�ĺ���,�������Ӿ�Ч����AI,ʹ��forѭ������������������
            for (int i = 0; i < 3; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch, 
                    0, 0, 0, default, 2);
                d.noGravity = true;//������������
                                   //Ϊ���õ�Ļ���������ÿ�������һЩ�����Ǳ�Ҫ��,��ô����һ����ɫ���Ӱ�
                d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch,
                    0, 0, 0, default, 2);
                d.noGravity = true;//������������
                                   //���ģ��������ӵķ������ڵ�һ�������͵ڶ�������������ɵ�һ�����������λ���������ӣ������ص���
            }
            base.AI();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //Ϊ���õ�Ļ���д���У����ǻ�Ҫ�ڻ��к�����дһ�����ɱ�ը�ĵ�Ļ
            Projectile.NewProjectile(Projectile.GetSource_FromAI()//����Դһ�㲻֪����ʲô��ʱ�����ôд������û��
                , Projectile.Center, Vector2.Zero,//��Ϊ��ը��Ļ�Ǿ�ֹ�������ٶ�Ϊ0
                ProjectileID.DaybreakExplosion,//����һ�������ı�ըЧ��
                0, //�˺�Ϊ0��Ϊ���ǲ�����������˺�
                0, player.whoAmI);
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
    public class MyGunProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";//��Ϊ��α��������ֱ����͸����ͼ����
        //���п��Խ���Ļ�Ĳ�������ָ��ԭ���Ӧ·������ͼ���ǳ����ã������Ͳ���׼��ͼƬ��
        //����·����ʽ��Terraria/Images/Item_ �ȵȣ�������ο������������ͼ������
        Player player => Main.player[Projectile.owner];
        //�������ɵ�Ļʱ�����owner������Ӧ�����
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;//����Ϊ������鳤��
            //ע��ϸ���ε�Ļǧ�����պ�«��ư�ѳ�����ͼ������Ϊ��ײ���ǹ̶��ģ�����������ͼ����ת����ת
            Projectile.friendly = true;//�ѷ���Ļ                                          
            Projectile.tileCollide = false;//false����������ǽ
            Projectile.timeLeft = 20;//��ɢʱ��
            Projectile.aiStyle = -1;//��ʹ��ԭ��AI
            Projectile.DamageType = DamageClass.Ranged;//Զ��
            Projectile.penetrate = 1;//��ʾ�ܴ�͸����
            Projectile.ignoreWater = true;//����Һ��
            base.SetDefaults();
        }
        public override bool? CanDamage()//ע�⣡��Ϊ����һ��ֻ���������ӵ���������Ļ����������˺�
        {
            return false;//�����Ҫ����false
        }
        public override void AI()
        {
            if (player.channel)//��Ұ�סʱִ��
            {          
                //��Ϊ����һ��ǹ����ϣ������������泯ǹָ�ŵİ�ߣ�����д���´���
                player.direction = (Main.MouseWorld - player.Center).X > 0 ? 1 : -1;//��������ʵ����       
                //������ʹ����ҵ�����������ŵ�Ļ��Ч��
                if (player.direction == 1)//�����ҳ����ұ�
                {
                    player.itemRotation = (Main.MouseWorld - player.Center).ToRotation();//��ȡ��ҵ���Ļ�����ķ���
                }
                else
                {
                    player.itemRotation = (Main.MouseWorld - player.Center).ToRotation() + 3.1415926f;//��֮��Ҫ+��Ȧ
                }
               
                Projectile.timeLeft = 2;//��Ϊ�����Ե�Ļ��Ϊ�������������ֺ����������ʧ!
                player.itemTime = player.itemAnimation = 20;//ͬ����������Ҫ����ұ���ʹ��״̬
                //������������Ҫ��Ϊ
                //α������Ļ��Ҫʱ�̶���������ϣ�����
                Projectile.Center = player.Center;
                //��ů�������͵����������Ӱ��������ǿ������ʱ�����ӣ������Ҫ���ܼ�ʱ��,�������ǲ���ai[0]
                Projectile.ai[0]++;//������ai0ÿ֡��һ
                int Jiange;//���Ƕ���һ�������ֲ�������������Ϊ���ʹ��
                if(Projectile.ai[0] < 120)//ǰ�����ڵĹ������
                {
                    Jiange = 50;//�������Ϊ50
                }
                else if(Projectile.ai[0] < 210)//��2~3.5��Ĺ������
                {
                    Jiange = 36;//�������Ϊ36
                }
                else if(Projectile.ai[0] < 270)//��3.5~4.5��Ĺ������
                {
                    Jiange = 22;//�������Ϊ22
                }
                else //��֮��Ĺ������
                {
                    Jiange = 10;//�������Ϊ10
                }
                //�ֲ����������ˣ�����������ʵ��ÿ����ô��������һ�ε�Ļ��,������ai[1]��Ϊ��ʱ
                Projectile.ai[1]++;
                if(Projectile.ai[1] > Jiange)
                {
                    Projectile.ai[1] = 0;//�������������
                    player.PickAmmo(player.HeldItem, out int type, out float speed, out int damage, out float knockback,
                        out int ammo);//��ηǳ���Ҫ���ǻ�ȡ������������ӵ������Լ��˺��ٶȵȱر����Եķ���
                    //����������ѭ������������䲻����ɢ���Ľ̳�
                    for(int i = 0; i < 6; i++)//ѭ��ִ��6��
                    {
                        float r = Main.rand.NextFloat(-0.33f, 0.33f);//һ�����ƫ����
                        //���䵯Ļ
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center,
                 (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero).RotatedBy(r) * speed,//����귢���ٳ����ٶ�
                 type,//��ȡ��pickammo��out������type������Եõ������ӵ���Ӧ�ĵ�Ļ
                  damage,//ͬ��
                  knockback,//ͬ��
                  player.whoAmI//ownerΪ�������
                  );
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item36, player.Center);//�����ǲ�����Ч��
                }
            }

            base.AI();
        }
    }
}