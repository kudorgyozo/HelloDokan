// See https://aka.ms/new-console-template for more information
using DokanNet.Logging;
using DokanNet;
using static DokanNet.Dokan;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HelloDokan;

Console.WriteLine("Hello, World!");

try {

    //var m = new StupidFS();
    // mounting point, dokan options, num of threads
    //m.Mount("g:\\", DokanOptions.DebugMode | DokanOptions.StderrOutput, 1);
    Task.Run(() => ListenForKeyPress());


    using var mirrorLogger = new ConsoleLogger("[Stupid] ");
    using var dokanLogger = new ConsoleLogger("[Dokan] ");
    using var dokan = new Dokan(dokanLogger);
    //var mirror = new DokanTest(mirrorLogger);
    var mirror = new StupidFS(mirrorLogger);
    var mountPath = @"N:\";

    var dokanBuilder = new DokanInstanceBuilder(dokan)
        //.ConfigureLogger(() => dokanLogger)
        .ConfigureOptions(options => {
            //options.Options = DokanOptions.DebugMode;
            options.MountPoint = mountPath;
            options.SingleThread = true;
        });
    using var dokanInstance = dokanBuilder.Build(mirror);


    Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => {

        e.Cancel = true;
        dokan.RemoveMountPoint(mountPath);
    };

    await dokanInstance.WaitForFileSystemClosedAsync(uint.MaxValue);

    Console.WriteLine(@"Success");
} catch (DokanException ex) {
    Console.WriteLine(@"Error: " + ex.Message);
}

void ListenForKeyPress() {
    while (true) {
        var keyInfo = Console.ReadKey(intercept: true);
        if (keyInfo.KeyChar == 'C' || keyInfo.KeyChar == 'c') {
            Console.Clear();
        }

    }
}