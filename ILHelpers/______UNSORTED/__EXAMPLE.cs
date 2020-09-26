namespace ILHelpers
{
    using System;



    public static class Ex
    {

        private static int IdkRandomShit(int a, int b, int c, float d)
        {
            return (int)(a + b + c * d);
        }

        private static void LogToString(object obj)
        {
            Console.WriteLine(obj);
        }

        public unsafe static void Example()
        {
            ICursor<Empty> cursor = default;
            Arg<int> arg1 = default;
            Arg<float> arg2 = default;
            Arg<object> arg3 = default;
            Local<int> loc0 = default;
            Local<object> loc1 = default;
            return cursor
                .LoadArg(arg1)
                .Dupe()
                .Dupe()
                .LoadArg(arg2)
                .Inline((delegate*<int, int, int, float, int>)&IdkRandomShit)
                .StoreLocal(loc0)
                .LoadArg(arg3)
                .Inline((delegate*<object,void>)&LogToString)
                .LoadLocal(loc0)
                .Dupe()
                .
                
        }
    }
}
