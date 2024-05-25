namespace Frank.Mapping.Documents;

public static class Ngram
{
    public static double Compare(string text1, string text2, Size parseSize = Size.Tri)
    {
        var temp1 = ParseToCharArray(text1, parseSize).ToArray();
        var temp2 = ParseToCharArray(text2, parseSize).ToArray();

        if (temp1.Length == 0 || temp2.Length == 0)
            return 0;

        char[][] data1;
        char[][] data2;

        if (temp1.Length > temp2.Length)
        {
            data1 = temp1;
            data2 = temp2;
        }
        else
        {
            data1 = temp2;
            data2 = temp1;
        }

        var dataCount = data1.Length + data2.Length;
        var sameCount = data1.Count(t1 => data2.Any(t1.SequenceEqual));

        return (double)sameCount / (dataCount - sameCount);
    }

    private static List<char[]> ParseToCharArray(string text, Size parseSize)
    {
        var words = new List<char[]>();
        var size = (int)parseSize;
        
        if (string.IsNullOrEmpty(text))
            return words;

        if (text.Length <= size)
        {
            var dest = new char[size];

            Array.Copy(text.ToCharArray(), 0, dest, 0, text.Length);
            words.Add(dest);

            return words;
        }

        for (var i = 0; i < text.Length - size + 1; i++)
        {
            var dest = new char[size];

            text.CopyTo(i, dest, 0, size);
            words.Add(dest);
        }

        return words;
    }

}