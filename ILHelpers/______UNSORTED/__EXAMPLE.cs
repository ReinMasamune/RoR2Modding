namespace ILHelpers
{
    using System;
    using System.Runtime.InteropServices.ComTypes;

    public static class Ex
    {



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
                .Inline((delegate*<object, void>)&LogToString)
                .LoadLocal(loc0)
                .Dupe()
                .Pop()
                .Return()
                ;
                
        }

        private static int IdkRandomShit(int a, int b, int c, float d)
        {
            return (int)(a + b + c * d);
        }

        public unsafe static DMDReturn<int> DoStuff(ICursor<Empty> ctx, Arg<int> a, Arg<int> b, Arg<int> c, Arg<float> d)
        {
            ICursor<IL<int,Empty>> s1 = ctx.LoadArg<int, Empty>(a);
            ICursor<IL<int,IL<int,Empty>>> s2 = s1.LoadArg<int, IL<int, Empty>>(b);
            ICursor<IL<int,IL<int,IL<int,Empty>>>> s3 = s2.LoadArg<int, IL<int, IL<int, Empty>>>(c);
            ICursor<IL<float, IL<int, IL<int, IL<int, Empty>>>>> s4 = s3.LoadArg<float, IL<int, IL<int, IL<int, Empty>>>>(d);



            var s5 = s4.Call((delegate*<int, int, int, float, int>)&IdkRandomShit);
            var s6 = s5.Return();



            return default;
        }
    }
}
