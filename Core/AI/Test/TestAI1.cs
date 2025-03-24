using UnityEngine;

namespace Lost.AI
{
    [RequireComponent(typeof(AIController))]
    public class TestAI1 : MonoBehaviour
    {
        public AIBehaviorController behaviorController;
        public AIController aiController;
        
        private void OnValidate() {
            aiController = gameObject.GetComponent<AIController>();
        }
        void Awake()
        {
            behaviorController = new AIBehaviorController(null);

            behaviorController.SetOwener(aiController);
            behaviorController.AddTaskSequence<LoopTaskSequence>();
            behaviorController.AddTaskSequence<AttackTaskSequence>();
        }
        public void Start()
        {
            behaviorController.Start();
            //角色死亡时，回调结束ai控制器
        }
        void Update()
        {
            behaviorController.Update();
        }
        void OnDestroy()
        {
            behaviorController.Cancel();
        }

        public class LoopTaskSequence : TaskSequenceBase
        {
            public override string GetSequenceName()
            {
                return "LoopTaskSequence";
            }
            public override int GetPriority()
            {
                return 0;
            }
            public override void InitActions()
            {
                LoopAction loopAction = new LoopAction();
                loopAction.AddLoopPosition(new Vector3(0, 0, 0));
                loopAction.AddLoopPosition(new Vector3(1, 1, 1));
                loopAction.AddLoopPosition(new Vector3(2, 2, 2));
                AddAction(loopAction);
                AddAction<SearchAction>(TaskActionType.Parallel);
            }
            public override void Update()
            {
                base.Update();
                //如果有敌人，则结束任务
            }
        }
        public class AttackTaskSequence : TaskSequenceBase
        {
            public override string GetSequenceName()
            {
                return "AttackTaskSequence";
            }
            public override int GetPriority()
            {
                return 1;
            }
            public override void InitActions()
            {
                AddAction<FollowTargetAction>();
                AddAction<SearchAction>(TaskActionType.Parallel);
                AddAction<AttackAction>();

            }
            public override void Update()
            {
                base.Update();
                //如果目标丢失，则任务结束
            }

        }

    }
}