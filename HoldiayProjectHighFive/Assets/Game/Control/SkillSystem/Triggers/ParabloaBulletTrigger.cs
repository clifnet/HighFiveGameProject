﻿using System;
using Game.Const;
using Game.Control.PersonSystem;
using UnityEngine.Assertions;
using Game.Model.SpriteObjSystem;
using ReadyGamerOne.Data;
using UnityEngine;
namespace Game.Control.SkillSystem
{
    class ParabloaBulletTrigger : AbstractSkillTrigger
    {
        private string resName;
        private int damage;
        private float maxLife;
        private float shootSpeed;
        private float timeToTarget;
        private Vector3 offect;

        public override void LoadTxt(string args)
        {
            //shootspeed timeToTarget offect.x.y.z damage maxlife 
            var strs = args.Trim().Split(TxtManager.SplitChar);
            Assert.IsTrue(strs.Length >= BasePropertyCount+8);


            this.resName = strs[BasePropertyCount].Trim();
            this.shootSpeed = Convert.ToSingle(strs[BasePropertyCount+1].Trim());
            this.timeToTarget = Convert.ToSingle(strs[BasePropertyCount+2].Trim());
            this.offect = new Vector3
            {
                x = Convert.ToSingle(strs[BasePropertyCount+3].Trim()),
                y = Convert.ToSingle(strs[BasePropertyCount+4].Trim()),
                z = Convert.ToSingle(strs[BasePropertyCount+5].Trim())
            };
            this.damage = Convert.ToInt32(strs[BasePropertyCount+6].Trim());
            this.maxLife = Convert.ToSingle(strs[BasePropertyCount+7].Trim());
            base.LoadTxt(string.Join(TxtManager.SplitChar.ToString(), strs,0, this.BasePropertyCount));
            
        }
        public override void Execute(AbstractPerson self)
        {
            new ParabloaBullet(this.damage, this.shootSpeed, this.timeToTarget, self.Pos + this.offect,
                self.Pos, self, Const.DirPath.BulletDir+this.resName, this.maxLife);
        }
        public override string Sign
        {
            get
            {
                return TxtSign.paraBullet;
            }
        }
    }
}
