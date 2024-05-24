﻿using System.Diagnostics;

namespace C_BuildTool.Builder
{
    internal class CBuilder {
        public CBuilderConfig Config { get; private set; }

        private string targetDir;
        private bool isDirInitialized = false;

        public CBuilder(string _targetDir) {
            targetDir = _targetDir;
            Config = CBuilderConfig.LoadConfig(_targetDir + "./CBConfig.json");

            isDirInitialized = File.Exists(targetDir + "./CBConfig.json");
        }

        public void InitializedTargetDir() {
            try {
                Directory.CreateDirectory(targetDir + "/bin");
                Directory.CreateDirectory(targetDir + "/lib");
                Directory.CreateDirectory(targetDir + "/obj");
                Directory.CreateDirectory(targetDir + "/res");
                Directory.CreateDirectory(targetDir + "/src");

                string _cFile = "#include <stdio.h>\r\n\r\nint main() {\r\n\tprintf(\"Hello World!\\n\");\r\n\treturn 1;\r\n}";
                File.WriteAllText(targetDir + "/src/main.c", _cFile);

                CBuilderConfig.SaveConfig(Config, targetDir + "./CBConfig.json");
            }
            catch (Exception _e) {
                Console.WriteLine("Failed to initialized directory: " + _e.Message);
            }
        }

        public void SaveConfig() {
            CBuilderConfig.SaveConfig(Config, targetDir + "./CBConfig.json");
        }

        public void Clean() {
            Console.WriteLine("Cleaning 'obj' and 'bin' folders");
            CleanFolder("/obj");
            CleanFolder("/bin");
        }

        public void Build() {
            Clean();

            Console.WriteLine("Compile: ");

            bool _compRes = SearchFolderForCompelation("/src");
            if (!_compRes) return;

            Console.WriteLine("Link: ");

            List<string>? _objFiles = SearchFolderForObjFiles("/obj");
            if (_objFiles == null) return;

            Console.WriteLine($"Creating {"/bin/" + Config.Target}.");

            bool _linkRes = LinkFiles(_objFiles);
            if (!_linkRes) return;

            Console.WriteLine("Moveing 'res' files into bin folder.");
            MoveResources();
        }

        private bool SearchFolderForCompelation(string _path) {
            try {
                string[] _files = Directory.GetFiles(targetDir + _path);

                string _targetfolder = targetDir + "/obj" + _path.Replace("/src", "");
                if (!Directory.Exists(_targetfolder))
                    Directory.CreateDirectory(_targetfolder);

                foreach (string _f in _files) {
                    if (Path.GetExtension(_f) != ".c") continue;

                    Console.WriteLine($"File: {Path.GetFileName(_f)}");

                    string targetpath = _targetfolder + "/" + Path.GetFileNameWithoutExtension(_f) + ".o";
                    bool _res = CompileFile(_f, targetpath);
                    if (!_res) return false;
                }

                string[] _dirs = Directory.GetDirectories(targetDir + _path);

                foreach (string _d in _dirs) {
                    string _targetpath = _path + "/" + Path.GetFileName(_d);
                    bool _res = SearchFolderForCompelation(_targetpath);
                    if (!_res) return false;
                }
            }
            catch (Exception _e) {
                Console.WriteLine("Failed to compile files: " + _e.Message);
                return false;
            }

            return true;
        }

        private bool CompileFile(string _path, string _target) {
            string _args = $"\"{_path}\" " +
                $"-o \"{_target}\" " +
                $"-c {FormatArgs(Config.CompilerFlags, "-")} {FormatArgs(Config.IncludeDirectories, "-I")}";

            Console.WriteLine(_args);

            int _result = RunCommand("gcc", _args);
            if (_result == -1) return false;

            return true;
        }

        private List<string>? SearchFolderForObjFiles(string _path) {
            List<string> _objFiles = new List<string>();
            try {
                string[] _files = Directory.GetFiles(targetDir + _path);

                foreach (string _f in _files) {
                    if (Path.GetExtension(_f) != ".o") continue;
                    _objFiles.Add(_f);
                }

                string[] _dirs = Directory.GetDirectories(targetDir + _path);

                foreach (string _d in _dirs) {
                    string _targetpath = _path + "/" + Path.GetFileName(_d);
                    List<string>? _res = SearchFolderForObjFiles(_targetpath);
                    if (_res == null) return null;
                    _objFiles.AddRange(_res);
                }
            }
            catch (Exception _e) {
                Console.WriteLine("Failed to find files for linking: " + _e.Message);
                return null;
            }

            return _objFiles;
        }

        private bool LinkFiles(List<string> _objFiles) {
            string _args =
                $"-o \"{targetDir + "/bin/" + Config.Target}\" " +
                $"{FormatArgs(_objFiles, "")}" +
                $"{FormatArgs(Config.LinkerFlags, "-l")} {FormatArgs(Config.LinkedDirectories, "-L")}";

            Console.WriteLine(_args);

            int _res = RunCommand(Config.Compiler, _args);

            return _res != -1;
        }

        private void CleanFolder(string _path) {
            try {
                Directory.Delete(targetDir + _path, true);
                Directory.CreateDirectory(targetDir + _path);
            }
            catch (Exception _e) {
                Console.WriteLine($"Could not clean folder {_path}: {_e.Message}");
            }
        }

        private void MoveResources() {
            try {
                var _resFolder = new DirectoryInfo(targetDir + "/res");
                var _binFolder = new DirectoryInfo(targetDir + "/bin");

                CopyFilesRecursively(_resFolder, _binFolder);
            }
            catch (Exception _e) {
                Console.WriteLine("Could not copy 'res' files into 'bin' folder: " + _e.Message);
            }
        }

        private static void CopyFilesRecursively(DirectoryInfo _source, DirectoryInfo _target) {

            Directory.CreateDirectory(_target.FullName);

            foreach (FileInfo fi in _source.GetFiles()) {
                fi.CopyTo(Path.Combine(_target.FullName, fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in _source.GetDirectories()) {
                DirectoryInfo nextTargetSubDir =
                    _target.CreateSubdirectory(diSourceSubDir.Name);
                CopyFilesRecursively(diSourceSubDir, nextTargetSubDir);
            }
        }

        private string FormatArgs(List<string> _args, string _symbol) {
            string _output = "";

            foreach (string _s in _args) {
                _output += $"\"{_symbol}{_s}\" ";
            }

            return _output;
        }

        private int RunCommand(string _program, string _args) {
            try {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = _program;
                startInfo.Arguments = _args;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                Process? _p = Process.Start(startInfo);

                if (_p == null) return -1;

                while (!_p.HasExited) {
                    if (_p.StandardOutput.EndOfStream) break;
                    string? line = _p.StandardOutput.ReadLine();
                    Console.WriteLine(line);
                }

                return _p.ExitCode;
            }
            catch (Exception _e) {
                Console.WriteLine($"Could not run command '{_program} {_args}':\n" + _e.Message);
                return -1;
            }
        }
    }
}
