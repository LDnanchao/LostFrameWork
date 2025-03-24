using UnityEngine;

namespace Lost.AI
{

    /// <summary>
    /// 基于任务序列的AI控制器
    /// </summary>
    public class AIController:MonoBehaviour
    {
        private AIBehaviorController _behaviorController;
        private Rigidbody _rigidbody;
        
        
        public void MoveToPosition(Vector3 targetPosition)
        {

        }
        public void MoveToTarget(Transform target)
        {

        }
        public void RunAI(AIBehaviorController controller){

        }
        public void OnMoveCompleted(){

        }


        public void StopMovement(){

        }
    }

}