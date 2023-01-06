using StringFormatter.Core.Interfaces;
using System.Text;

namespace StringFormatter.Core
{
    public class StringFormatter : IStringFormatter
    {
        public static readonly StringFormatter Shared = new StringFormatter();

        private readonly Cache _cache;

        private StringFormatter()
        {
            _cache = new Cache();
        }

        public string Format(string template, object target)
        {
            int currentState = 1;
            int previousState = 0;
            int startPointer = 0;
            int endPointer = 0;

            var result = new StringBuilder();

            for (int i = 0; i < template.Length; i++)
            {
                previousState = currentState;
                currentState = _transitionMatrix[currentState, GetSubset(template[i])];

                switch (currentState)
                {
                    case 0:
                        throw new ArgumentException($"Invalid template or interpolation " +
                            $"argument not supported. Position: {i}");

                    case 1:
                        if (previousState == 4 || previousState == 9 || previousState == 10)
                        {
                            string cacheValue = String.Concat(template[endPointer..i]
                                .Where(c => !Char.IsWhiteSpace(c)));
                            result.Remove(startPointer, i - startPointer);
                            result.Append(_cache.TryHitCache(target, cacheValue));
                        }
                        else
                            result.Append(template[i]);
                        break;
                    
                    case 4:
                        if (previousState == 2 || previousState == 5 ) 
                            startPointer = i;
                        break;

                    default:
                        result.Append(template[i]); break;
                }
            }
            if ( currentState == 1 ) 
                return result.ToString();
            else
                throw new ArgumentException($"Invalid template ending");
        }

        // Character subsets in positions of transition collumns
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

        // State machine
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

        // Determine subset from character
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