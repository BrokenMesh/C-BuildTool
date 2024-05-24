// ---- C_BuildTool ---- 21.8.2023 ----

using C_BuildTool.Builder;

internal class Program
{
    private static void Main(string[] _args) {

        if (_args.Length > 0) {
            switch (_args[0]) {
                case "Setup": Setup(); return;
                case "Clean": Clean(); return;
                case "Build": Build(); return;
            }
        }

        PrintUsage();
    }

    private static void Setup() {
        CBuilder _builder = new CBuilder(Environment.CurrentDirectory);
        _builder.InitializedTargetDir();
    }

    private static void Build() {
        CBuilder _builder = new CBuilder(Environment.CurrentDirectory);
        _builder.Build();
    }

    private static void Clean() {
        CBuilder _builder = new CBuilder(Environment.CurrentDirectory);
        _builder.Clean();
    }

    private static void PrintUsage() {
        string _usage =
            "Usage:\n" +
            "C-BuildTool [Action]\n\n" +
            "Actions: \n" +
            "> Setup\t\t\t - Set up c project\n" +
            "> Clean\t\t\t - Clean binary and object folders\n" +
            "> Build\t\t\t - Build c project\n";

        Console.WriteLine(_usage);
    }

}
