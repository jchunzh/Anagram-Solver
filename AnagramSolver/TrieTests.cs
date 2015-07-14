using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AnagramSolver
{
    [TestFixture]
    public class TrieTests
    {
        public Trie Subject;

        [SetUp]
        public void SetUp()
        {
            Subject = new Trie();
        }

        [TestFixture]
        public class Contains : TrieTests
        {
            [Test]
            public void when_single_word_is_added_then_trie_contains_word()
            {
                var word = "add";
                Subject.Add(word);

                Assert.IsTrue(Subject.Contains(word));
            }

            [Test]
            public void when_multiple_words_are_added_then_trie_contains_all_words()
            {
                var words =
                new [] {
                    "a",
                    "ab",
                    "abs",
                    "abaci",
                    "aback",
                    "abacus",
                    "abacuses"
                };

                foreach (var word in words)
                {
                    Subject.Add(word);
                }

                foreach (var word in words)
                {
                    Assert.IsTrue(Subject.Contains(word));
                }
            }

            [Test]
            public void when_duplicate_words_are_added_then_trie_contains_word()
            {
                var words = new[]
                {
                    "aback",
                    "aback",
                    "aback",
                    "aback",
                    "aback"
                };

                foreach (var word in words)
                {
                    Subject.Add(word);
                }

                Assert.IsTrue(Subject.Contains("aback"));
            }

            [Test]
            public void contains_complete_word_that_is_also_a_subword_of_another_word()
            {
                var words =
                   new[]
                    {
                        "dad",
                        "da",
                    };

                foreach (var word in words)
                {
                    Subject.Add(word);
                }

                Assert.IsTrue(Subject.Contains("da"));
            }
        }

        [TestFixture]
        public class SolveAnagram : TrieTests
        {
            [Test]
            public void solves_single_word()
            {
                var words =
               new[] {
                   "bar",
                   "arb",
                   "rab"
                };

                foreach (var word in words)
                {
                    Subject.Add(word);
                }

                var results = Subject.SolveAnagram("rba");

                Assert.AreEqual(3, results.Count);
                CollectionAssert.AreEquivalent(words, results.SelectMany(x => x));
            }

            [Test]
            public void solves_phrase()
            {
                var words =
                    new[]
                    {
                        "a",
                        "an",
                        "and",
                        "dad",
                        "da",
                        "dare",
                        "d",
                        "n",
                        "dn"
                    };

                foreach (var word in words)
                {
                    Subject.Add(word);
                }

                var results = Subject.SolveAnagram("dan");

                var expected = new List<List<string>>
                {
                    new List<string>
                    {
                        "an",
                        "d"
                    },
                    new List<string>
                    {
                        "and"
                    },
                    new List<string>
                    {
                        "da",
                        "n"
                    },
                    new List<string>
                    {
                        "d",
                        "a",
                        "n"
                    },
                    new List<string>
                    {
                        "dn",
                        "a"
                    }
                };

                //currently returns duplicate combinations 
                //e.g. {"dn" "a"} and {"a", "dn"}
                AssertPhrasesEqual(expected, results);
            }

            private void AssertPhrasesEqual(List<List<string>> expected, List<List<string>> actual)
            {
                foreach (var phrase in expected)
                {
                    Assert.IsTrue(actual.Exists(x => AreEquivalent(phrase, x)));
                }
            }

            private bool AreEquivalent(List<string> expected, List<string> actual)
            {
                if (expected.Count != actual.Count)
                    return false;

                var sortedExpected = expected.OrderBy(x => x).ToList();
                var sortedActual = expected.OrderBy(x => x).ToList();

                for (int i = 0; i < sortedActual.Count(); i++)
                {
                    if (sortedExpected[i] != sortedActual[i])
                        return false;
                }

                return true;
            }
        }
    }
}
