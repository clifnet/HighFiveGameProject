﻿using Game.Control.PersonSystem;
using Game.View;
using UnityEngine;

namespace Game.Control.BattleEffectSystem
{
    /// <summary>
    /// 数字效果
    /// </summary>
    public class DamageNumberEffect : AbstractBattleEffect
    {
        private int damageNum;
        private float time;
        private int size;
        private Color color;
        private float yOffect;
        private int dir;
        public DamageNumberEffect(int damageNum, float yOffect, Color color, int dir, float time = 1f, int size = 24)
        {
            this.damageNum = damageNum;
            this.size = size;
            this.time = time;
            this.color = color;
            this.yOffect = yOffect;
            this.dir = dir;
        }

        public override void Execute(AbstractPerson ap)
        {
            new NumberTipUI(this.damageNum, this.yOffect, this.size, this.color, ap.obj.transform, this.dir, this.time);
            this.Release(ap);
        }
    }
}
