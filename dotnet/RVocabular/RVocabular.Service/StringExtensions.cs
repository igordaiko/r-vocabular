namespace RVocabular;

public static class StringExtensions
{
    public static string ToUnderscore(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        var length = str.Length;

        for (var i = 1; i != str.Length; ++i)
        {
            if (char.IsUpper(str[i]) && str[i - 1] != '_')
                ++length;
        }

        return string.Create(length, str, static (chars, s) =>
        {
            var ch = s[0];

            chars[0] = char.ToLower(ch);

            for (int i = 1, j = 0; i != s.Length; ++i)
            {
                ch = s[i];

                if (char.IsUpper(ch) && s[i - 1] != '_')
                    chars[++j] = '_';

                chars[++j] = char.ToLower(ch);
            }
        });
    }
}
