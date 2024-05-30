using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace C_BuildTool.Builder
{
    internal class CBuilderConfig 
    {
        [JsonRequired] public string Target { get; }
        [JsonRequired] public string Compiler { get; }
        [JsonRequired] public List<string> CompilerFlags { get; }
        [JsonRequired] public List<string> IncludeDirectories { get; }
        [JsonRequired] public List<string> LinkerFlags { get; }
        [JsonRequired]  public List<string> LinkedElements { get; }
        [JsonRequired]  public List<string> LinkedDirectories { get; }

        public CBuilderConfig(string target, string compiler, List<string> compilerFlags, List<string> includeDirectories, List<string> linkerFlags, List<string> linkedElements, List<string> linkedDirectories) {
            Target = target;
            Compiler = compiler;
            CompilerFlags = compilerFlags;
            IncludeDirectories = includeDirectories;
            LinkerFlags = linkerFlags;
            LinkedElements = linkedElements;
            LinkedDirectories = linkedDirectories;
        }

        public static void SaveConfig(CBuilderConfig _config, string _path) {
            try {
                string _configText = JsonConvert.SerializeObject(_config);
                File.WriteAllText(_path, _configText);
                Console.WriteLine("Saved Config File at " + _path);
            }
            catch (Exception _e) {
                Console.WriteLine("Saving Confog failed: " + _e);
            }
        }

        public static CBuilderConfig LoadConfig(string _path) {

            try {
                string _file = File.ReadAllText(_path);
                CBuilderConfig? _config = JsonConvert.DeserializeObject<CBuilderConfig>(_file);

                if (_config != null) {
                    Console.WriteLine("Loaded Config File: " + _path);
                    return _config;
                }
            }
            catch (Exception _e) {
                Console.WriteLine("Could not load Config file: " + _e.Message);
                Console.WriteLine("Using standart configuration");
            }

            return new CBuilderConfig("main.exe", "gcc", new List<string> { "Wall", "g"}, new List<string>(), new List<string>(), new List<string>(), new List<string>());
        }


    }
}
