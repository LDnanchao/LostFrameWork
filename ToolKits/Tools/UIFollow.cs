using UnityEngine;

namespace LostFramework
{
    /// <summary>
    /// UI跟随脚本
    /// </summary>
    public class UIFollow : MonoBehaviour
    {
        public Transform target=null;
        public Camera see=null;
        public RectTransform rectTransform=null;
        public Vector2 offset=Vector2.zero;
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        /// <summary>
        /// 绑定跟随对象和摄像机
        /// </summary>
        /// <param name="targetTransf"></param>
        /// <param name="see"></param>
        public void Bind(Transform transf,Camera see,Vector2 offset)
        {
            this.target = transf;
            this.see = see;
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null && see == null && rectTransform == null) return;
            rectTransform.position = see.WorldToViewportPoint(target.position);
        }
    }

}
