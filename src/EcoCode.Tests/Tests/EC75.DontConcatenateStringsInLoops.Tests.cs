﻿namespace EcoCode.Tests;

[TestClass]
public sealed class DontConcatenateStringsInLoopsTests
{
    private static readonly AnalyzerDlg VerifyAsync = TestRunner.VerifyAsync<DontConcatenateStringsInLoops>;

    [TestMethod]
    public async Task EmptyCodeAsync() => await VerifyAsync("").ConfigureAwait(false);

    [TestMethod]
    public async Task DontConcatenateStringsInLoops1Async() => await VerifyAsync("""
        public class Test
        {
            private string s1 = string.Empty;
            private static string s2 = string.Empty;

            public void Run(string s0)
            {
                for (int i = 0; i < 10; i++)
                    [|s0 += i;|]
                for (int i = 0; i < 10; i++)
                    s0 = i.ToString();

                for (int i = 0; i < 10; i++)
                    [|s1 += i;|]
                for (int i = 0; i < 10; i++)
                    s1 = i.ToString();

                for (int i = 0; i < 10; i++)
                    [|s2 += i;|]
                for (int i = 0; i < 10; i++)
                    s2 = i.ToString();

                string s3 = string.Empty;
                for (int i = 0; i < 10; i++)
                    [|s3 += i;|]
                for (int i = 0; i < 10; i++)
                    s3 = i.ToString();
            }
        }
        """).ConfigureAwait(false);

    [TestMethod]
    public async Task DontConcatenateStringsInLoops2Async() => await VerifyAsync("""
        public class Test
        {
            private string s1 = string.Empty;
            private static string s2 = string.Empty;

            public void Run(string s0)
            {
                string s3 = string.Empty;
                for (int i = 0; i < 10; i++)
                {
                    s0 = i.ToString();
                    [|s0 += i;|]

                    s1 = i.ToString();
                    [|s1 += i;|]

                    s2 = i.ToString();
                    [|s2 += i;|]

                    s3 = i.ToString();
                    [|s3 += i;|]

                    string s4;
                    s4 = i.ToString();
                    s4 += i;
                }
            }
        }
        """).ConfigureAwait(false);


    [TestMethod]
    public async Task DontConcatenateStringsInLoopsForEachAsync() => await VerifyAsync("""
        using System.Collections.Generic;

        public class Test
        {
            public static void Run(string s0)
            {
                string s = "";

                List<int> numbers = [ 0, 1, 1, 2, 3, 5, 8, 13 ];

                foreach (var i in numbers) {
                    [|s += i;|]
                }
            }
        }
        """).ConfigureAwait(false);

    [TestMethod]
    public async Task DontConcatenateStringsInLoopsLinqForEachAsync() => await VerifyAsync("""
        using System.Collections.Generic;
        public class Test
        {
            public static void Run(string s0)
            {
                string s = "";

                List<int> numbers = [ 0, 1, 1, 2, 3, 5, 8, 13 ];

                numbers.ForEach(i => [|s += i|]);
            }
        }
        """).ConfigureAwait(false);
}
