using System;

public static class ThrowHelper
{
    public static void ArgumentOutOfRangeException()
    {
        throw new ArgumentOutOfRangeException();
    }
    public static void ArgumentOutOfRangeException(string name)
    {
        throw new ArgumentOutOfRangeException(name);
    }
}
