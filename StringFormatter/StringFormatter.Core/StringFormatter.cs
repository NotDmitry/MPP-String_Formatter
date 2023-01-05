using StringFormatter.Core.Interfaces;

namespace StringFormatter.Core
{
    public class StringFormatter : IStringFormatter
    {
        public static readonly StringFormatter Shared = new StringFormatter();

        public string Format(string template, object target)
        {
            throw new NotImplementedException();
        }

        private readonly string[] _subsets =
        {
            "",
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
            "0123456789",
            "_",
            "{",
            "}",
            "[",
            "]",
            " ",
        };

        private readonly int[,] _transitionMatrix =
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 1, 1, 1, 1, 2, 3, 1, 1, 1},
            { 0, 4, 0, 4, 1, 0, 0, 0, 5},
            { 0, 0, 0, 0, 0, 1, 0, 0, 0},
            { 0, 4, 4, 4, 0, 1, 6, 0, 10},
            { 0, 4, 0, 4, 0, 0, 0, 0, 5},
            { 0, 0, 7, 0, 0, 0, 0, 0, 6},
            { 0, 0, 7, 0, 0, 0, 0, 9, 8},
            { 0, 0, 0, 0, 0, 0, 0, 9, 8},
            { 0, 0, 0, 0, 0, 1, 0, 0, 9},
            { 0, 0, 0, 0, 0, 1, 6, 0, 10}
        };

        private int GetSubset(char token)
        {
            int result = 0;
            for (int i = 0; i < _subsets.Length; i++)
            {
                result = _subsets[i].Contains(token) ? i : result;
            }
            return result;
        }

    }
}