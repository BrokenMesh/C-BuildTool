// ---- C_BuildTool ---- 21.8.2023 ----

using C_BuildTool.Builder;

internal class Program
{
    private static void Main(string[] _args) {

        CBuilder _builder = new CBuilder(Environment.CurrentDirectory);

        if (_args.Length > 0) {
            switch (_args[0].ToLower()) {
                case "setup": _builder.InitializedTargetDir(); return;
                case "clean": _builder.Clean(); return;
                case "build": _builder.Build(); return;
                case "run": _builder.Run(); return;
            }
        }

        PrintUsage();
    }

    private static void PrintUsage() {
        string _usage =
            "Usage:\n" +
            "C-BuildTool [Action]\n\n" +
            "Actions: \n" +
            "> Setup\t\t\t - Set up c project\n" +
            "> Clean\t\t\t - Clean binary and object folders\n" +
            "> Build\t\t\t - Build c project\n" + 
            "> Run\t\t\t - Build & Run c project\n";

        Console.WriteLine(_usage);
    }

}
