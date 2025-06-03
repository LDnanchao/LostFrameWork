using System.Collections.Generic;


public delegate void RedPointDelegate(int redPoint);
public class RedPointNode
{
    public List<RedPointNode> ChildNodes;
    public RedPointNode ParentNode;
    private int _selfNum;
    public int SelfNum { 
        set{
            int oldNum = _selfNum;
            _selfNum = value; 
            UpdateTotalNum(oldNum, value);
        } 
    }
    private int _totalNum;
    public int TotalNum => _totalNum;
    public string NodeName;
    public string FullName;
    public RedPointDelegate redPointCallback = new RedPointDelegate((int num) => { });

    public RedPointNode(string nodeName, RedPointNode parentNode)
    {
        ChildNodes = new List<RedPointNode>();
        ParentNode = parentNode;
        SelfNum = 0;
        NodeName = nodeName;
        FullName = (parentNode!= null? parentNode.FullName + "_" : "") + NodeName;
    }
    private void UpdateTotalNum(int oldValue, int newValue)
    {
        int changeValue = newValue - oldValue;
        _totalNum += changeValue;
        if (ParentNode!= null)
        {
            ParentNode.UpdateTotalNum(oldValue, newValue);
        }
        redPointCallback.Invoke(TotalNum);
    }

    public void Dispose()
    {
        redPointCallback = null;
        SelfNum = 0;
        if (ParentNode != null)
        {
            ParentNode.ChildNodes.Remove(this);
        }

        for (int i = 0; i < ChildNodes.Count; i++)
        {
            ChildNodes[i].Dispose();
        }

        ChildNodes.Clear();
        ParentNode = null;
        ChildNodes = null;


    }
}

public class RedPointTree
{
    private RedPointNode RootNode = new RedPointNode("root", null);
    private List<RedPointNode> AllNodes = new List<RedPointNode>();
    /// <summary>
    /// 所有的节点都是基于Root节点向下扩展
    /// </summary>
    /// <param name="nodeName"></param>
    public RedPointNode AddNode(string nodeName)
    {
        //节点拆分
        string[] nodeNames = nodeName.Split("_");
        var currentNode = RootNode;
        for (int i = 0; i < nodeNames.Length; i++)
        {
            string currentNodeName = nodeNames[i];
            var nextNode = currentNode.ChildNodes.Find(x => x.NodeName == currentNodeName);
            if (nextNode!=null)
            {
                currentNode = nextNode;
            }
            else
            {
                RedPointNode newNode = new RedPointNode(currentNodeName, currentNode);
                currentNode.ChildNodes.Add(newNode);
                AllNodes.Add(newNode);
                currentNode = newNode;
            }
        }
        return currentNode;
    }
    public void RemoveNode(string nodeName)
    {
        //节点拆分
        string[] nodeNames = nodeName.Split("_");
        var currentNode = RootNode;
        for (int i = 0; i < nodeNames.Length; i++)
        {
            string currentNodeName = nodeNames[i];
            var nextNode = currentNode.ChildNodes.Find(x => x.NodeName == currentNodeName);
            if (nextNode != null)
            {
                currentNode = nextNode;
            }
            else
            {
                return;
            }
        }
        if (currentNode != RootNode && currentNode.ParentNode != null)
        {
            currentNode.Dispose();
        }
    }
    public void SetRedPoint(string nodeName, int redPoint)
    {
        GetNode(nodeName).SelfNum = redPoint;
    }
    public int GetRedPoint(string nodeName)
    {
        var node = GetNode(nodeName);
        if (node == null)
        {
            return 0;
        }
        return GetNode(nodeName).TotalNum;
    }
    public RedPointNode GetNode(string nodeName)
    {
        string[] nodeNames = nodeName.Split("_");
        var currentNode = RootNode;
        for (int i = 0; i < nodeNames.Length; i++)
        {
            string currentNodeName = nodeNames[i];
            var nextNode = currentNode.ChildNodes.Find(x => x.NodeName == currentNodeName);
            if (nextNode != null)
            {
                currentNode = nextNode;
            }
            else
            {
                return null;
            }
        }
        return currentNode;
    }

}