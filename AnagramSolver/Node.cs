using System.Collections.Generic;

namespace AnagramSolver
{
    public class Node
    {
        public Dictionary<char, Node> Children { get; private set; }
        public char Key { get; private set; }
        public string Value { get; private set; }
        public bool IsCompleteWord { get; set; }
        public bool IsUsed { get; set; }

        public Node(char key, string value)
        {
            Key = key;
            Value = value;
            Children = new Dictionary<char, Node>();
        }
    }
}
