using System;
using UnityEngine;

namespace LostFramework
{
    /// <summary>
    /// 士兵控制，包括移动，执行攻击等一系列动作
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        private bool faceRight = true;
        public float speed = 1f;

        private Rigidbody2D _rigidbody2D;
        public Animator characterAnimator =null;
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            SetFace();
            if(characterAnimator!=null) characterAnimator.SetFloat("Speed",_rigidbody2D.linearVelocity.magnitude);
        }

        private void FixedUpdate()
        {
            
        }

        public void Move(Vector3 inputAxis,Space space = Space.World)
        {
            
            switch (space)
            {
                case Space.Self:
                    inputAxis = transform.TransformDirection(inputAxis);
                    break;
                case Space.World:
                    inputAxis = inputAxis;
                    break;
            }
            _rigidbody2D.linearVelocity = inputAxis;
            if (inputAxis.x > 0)
            {
                faceRight = true;
            }

            if (inputAxis.x<0)
            {
                faceRight = false;
            }
            
            // transform.position += inputAxis*speed;
        }

        private void SetFace()
        {
            Vector3 scale = transform.localScale;
            if (faceRight)
                scale.x = Math.Abs(scale.x);
            else
            {
                scale.x = -Math.Abs(scale.x);
            }
            transform.localScale = scale;
        }

        
    }
}
