/**
 * summary:在使用Camera的worldToScreenPoint存在一些问题，即非平面情况下，超出屏幕外，坐标会不准确
 * author:ldnanchao
 * copy:https://blog.csdn.net/qq_52855744/article/details/121608886
 * */
using UnityEngine;
namespace LostFramework
{
    /// <summary>
    /// UI跟随脚本
    /// </summary>
    public class UIFollowWorldObject : MonoBehaviour
    {
        public Camera m_camera;
        public Transform m_target;
        public Canvas m_canvas;

        private bool hasFollowed = false;
        public bool alwaysFollow = true;
        public void Init(Camera camera, Transform target, Canvas canvas)
        {
            m_camera = camera;
            m_target = target;
            m_canvas = canvas;
            FollowObject();
        }

        public void Update()
        {
            FollowObject();
        }

        private void FollowObject()
        {
            if (!alwaysFollow && hasFollowed)
            {
                return;
            }

            if (m_camera != null && m_target != null)
            {
                Vector2 pos = m_camera.WorldToScreenPoint(m_target.transform.position);
                switch (m_canvas.renderMode)
                {
                    case RenderMode.ScreenSpaceOverlay:
                        (transform as RectTransform).position = pos;
                        hasFollowed = true;
                        break;
                    case RenderMode.ScreenSpaceCamera:
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, pos, m_camera, out Vector2 point))
                        {
                            transform.localPosition = new Vector3(point.x, point.y, 0);
                            hasFollowed = true;
                        }
                        break;
                }
            }
        }
    }

}
