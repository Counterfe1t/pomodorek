﻿namespace Pomodorek.Utils;

public class StringParser
{
    public static string Parse(string value, string content)
        => content.Replace("{{" + nameof(value) + "}}", value);
}