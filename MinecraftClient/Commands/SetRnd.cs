using System;
using System.Collections.Generic;

namespace MinecraftClient.Commands
{
    class SetRnd : Command
    {
        public override string CmdName { get { return "setrnd"; } }
        public override string CmdUsage { get { return Translations.Get("cmd.setrnd.format"); } }
        public override string CmdDesc { get { return "cmd.setrnd.desc"; } }
        private static readonly Random rand = new();

        public override string Run(McClient handler, string command, Dictionary<string, object>? localVars)
        {
            if (HasArg(command))
            {
                string[] args = GetArg(command).Split(' ');

                if (args.Length > 1)
                {
                    // detect "to" keyword in string
                    if (args.Length == 2 && args[1].Contains("to"))
                    {
                        int num1;
                        int num2;

                        // try to extract the two numbers from the string
                        try
                        {
                            num1 = Convert.ToInt32(args[1][..args[1].IndexOf('t')]);
                            num2 = Convert.ToInt32(args[1].Substring(args[1].IndexOf('o') + 1, args[1].Length - 1 - args[1].IndexOf('o')));
                        }
                        catch (Exception)
                        {
                            return Translations.Get("cmd.setrndnum.format");
                        }

                        // switch the values if they were entered in the wrong way
                        if (num2 < num1)
                            (num2, num1) = (num1, num2);

                        // create a variable or set it to num1 <= varlue < num2
                        if (Settings.Config.AppVar.SetVar(args[0], rand.Next(num1, num2)))
                        {
                            return string.Format("Set %{0}% to {1}.", args[0], Settings.Config.AppVar.GetVar(args[0])); //Success
                        }
                        else return Translations.Get("cmd.setrndnum.format");
                    }
                    else
                    {
                        // extract all arguments of the command
                        string argString = command[(8 + command.Split(' ')[1].Length)..];

                        // process all arguments similar to regular terminals with quotes and escaping
                        List<string> values = ParseCommandLine(argString);

                        // create a variable or set it to one of the values
                        if (values.Count > 0 && Settings.Config.AppVar.SetVar(args[0], values[rand.Next(0, values.Count)]))
                        {
                            return string.Format("Set %{0}% to {1}.", args[0], Settings.Config.AppVar.GetVar(args[0])); //Success
                        }
                        else return Translations.Get("cmd.setrndstr.format");
                    }
                }
                else return GetCmdDescTranslated();
            }
            else return GetCmdDescTranslated();
        }
    }
}
