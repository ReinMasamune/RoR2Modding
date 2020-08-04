namespace ILHelpers.IllegalTypes.Patcher
{
    using System;
    using System.Linq;
    using Mono.Cecil;
    using Mono.Cecil.Cil;

    public static class Program
    {
        public static void Main(String[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("Bad Args");
                _ = Console.ReadKey();
                return;
            }
            var readParams = new ReaderParameters
            {
                ReadWrite = true,
                InMemory = true,
                ReadingMode = ReadingMode.Deferred,
            };
            var asm = AssemblyDefinition.ReadAssembly(args[0], readParams);
            if(asm is null)
            {
                Console.WriteLine("No Assembly found");
                _ = Console.ReadKey();
                return;
            }

            static void ApplyTypeAttributes(TypeDefinition type)
            {
                foreach(var atr in type.CustomAttributes)
                {
                    switch(atr.AttributeType.FullName)
                    {

                    }
                }
            }


            static void PatchType(TypeDefinition type)
            {
                ApplyTypeAttributes(type);

                foreach(TypeDefinition sub in type.NestedTypes) PatchType(sub);
                
                
            }


            foreach(ModuleDefinition module in asm.Modules)
            {
                foreach(TypeDefinition type in module.Types)
                {
                    PatchType(type);
                    foreach(var method in type.Methods)
                    {

                        var skip = true;
                        CustomAttribute attribToRemove = null;
                        foreach(var attrib in method.CustomAttributes)
                        {
                            if(attrib.AttributeType.Name == "PatchCallToCalliAttribute")
                            {
                                skip = false;
                                attribToRemove = attrib;
                                break;
                            }
                        }
                        if(skip) continue;
                        method.CustomAttributes.Remove(attribToRemove);

                        var body = method.Body;
                        var instr = body.Instructions.First((ins) => ins.OpCode == OpCodes.Call);
                        var callSite = new CallSite(method.ReturnType);
                        foreach(var par in method.Parameters)
                        {
                            callSite.Parameters.Add(par);
                        }
                        var newInstr = Instruction.Create(OpCodes.Calli, callSite);

                        var processor = body.GetILProcessor();
                        processor.Replace(instr, newInstr);
                    }
                }
            }
            asm.Write(args[0]);
        }
    }
}
