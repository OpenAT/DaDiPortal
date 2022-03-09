namespace DbMigrationTool;

internal class Terminal
{
    internal Terminal GetString(string text, out string result)
    {
        Console.Write($"{text}: ");
        result = Console.ReadLine()!;

        return this;
    }

    internal Terminal GetInt(string text, out int result)
    {
        GetString(text, out string str);
        result = int.Parse(str);

        return this;
    }

    internal Terminal GetYesNo(string text, out bool result)
    {
        GetString($"{text} (y/n)", out string str);
        result = str == "y";

        return this;
    }

    internal Terminal Info(string info)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(info);

        return this;
    }

    internal Terminal Error(string error, Exception? exc = null)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        if (exc == null)
            Console.WriteLine(error);
        else
        {
            var messages = new List<string>();
            while (exc != null)
            {
                messages.Add(exc.Message);
                exc = exc.InnerException;
            }

            Console.WriteLine($"{error}:\n   {string.Join("\n   ", messages)}");
        }

        return this;
    }

    internal Terminal Success(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(text);

        return this;
    }
}
