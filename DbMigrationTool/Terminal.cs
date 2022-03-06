namespace DbMigrationTool;

internal class Terminal
{
    internal Terminal GetString(string text, out string result)
    {
        Console.Write($"{text}:");
        result = Console.ReadLine()!;

        return this;
    }

    internal Terminal Info(string info)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(info);

        return this;
    }

    internal Terminal Error(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);

        return this;
    }
}
