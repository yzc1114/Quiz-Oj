using System;
using System.Linq;
using quiz_oj.Configs.Exceptions;
using System.Runtime.InteropServices;

namespace quiz_oj.IndependentNS
{
    public class ListNode
    {
        public ListNode Next { get; set; }
        public int Val { get; set; }
        public ListNode(int x)
        {
            Val = x;
        }


        public override bool Equals(object? obj)
        {
            if (!(obj is ListNode))
            {
                return false;
            }
            var self = this;
            var other = (ListNode) obj;
            while (self.Val == other.Val)
            {
                self = self.Next;
                other = other.Next;
                if (self == null && other == null)
                {
                    return true;
                }
                if (self != null && other != null)
                {
                    continue;
                }
                return false;
            }
            return false;
        }
    }

    public static class DataStructureParser
    {

        [DllImport("TreeNodeStrChecker", EntryPoint = "isValidTreeNodeString", CallingConvention = CallingConvention.StdCall)]
        private static extern bool IsValidTreeNodeString(string arg);

        public static ListNode ParseListNode(string val)
        {
            val = val.Trim();
            void Raise()
            {
                throw new Exception($"ListNode Parse Failed. Value {val}");
            }
            if (val.Length == 2) return null;
            if (val.Length < 2) Raise();
            var head = new ListNode(-1);
            var curr = head;
            try
            {
                val = val.Substring(1, val.Length - 2);
                var numbers = val.Split(",").Select(s => s.Trim()).Select(int.Parse).ToArray();
                foreach (var number in numbers)
                {
                    curr.Next = new ListNode(number);
                    curr = curr.Next;
                }
                return head.Next;
            }
            catch (Exception e)
            {
                Raise();
                return null;
            }
        }
        public static TreeNode ParseTreeNode(string val)
        {
            if (!IsValidTreeNodeString(val)) 
            {
                Console.WriteLine("Parse Tree Node failed!");
            }
            Console.WriteLine("Parse Tree Node succeed.");
            return new TreeNode(1);
        }
    }

    public class TreeNode
    {
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public int Val { get; set; }

        public TreeNode(int x)
        {
            Val = x;
        }
    }
}