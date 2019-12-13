namespace DotNetProjectParser.SampleApps.NetCore.Console
{
    class OtherClass
    {
        static void Test(string[] args)
        bla bla this should not compile because it's a content file!
        {
            System.Console.WriteLine("Hello World!");
        }
    }
}