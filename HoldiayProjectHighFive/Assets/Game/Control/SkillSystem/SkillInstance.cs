﻿using Game.Control.PersonSystem;
using Game.Script;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Control.SkillSystem
{
    /// <summary>
    /// 技能实例类，一个具体的技能对应一个这个类的实例
    /// 技能由多个触发器构成
    /// </summary>
    public class SkillInstance:ITxtSerializable
    {
        public event Action<SkillInstance> onSkillBegin = null;
        public event Action<SkillInstance> onSkillExit = null;
        public float startTime;
        public string name;
        private bool isUsed = false;
        private float lastTime = 0f;//是否正在使用
        private List<ISkillTrigger> skillTriggers = new List<ISkillTrigger>();//所有触发器

        
        public float LastTime
        {
            get { return lastTime; }
        }

        public int TriggerCount
        {
            get { return skillTriggers.Count; }
        }

        /// <summary>
        /// 添加触发器
        /// </summary>
        /// <param name="trigger"></param>
        public void AddTrigger(ISkillTrigger trigger)
        {
            if (trigger.StartTime + trigger.LastTime > this.lastTime)
                this.lastTime = trigger.StartTime + trigger.LastTime;
            skillTriggers.Add(trigger);
        }

        /// <summary>
        /// 移除触发器
        /// </summary>
        /// <param name="trigger"></param>
        public void RemoveTrigger(ISkillTrigger trigger)
        {
            skillTriggers.Remove(trigger);
            this.lastTime = 0f;
            foreach (var t in skillTriggers)
            {
                if (trigger.StartTime + trigger.LastTime > this.lastTime)
                    this.lastTime = trigger.StartTime + trigger.LastTime;
            }
        }

        /// <summary>
        /// 刷新技能
        /// </summary>
        private void Reset()
        {
            foreach (var trigger in skillTriggers)
                trigger.Release();
            if (onSkillExit != null)
                onSkillExit(this);
            this.isUsed = false;
        }

        /// <summary>
        /// 触发函数
        /// </summary>
        /// <param name="self">使用技能的单位</param>
        /// <param name="ignoreInput">是否屏蔽技能输入</param>
        public void Execute(AbstractPerson self, bool ignoreInput = false, float startTime = 0)
        {
            if (!self.InputOk)
                return;
            if (ignoreInput)
            {
                self.IgnoreInput(this.lastTime);
                return;
            }
            this.startTime = startTime;
            if (this.isUsed == true)
            {
                foreach (var trigger in skillTriggers)
                    trigger.Release();
            }
            else
                this.isUsed = true;
            if (onSkillBegin != null)
                onSkillBegin(this);
            foreach (var trigger in skillTriggers)
                trigger.Execute(self);
            MainLoop.Instance.ExecuteLater(Reset, this.lastTime);
        }

        
        
        #region ITxtSerializable
        public string Sign
        {
            get { return "skill"; }
        }

        public void SignInit(string args)
        {
            var strs = args.Split('|');
            Assert.IsTrue(strs.Length >= 2);
            this.name = strs[1].Trim();
//            Debug.Log(this.name + " SignInit");
        }

        public void LoadTxt(StreamReader sr)
        {
//            Debug.Log(this.name + " Main");
            do
            {

                var trigger = TxtManager.LoadData(sr) as AbstractSkillTrigger;
                //throw new Exception(sr.ReadLine());
                if (trigger == null)
                {
//                    Debug.LogWarning("trigger is null");
                    break;
                }

                this.AddTrigger(trigger);
            } while (true);
//            Debug.Log(this.name+" end Main");
        }

        #endregion
    }
}
