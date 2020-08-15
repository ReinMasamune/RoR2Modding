namespace _PATCH
{
    using System;

    using AT = System.AttributeTargets;

    [Remove]
    [AttributeUsage(AT.Class | AT.Constructor | AT.Delegate | AT.Enum | AT.Event | AT.Field | AT.Interface | AT.Method | AT.Property | AT.Struct)]
    public class RemoveAttribute : Attribute { }

    [Remove]
    [AttributeUsage(AT.Struct)]
    public class MakeByRefAttribute : Attribute { }

    [Remove]
    [AttributeUsage(AT.Class | AT.Delegate | AT.Enum | AT.Event | AT.Field | AT.GenericParameter | AT.Interface | AT.Method | AT.Module | AT.Parameter | AT.Property | AT.Struct)]
    public class ChangeNameAttribute : Attribute
    {
        public String newName;
        public ChangeNameAttribute(String newName)
        {
            this.newName = newName;
        }
    }

    [Remove]
    [AttributeUsage(AT.Event | AT.Field | AT.Parameter | AT.Property | AT.ReturnValue)]
    public class ChangeTypeAttribute : Attribute
    {
        public Type newType;
        public ChangeTypeAttribute(Type newType)
        {
            this.newType = newType;
        }
    }

    [Remove]
    [AttributeUsage(AT.Event | AT.Field | AT.Parameter | AT.Property | AT.ReturnValue)]
    public class MakeManagedPointerTypeAttribute : Attribute { }


    [Remove]
    [AttributeUsage(AT.Method)]
    public class ILBodyAttribute : Attribute
    {
        public String[] opcodes;
        public ILBodyAttribute(params String[] opcodes)
        {
            this.opcodes = opcodes;
        }
    }

    [Remove]
    [AttributeUsage(AT.Class | AT.Delegate | AT.Interface | AT.Enum | AT.Struct)]
    public class MakeSealedInterfaceTypeAttribute : Attribute { }
}