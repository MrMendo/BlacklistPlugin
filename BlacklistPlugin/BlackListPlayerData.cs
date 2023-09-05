using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Memory;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace BlacklistPlugin
{
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public readonly unsafe struct BlackListPlayerData
    {
        [FieldOffset(0)] private readonly byte* _Value;

        public byte* pointerToValue => _Value;
        public string Value => MemoryHelper.ReadStringNullTerminated((nint)_Value);
    }
}
