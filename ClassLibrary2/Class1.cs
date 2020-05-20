
namespace ClassLibrary2
{
    using System;
    using InterfaceThings;

    public class Class1
    {
        public void DoShit()
        {
            var obj = new Object();

            var conv = obj as InterfaceThings.ITestInterface;

            var conv2 = (ITestInterface)obj;
        }
    }
}
