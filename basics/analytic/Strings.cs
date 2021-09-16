using System;
using System.Linq;

namespace Analytic
{
    public class Strings
    {

        /// <summary>
        /// - A pangram is a unique sentence in which every letter of the alphabet is used at least once
        /// - Example:
        ///     The quick brown fox jumps over a lazy dog
        /// </summary>
        public void Pangrams(string input)
        {
            var alphabateBucket = new bool[26];
            var searchArray = input.ToCharArray();

            foreach (var item in searchArray)
            {
                if (item >= 97 && item <= 122)
                {
                    var aplhaIndex = 25 - (122 - item);
                    var bucketValue = alphabateBucket.ElementAt(aplhaIndex);
                    if (bucketValue)
                        continue;
                    else
                        alphabateBucket[aplhaIndex] = true;
                }
            }

            var isPangram = alphabateBucket.Aggregate((first, next) => next && first);

            if (isPangram)
                Console.WriteLine("pangram");
            else
                Console.WriteLine("not pangram");

        }

        /// <summary>
        /// - If a = 1, b = 2, c = 3 ,..., z = 26. Given a string, find all possible codes that string can generate.
        /// - Example:
        ///     Input: "1123"
        ///     Output: aabc (a = 1, a = 1, b = 2, c = 3), 
        ///             kbc (k = 11, b = 2, c= 3), 
        ///             alc (a = 1, l = 12, c = 3), 
        ///             aaw (a= 1, a =1, w= 23), 
        ///             kw (k = 11, w = 23)
        /// </summary>
        public void StringDecode(string input, string code)
        {
            if (string.IsNullOrEmpty(input))
            {
                if (!string.IsNullOrEmpty(code) && code.Length > 0)
                    Console.WriteLine(code);
                return;
            }
            StringDecode(input.Substring(1), code + ('a' + (input[0] - '0') - 1));
            if (input.Length > 1)
            {
                var number = Convert.ToInt32(input.Substring(0, 2));
                if (number <= 26)
                    StringDecode(input.Substring(2), code + ('a' + number - 1));
            }
        }
    }
}
