using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Commands;

namespace RotMG.Game.Logic.Commands
{
    using CommandInfo = Tuple<MethodInfo, ParameterInfo[], CommandAttribute>;
    
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public readonly string Name;
        public readonly bool RequiresPerms;
        public readonly bool ListCommand;
        public readonly string[] Aliases;

        public CommandAttribute(string name, bool requiresPerms = false, bool listCommand = true, params string[] aliases)
        {
            Name = name;
            RequiresPerms = requiresPerms;
            ListCommand = listCommand;
            Aliases = aliases;
        }
    }
    
    public static class CommandManager
    {
        public static Dictionary<string, CommandInfo> Commands;
        
        public static void Init()
        {
            var methods = typeof(RankedCommands).GetMethods()
                .Union(typeof(UnrankedCommands).GetMethods())
                .Where(x => x.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0);

            Commands = new Dictionary<string, CommandInfo>();
            foreach (var method in methods)
            {
                var attribute = (CommandAttribute) method.GetCustomAttributes(typeof(CommandAttribute), false)[0];
                if (Commands.ContainsKey(attribute.Name.ToLower()))
                    throw new Exception(
                        $"Command with key <{attribute.Name}> already defined. <{method.DeclaringType}.{method.Name}()>");

                Commands[attribute.Name.ToLower()] = new CommandInfo(method, method.GetParameters(), attribute);

                foreach (var alias in attribute.Aliases)
                {
                    if (Commands.ContainsKey(alias.ToLower()))
                        throw new Exception(
                            $"Command with key <{alias}> already defined. <{method.DeclaringType}.{method.Name}()>");

                    Commands[alias.ToLower()] = new CommandInfo(method, method.GetParameters(), attribute);
                }
            }

            Commands = Commands.OrderBy(k => k.Key)
                .ToDictionary(k => k.Key, k => k.Value);
        }

        public static void Execute(Player player, string rawText)
        {
            var words = rawText[1..].Split(' ');
            var inputWords = new string[words.Length - 1];
            for (var i = 1; i < words.Length; i++)
                inputWords[i - 1] = words[i];
            var command = words[0];

            if (!Commands.TryGetValue(command, out var commandInfo))
            {
                player.SendError("Unknown command");
                return;
            }

            if (!player.Client.Account.Ranked && commandInfo.Item3.RequiresPerms)
            {
                player.SendError("Unknown command");
                return;
            }

            var mandatoryInputParameters = commandInfo.Item2
                .Where(x => x.ParameterType != typeof(Player) && !x.IsOptional)
                .ToArray();

            if (inputWords.Length < mandatoryInputParameters.Length)
            {
                var usage = mandatoryInputParameters
                    .Select(x => x.Name);

                player.SendError($"Usage: /{command} <{string.Join("> <", usage)}>");
                return;
            }

            var parameters = new object[commandInfo.Item2.Length];
            var inputIndex = 0;
            for (var i = 0; i < parameters.Length; i++)
            {
                var paramType = commandInfo.Item2[i].ParameterType;
                if (paramType == typeof(Player))
                {
                    parameters[i] = player;
                    continue;
                }

                if (commandInfo.Item2[parameters.Length - 1].ParameterType == typeof(string) && i == parameters.Length - 1)
                {
                    parameters[i] = string.Join(' ', inputWords[inputIndex..]);
                    break;
                }

                if (!TryGetValue(inputWords[inputIndex], paramType, out var value))
                {
                    player.SendError($"Unable to parse {paramType} from {inputWords[inputIndex]}");
                    return;
                }

                parameters[i] = value;
                inputIndex++;
            }
            player.Client.Account.Save();

            //we disconnect the player thats why it crashed 
            try {
                commandInfo.Item1.Invoke(null, parameters);

            }
            catch(Exception e)
            {
                Program.Print(PrintType.Error, "[Command Manager] " + e.Message + "\n" + e.StackTrace);
            }
        }


        private static bool TryGetValue(string input, Type targetType, out object value)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(targetType);
                value = converter.ConvertFromInvariantString(input);
                return true;
            }
            catch (Exception)
            {
                value = null;
                return false;
            }
        }
    }
}