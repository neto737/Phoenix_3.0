using System;
using System.Threading;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct TimedLock : IDisposable
{
	private readonly object target;
	private TimedLock(object o)
	{
		this.target = o;
	}
	public static TimedLock Lock(object o)
	{
		return TimedLock.Lock(o, TimeSpan.FromSeconds(0.0));
	}
	public static TimedLock Lock(object o, TimeSpan timeout)
	{
		TimedLock result = new TimedLock(o);
		if (!Monitor.TryEnter(o, timeout))
        {
            Console.WriteLine("Timeout Waiting for lock... @" + o.ToString());
		}
		return result;
	}
	public void Dispose()
	{
		Monitor.Exit(this.target);
	}
}
