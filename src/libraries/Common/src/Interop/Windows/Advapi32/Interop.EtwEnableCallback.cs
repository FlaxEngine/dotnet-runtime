// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;

#if FEATURE_ETW
internal static partial class Interop
{
    internal static partial class Advapi32
    {
        internal const int EVENT_CONTROL_CODE_DISABLE_PROVIDER = 0;
        internal const int EVENT_CONTROL_CODE_ENABLE_PROVIDER = 1;
        internal const int EVENT_CONTROL_CODE_CAPTURE_STATE = 2;

        [StructLayout(LayoutKind.Sequential)]
        internal struct EVENT_FILTER_DESCRIPTOR
        {
            public long Ptr;
            public int Size;
            public int Type;
        }
    }
}
#endif
