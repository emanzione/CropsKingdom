using System.Runtime.CompilerServices;

namespace GameLoop.Networking.Memory
{
    public sealed class SimpleManagedAllocator : IMemoryAllocator
    {
        public static SimpleManagedAllocator Instance = new SimpleManagedAllocator();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] Allocate(int size)
        {
            return new byte[size];
        }
    }
}