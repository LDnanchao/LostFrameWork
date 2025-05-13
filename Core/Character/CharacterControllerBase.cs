
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
        public AbstractCharacterAnimateController animateController;
        
       
        /// <summary>
        /// 是否存活
        /// </summary>
        public virtual bool isLive { get; } = true;


       

        public virtual void Move(Vector3 direction)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

       
    }
}