using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver
{
    public class Trie
    {
        private Node _root;

        public Trie()
        {
            _root = new Node(' ', string.Empty);
        }

        public void Add(string value)
        {
            AddNode(_root, value, 0);
        }

        public bool Contains(string value)
        {
            return ContainsValue(_root, value, 0);
        }

        public List<List<string>> SolveAnagram(string phrase)
        {
            return FindAllAnagrams(_root, _root, BuildPhraseDictionary(phrase), new List<string>());
        }

        private List<List<string>> FindAllAnagrams(Node root, Node currentNode, Dictionary<char, int> phraseDictionary, List<string> candidates)
        {
            var remainingChars = GetRemainingCharacters(phraseDictionary);

            if (remainingChars.Count == 0 && currentNode.IsCompleteWord)
            {
                currentNode.IsUsed = true;
                candidates.Add(currentNode.Value);
                return new List<List<string>> { candidates };
            }

            if (remainingChars.Count == 0)
                return null;
           
            var allAnagrams = new List<List<string>>();

            foreach (var character in remainingChars)
            {
                phraseDictionary[character]--;

                if (currentNode.Children.ContainsKey(character) && !currentNode.Children[character].IsUsed)
                {
                    var copy = candidates.ToList();

                    var anagrams = FindAllAnagrams(root, currentNode.Children[character], phraseDictionary, copy);
                    if (anagrams != null)
                        allAnagrams.AddRange(anagrams);
                }

                phraseDictionary[character]++;
            }

            if (IsCandidate(currentNode))
            {
                //Found a candidate word, search for next one
                candidates.Add(currentNode.Value);
                currentNode.IsUsed = true;

                var anagrams = FindAllAnagrams(root, root, phraseDictionary, candidates);
                if (anagrams != null)
                    allAnagrams.AddRange(anagrams);
            }

            foreach (var node in currentNode.Children.Values)
            {
                node.IsUsed = false;
            }

            return allAnagrams;
        }

        private bool IsCandidate(Node node)
        {
            //add word to possible list if current node is complete word and:
            //node is leaf
            //node is not leaf but all children have been used

            if (!node.IsCompleteWord)
                return false;

            if (node.Children.Keys.Count == 0 || node.Children.Values.Count == 0)
                return true;

            if (node.Children.Values.All(x => x.IsUsed))
                return true;

            return false;
        }

        private List<char> GetRemainingCharacters(Dictionary<char, int> phraseDictionary)
        {
            var remainingCharacters = new List<char>();

            foreach (var key in phraseDictionary.Keys)
            {
                if (phraseDictionary[key] > 0)
                    remainingCharacters.Add(key);
            }

            return remainingCharacters;
        }

        private Dictionary<char, int> BuildPhraseDictionary(string phrase)
        {
            var dictionary = new Dictionary<char, int>();

            foreach (var character in phrase)
            {
                if (dictionary.ContainsKey(character))
                    dictionary[character]++;
                else
                    dictionary.Add(character, 1);
            }

            return dictionary;
        }

        private void AddNode(Node currentNode, string value, int index)
        {
            if (currentNode.Value == value)
                currentNode.IsCompleteWord = true;

            if (index == value.Length)
                return;

            var key = value[index];

            if (!currentNode.Children.ContainsKey(key))
            {
                var newNode = new Node(key, value.Substring(0, index + 1));
                currentNode.Children.Add(key, newNode);
            }

            currentNode = currentNode.Children[key];
            AddNode(currentNode, value, index + 1);
        }

        private bool ContainsValue(Node currentNode, string value, int index)
        {
            if (currentNode == null)
                return false;

            if (currentNode.Value == value && currentNode.IsCompleteWord)
                return true;

            return ContainsValue(currentNode.Children[value[index]], value, index + 1);
        }
    }
}
