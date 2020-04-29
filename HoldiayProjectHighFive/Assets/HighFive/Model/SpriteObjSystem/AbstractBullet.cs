using System;
using HighFive.Control.Movers;
using HighFive.Model.Person;
using ReadyGamerOne.Rougelike.Mover;
using ReadyGamerOne.Rougelike.Person;
using ReadyGamerOne.Script;
using UnityEngine;
using UnityEngine.Assertions;

namespace HighFive.Model
{
    /// <summary>
    /// 子弹类
    /// 需求：
    ///    子弹：
    ///         1、造成伤害
    ///             input:damageScale,HighFivePerson
    ///         2、特效
    ///             击中音效、爆炸震屏、粒子效果
    ///         3、打到地面回调，打到敌人回调
    ///         4、Input:
    ///              enemyLayer,terrainLayer
    ///         5、单体伤害/AOE伤害，半径多少
    ///         6、子弹如何飞行
    /// 
    ///         
    ///         
    /// 
    ///     直线子弹：
    ///         input：dir,speed,shotThrough
    ///         思路：无视重力，设置初始速度后，匀速直线飞行
    ///     抛物线子弹：
    ///         input:initialSpeed
    ///         思路：考虑重力，速度会受重力影响，抛物线运动
    ///     追踪子弹：
    ///         input:target,speed
    ///         思路：
    ///     弹道子弹：
    ///         input：具体看情况
    ///         思路：使用数学公式计算其每一时刻速度
    /// </summary>
    public abstract class AbstractBullet:MonoBehaviour
    {
        private bool _init = false;
        public float damageScale = 1;
        public float maxLife = 2.0f;
        protected IHighFivePerson selfPerson;
        protected AbstractMover mover;

        public LayerMask EnemyLayers
        {
            get => mover.TriggerLayers;
            set => mover.TriggerLayers = value;
        }
        public LayerMask TerrainLayers
        {
            get => mover.ColliderLayers;
            set => mover.ColliderLayers = value;
        }

        protected virtual Action OnShotUpdate => null;

        /// <summary>
        /// 初始化子弹
        /// </summary>
        public virtual void ShotStart(IHighFivePerson self)
        {
            Assert.IsNotNull(self);
            selfPerson = self;
            mover = GetComponent<BulletMover>();
            mover.eventOnColliderEnter += OnTerrainEnter;
            mover.eventOnTriggerEnter += OnEnemyEnter;
            _init = true;
            MainLoop.Instance.ExecuteLater(DestorySelf, maxLife);
        }

        protected virtual void Update()
        {
            if (!_init)
                return;
            OnShotUpdate?.Invoke();
        }
        
        protected virtual void OnTerrainEnter(GameObject terrain, TouchDir touchDir)
        {
            Debug.Log($"hit terrain [{terrain.name}], destory self");
            DestorySelf();
        }

        protected virtual void OnEnemyEnter(GameObject enemy, TouchDir touchDir)
        {
            if (!selfPerson.gameObject.TryAttack(enemy, damageScale))
            {
                Debug.LogWarning($"无效攻击【{selfPerson?.CharacterName}=>{enemy.name}】");
            }
        }


        protected void DestorySelf()
        {
            if (this)
                Destroy(this.gameObject);
        }
    }
}