using System;
using Lost;
using Lost.Character;
using UnityEngine;
namespace Lost.AI
{
    public class AIControllerBase : MonoBehaviour
    {
        public CharacterControllerBase characterController;
        public string aiType;
        public string currentState;
        public virtual void Update()
        {
            if (isRunAI)
            {
                _aiFSM.Update();
                currentState = _aiFSM.GetCurrentState();
            }

            if (isMoving)
            {
                Vector3 direction = isPositionTarget ? targetPosition - transform.position : targetTransform.position - transform.position;
                characterController.Move(direction.normalized);

                if (Vector3.Distance(transform.position, isPositionTarget ? targetPosition : targetTransform.position) < 0.1f)
                {
                    MoveComplete();
                }
            }

        }
       
       public virtual void SetCharacterController(CharacterControllerBase characterController){
           this.characterController = characterController;
       }
       public virtual CharacterControllerBase GetCharacterController(){
           return characterController;
       }

        #region  Movement
        private bool isMoving = false;
        private bool isPositionTarget = false;
        private Vector3 targetPosition;
        private Transform targetTransform;
        public void MoveToTarget(Transform target)
        {
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

        public void StopMove()
        {
            isMoving = false;
            characterController.Move(Vector3.zero);
        }

        private void MoveComplete()
        {
            isMoving = false;
            characterController.Move(Vector3.zero);
        }
        #endregion

        #region  AI
        private IAIFSM _aiFSM;
        private bool isRunAI = false;


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