﻿using Game.Control.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Control.SkillSystem
{
    /// <summary>
    /// 动画触发器
    /// </summary>
    public class AnimationTrigger : AbstractSkillTrigger
    {
        private string animationName;
        private float speed;

        /// <summary>
        /// 初始化子类，这里要通过从args中获取信息，完成子类特有信息的初始化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        public override void Init(string args)//type,int id,float startTime,float lastTime, string args)
        {
            var strs = args.Split('|');
            Assert.IsTrue(strs.Length >= 6);
            this.animationName = strs[4].Trim();
            this.speed = Convert.ToSingle(strs[5].Trim());
            this.LastTime /= this.speed;
            base.Init(string.Join("|", strs, 0, 4));
        }

        /// <summary>
        /// 触发函数
        /// </summary>
        /// <param name="self">挂载每个AbstractPerson身上的脚本，也可以识别的东西，这里以后可以换</param>
        /// <returns></returns>
        public override void Execute(AbstractPerson self)
        {
            //触发器执行具体代码
            //这里也可以由延时调用，比如技能开始 0.15s后开始动画/音效/位移，

            if (self == null)
            {
                throw new Exception("SkillCore为空");
            }

            var animator = GameAnimator.GetInstance(self.obj.GetComponent<Animator>());

            if (animator == null)
            {
                throw new Exception(self.name + "没有Animator组件");
            }
            animator.speed = this.speed;
            //            Debug.Log("播放动画了");
            animator.Play(Animator.StringToHash(this.animationName), AnimationWeight.High);
        }
    }
}