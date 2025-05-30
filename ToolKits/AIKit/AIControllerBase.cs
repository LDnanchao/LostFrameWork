using System;
using Lost;
using Lost.Character;
using UnityEngine;
namespace Lost.AI
{
    public class AIControllerBase : MonoBehaviour
    {
        [HideInInspector]
        public CharacterControllerBase characterController;
        public string aiType;
        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterControllerBase>();
        }
        public virtual void Update()
        {
            if (isRunAI)
            {
                _aiFSM.Update();
            }

            if (isMoving)
            {
                if (!isPositionTarget && targetTransform == null)
                {
                    MoveComplete();
                }
                else
                {
                    Vector3 direction = isPositionTarget ? targetPosition - transform.position : targetTransform.position - transform.position;
                    characterController.Move(direction.normalized);

                    if (Vector3.Distance(transform.position, isPositionTarget ? targetPosition : targetTransform.position) < distance)
                    {
                        MoveComplete();
                    }
                }

            }

        }

        public virtual void SetCharacterController(CharacterControllerBase characterController)
        {
            this.characterController = characterController;
        }
        public virtual CharacterControllerBase GetCharacterController()
        {
            return characterController;
        }

        #region  Movement
        protected bool isMoving = false;
        protected bool isPositionTarget = false;
        protected Vector3 targetPosition;
        protected Transform targetTransform;
        protected float distance = 0.1f;
        public bool IsMoving => isMoving;
        public void MoveToTarget(Transform target, float distance = 0.1f)
        {
            this.distance = distance;
            isMoving = true;
            isPositionTarget = false;
            targetTransform = target;
        }

        public void MoveToPosition(Vector3 position)
        {
            isMoving = true;
            isPositionTarget = true;
            targetPosition = position;
        }

        public virtual void StopMove()
        {
            isMoving = false;
            characterController.Move(Vector3.zero);
        }

        protected virtual void MoveComplete()
        {
            isMoving = false;
            characterController.Move(Vector3.zero);
        }
        #endregion

        #region  AI
        protected IAIFSM _aiFSM;
        protected bool isRunAI = false;


        /// <summary>
        /// 运行AI
        /// </summary>
        /// <param name="aiFSM"></param>
        public void RunAI(IAIFSM aiFSM)
        {
            _aiFSM = aiFSM;
            aiType = _aiFSM.GetType().FullName;
            _aiFSM.Start();
            isRunAI = true;
        }

        public void StopAI()
        {
            _aiFSM.Stop();
            isRunAI = false;
        }





        #endregion


    }
}