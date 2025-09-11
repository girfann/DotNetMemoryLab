using DotNetMemoryLab.Domain.Models;
using DotNetMemoryLab.Domain.Primitives;

namespace DotNetMemoryLab.Diagnostics.Process;

/// <summary>
/// Reads process-level memory metrics from the current process.
/// </summary>
public static class ProcessMemoryReader
{
    /// <summary>
    /// Captures a snapshot of working set, private bytes, virtual memory and thread count.
    /// </summary>
    /// <returns> Process memory snapshot. </returns>
    public static ProcessMemory Read()
    {
        var process = System.Diagnostics.Process.GetCurrentProcess();

        return new ProcessMemory
        {
            WorkingSet = new ByteSize(process.WorkingSet64),
            PrivateBytes = new ByteSize(process.PrivateMemorySize64),
            VirtualMemory = new ByteSize(process.VirtualMemorySize64),
            ThreadCount = process.Threads.Count
        };
    }
}
