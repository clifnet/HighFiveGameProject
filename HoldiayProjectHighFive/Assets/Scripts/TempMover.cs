using System;
using ReadyGamerOne.Rougelike.Mover;
using ReadyGamerOne.Utility;
using UnityEngine;

namespace Game.Scripts
{
    public class TempMover:MonoBehaviour,IMover2D
    {
        #region Rigidbody

        private Rigidbody2D rig;
        private Rigidbody2D Rig
        {
            get
            {
                if (!rig)
                    rig = GetComponent<Rigidbody2D>();
                return rig;
            }
        }        

        #endregion

        public Vector3 Position
        {
            get => Rig.position;
            set => Rig.position = value;
        }
        public Vector2 Velocity
        {
            get => Rig.velocity;
            set => Rig.velocity = value;
        }
        public float GravityScale
        {
            get => Rig.gravityScale;
            set => Rig.gravityScale = value;
        }
        
        [SerializeField] private LayerMask colliderLayers;
        public LayerMask ColliderLayers
        {
            get => colliderLayers;
            set => colliderLayers = value;
        }
        [SerializeField] private LayerMask triggerLayers;
        public LayerMask TriggerLayers
        {
            get => triggerLayers; 
            set=>triggerLayers=value; 
        }

        public event Action<GameObject> eventOnColliderEnter;
        public event Action<GameObject> eventOnColliderStay;
        public event Action<GameObject> eventOnColliderExit;
        public event Action<GameObject> eventOnTriggerEnter;
        public event Action<GameObject> eventOnTriggerStay;
        public event Action<GameObject> eventOnTriggerExit;

        private void Update()
        {
            transform.position += Time.deltaTime * new Vector3(Velocity.x, Velocity.y);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision2D)
        {
            eventOnColliderEnter?.Invoke(collision2D.gameObject);
        }


        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
//            Debug.Log($"发现：{other.name}");
            if (1 == colliderLayers.value.GetNumAtBinary(other.gameObject.layer))
            {
                eventOnColliderEnter?.Invoke(other.gameObject,TouchDir.Top);
            }
            else if (1 == triggerLayers.value.GetNumAtBinary(other.gameObject.layer))
            {
                eventOnTriggerEnter?.Invoke(other.gameObject);
            }
        }
    }
}