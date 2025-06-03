
using UnityEngine;

namespace Lost.Character
{

    [RequireComponent(typeof(AbstractCharacterAnimateController))]
    public class CharacterControllerBase : MonoBehaviour
    {
        public float moveSpeed = 5f;
        /// <summary>
        /// 动画控制器
        /// </summary>
        [HideInInspector]
        public AbstractCharacterAnimateController animateController;


        /// <summary>
        /// 是否存活
        /// </summary>
        public virtual bool isLive { get; } = true;


        protected virtual void Awake()
        {
            animateController = GetComponent<AbstractCharacterAnimateController>();
        }

        public virtual void Move(Vector3 direction)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }


    }
}