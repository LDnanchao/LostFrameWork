namespace Lost.AI
{
    /// <summary>
    /// 任务动作类型
    /// </summary>
    public enum TaskActionType
    {
        Solo,//单独
        Parallel//并行
    }
    public enum TaskActionStatus
    {
        Running,
        Completed,
    }
    /// <summary>
    /// 任务动作
    /// </summary>
    public interface ITaskAction
    {
        public TaskActionStatus Status { get; set; }
        public AIController GetOwner();
        public void SetOwner(AIController owner);
        /// <summary>
        /// 动作类型，并行，单独
        /// </summary>
        /// <returns></returns>
        public TaskActionType GetTaskActionType();
        /// <summary>
        /// 开始
        /// </summary>
        public void Enter();
        /// <summary>
        /// 刷新
        /// </summary>
        public void Update();
        /// <summary>
        /// 结束
        /// </summary>
        public void Exit();
    }
    public abstract class TaskActionBase : ITaskAction
    {
        private TaskActionStatus _status;
        public TaskActionStatus Status { get => _status; set => _status = value; }

        public TaskActionType GetTaskActionType()
        {
            return TaskActionType.Solo;
        }
        public virtual void Enter()
        {

        }
        public virtual void Exit()
        {

        }

        public virtual void Update()
        {

        }
        public void Finish()
        {
            Status = TaskActionStatus.Completed;
        }

        private AIController _owner;



        public AIController GetOwner()
        {
            return _owner;
        }

        public void SetOwner(AIController owner)
        {
            _owner = owner;
        }
    }
}