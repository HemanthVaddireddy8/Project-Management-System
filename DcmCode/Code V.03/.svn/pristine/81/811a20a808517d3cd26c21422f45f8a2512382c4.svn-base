using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Dcm.Models
{
    
    #region Main Menu
    public class TreeNode
    {
        public int MenuId;

        public int ParentMenuId;

        public TreeNode Parent;

        public string Name;

        public int MenuOrder;

        public string PrimaryKey;

        public string TableName;

        public string ObjectName;

        public string OnlyDetailPage;

        public string DetailPageUrl;

        public string IconClass;
        public List<TreeNode> Children = new List<TreeNode>();
    }

    public class Tree
    {
        private TreeNode rootNode;
        public TreeNode RootNode
        {
            get { return rootNode; }
            set
            {
                if (RootNode != null)
                    Nodes.Remove(RootNode.MenuId);

                Nodes.Add(value.MenuId, value);
                rootNode = value;
            }
        }

        public Dictionary<int,TreeNode> Nodes { get; set; }

        public Tree()
        {
        }

        public void BuildTree()
        {
            TreeNode parent;
            foreach (var node in Nodes.Values)
            {
                if (Nodes.TryGetValue(node.ParentMenuId, out parent) &&
                    node.MenuId != node.ParentMenuId)
                {
                    node.Parent = parent;
                    parent.Children.Add(node);
                }
            }
        }
    }
    #endregion

#region Mini Menu

    public class TreeNodeMini
    {
        public int MenuId;

        public int ParentMenuId;

        public TreeNodeMini Parent;

        public string Name;

        public int MenuOrder;

        public string PrimaryKey;

        public string TableName;

        public string ObjectName;

        public string OnlyDetailPage;

        public string DetailPageUrl;

        public string IconClass;
        public List<TreeNodeMini> Children = new List<TreeNodeMini>();
    }

    public class TreeMini
    {
        private TreeNodeMini rootNode;
        public TreeNodeMini RootNode
        {
            get { return rootNode; }
            set
            {
                if (RootNode != null)
                    Nodes.Remove(RootNode.MenuId);

                Nodes.Add(value.MenuId, value);
                rootNode = value;
            }
        }

        public Dictionary<int, TreeNodeMini> Nodes { get; set; }

        public TreeMini()
        {
        }

        public void BuildTreeMini()
        {
            TreeNodeMini parent;
            foreach (var node in Nodes.Values)
            {
                if (Nodes.TryGetValue(node.ParentMenuId, out parent) &&
                    node.MenuId != node.ParentMenuId)
                {
                    node.Parent = parent;
                    parent.Children.Add(node);
                }
            }
        }
    }
#endregion
}