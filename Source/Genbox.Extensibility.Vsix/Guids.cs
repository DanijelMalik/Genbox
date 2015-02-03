// Guids.cs
// MUST match guids.h

using System;

namespace Genbox.Extensibility.Vsix
{
    static class GuidList
    {
        public const string guidGenerator4Developers_Extensibility_VsixPkgString = "bc388390-9c8e-4f89-8266-0bd3ecefe1cc";
        public const string guidGenerator4Developers_Extensibility_VsixCmdSetString = "78b90208-d4ae-44fc-8c74-b18c886dc4bf";
        public const string guidToolWindowPersistanceString = "0ac85823-f815-43f2-88f9-cb8c7122fc79";

        public static readonly Guid guidGenerator4Developers_Extensibility_VsixCmdSet = new Guid(guidGenerator4Developers_Extensibility_VsixCmdSetString);
    };
}