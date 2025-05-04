using QFramework;
using System;
using System.Collections.Generic;

namespace Client
{
    
    public class RedPointManager : Singleton<RedPointManager>
    {
        private RedPointManager() { }
        private RedPointTree mRedPointTree = new RedPointTree();

        public void RegisterRedPoint(string redPoint, RedPointDelegate onRedPointChanged)
        {
            var node = mRedPointTree.GetNode(redPoint);
            if (node == null)
            {
                node = mRedPointTree.AddNode(redPoint);  
            }
            node.redPointCallback += onRedPointChanged;
        }

        public void UnRegisterRedPoint(string redPoint, RedPointDelegate onRedPointChanged)
        {
            var node = mRedPointTree.GetNode(redPoint);
            if (node != null)
            {
                node.redPointCallback -= onRedPointChanged;
            }
        }

        public int GetRedPointCount(string redPoint)
        {
            var node = mRedPointTree.GetNode(redPoint);
            if (node!= null)
            {
                return node.TotalNum; ;
            }
            return 0;
        }
    }
}
