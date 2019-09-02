﻿using Game.Control.PersonSystem;

namespace Game.Control.BattleEffectSystem
{
    /// <summary>
    /// 所有战斗效果（恢复，伤害，治疗，Buff）的基类
    /// </summary>
    public interface IBattleEffect
    {
        /// <summary>
        /// 发挥作用
        /// </summary>
        void Execute(AbstractPerson ap);
    }
}
