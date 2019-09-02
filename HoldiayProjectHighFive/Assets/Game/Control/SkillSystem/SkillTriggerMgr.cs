﻿using System.Collections.Generic;

namespace Game.Control.SkillSystem
{
    /// <summary>
    /// 触发器管理者类
    /// </summary>
    public static class SkillTriggerMgr
    {
        /// <summary>
        /// AbstractPerson通过技能名从这里获取技能索引
        /// </summary>
        public static Dictionary<string, SkillInstance> skillInstanceDic = new Dictionary<string, SkillInstance>();

    }
}
