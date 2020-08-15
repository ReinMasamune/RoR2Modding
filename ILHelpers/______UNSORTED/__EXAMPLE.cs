namespace ILHelpers
{
    using System;



    public static class Ex
    {



        public unsafe static void CreateMyDmD()
        {
            //delegate*<Int32, ref Boolean, Single> sig = default;
            //var dmd = DMD.Create(sig);
            //dmd.body = (stack, arg0, arg1) =>
            //{
            //    static Single SomeCode(Int32 i, ref Boolean b)
            //    {
            //        if(i < 0) b = false;
            //        return i;
            //    }
            //    delegate*<Int32, ref Boolean, Single> del = &SomeCode;
            //    return stack
            //        .LoadArg(arg0)
            //        .LoadArg(arg1)
            //        .CallInline(del)
            //        .Return();
            //};
        }
    }
}
