using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

public class ThreadPool
{
	public enum ThreadStatus
	{
		Started,
		Stopped,
		Paused
	}
	public delegate void OnStartedEventHandler();
	public delegate void OnFinishedEventHandler();
	private global::ThreadPool.OnStartedEventHandler OnStartedEvent;
	private global::ThreadPool.OnFinishedEventHandler OnFinishedEvent;
	private object __CountLock;
	private object __PushLock;
	private bool __AllPushed;
	private int __MaxThreadCount;
	private static ThreadPool.ThreadStatus __Status;
	private List<Thread> __ActiveThreads;

	public event global::ThreadPool.OnStartedEventHandler OnStarted
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.OnStartedEvent = (global::ThreadPool.OnStartedEventHandler)Delegate.Combine(this.OnStartedEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.OnStartedEvent = (global::ThreadPool.OnStartedEventHandler)Delegate.Remove(this.OnStartedEvent, value);
		}
	}
	public event global::ThreadPool.OnFinishedEventHandler OnFinished
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.OnFinishedEvent = (global::ThreadPool.OnFinishedEventHandler)Delegate.Combine(this.OnFinishedEvent, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.OnFinishedEvent = (global::ThreadPool.OnFinishedEventHandler)Delegate.Remove(this.OnFinishedEvent, value);
		}
	}

    public int ThreadCount
    {
        get
        {
            int count = 0;
            int num3 = 0;
            bool Label_009C_go = false;
        Label_0000:
            try
            {
                int num4;
            Label_0001:
                ProjectData.ClearProjectError();
                int num2 = 1;
            Label_0008:
                num4 = 2;
                List<Thread>.Enumerator enumerator = this.__ActiveThreads.GetEnumerator();
            Label_0017:
                if (!enumerator.MoveNext())
                {
                    goto Label_0046;
                }
                Thread current = enumerator.Current;
            Label_0028:
                num4 = 3;
                if (current.ThreadState != System.Threading.ThreadState.Stopped)
                {
                    goto Label_0041;
                }
            Label_0035:
                num4 = 4;
                this.Close(current);
                goto Label_0000;
            Label_0041:
                num4 = 7;
                goto Label_0017;
            Label_0046:
                enumerator.Dispose();
            Label_0053:
                num4 = 8;
                count = this.__ActiveThreads.Count;
                goto Label_00DC;
            Label_0066:
                num3 = 0;
                switch ((num3 + 1))
                {
                    case 1:
                        goto Label_0001;

                    case 2:
                        goto Label_0008;

                    case 3:
                        goto Label_0028;

                    case 4:
                        goto Label_0035;

                    case 5:
                        goto Label_0000;

                    case 6:
                    case 7:
                        goto Label_0041;

                    case 8:
                        goto Label_0053;

                    case 9:
                        goto Label_00DC;

                    default:
                        goto Label_00D1;
                }
            Label_009C:
                if (Label_009C_go)
                {
                    num3 = num4;
                    switch (num2)
                    {
                        case 0:
                            goto Label_00D1;

                        case 1:
                            goto Label_0066;
                    }
                }
            }
            catch (Exception exception1) //when (?)
            {
                ProjectData.SetProjectError(exception1);
                Label_009C_go = true;
                goto Label_0000;
            }
        Label_00D1:
            throw ProjectData.CreateProjectError(-2146828237);
        Label_00DC:
            if (num3 != 0)
            {
                ProjectData.ClearProjectError();
            }
            return count;
        }
    }

	public int MaxThreadCount
	{
		get
		{
			return this.__MaxThreadCount;
		}
		set
		{
			this.__MaxThreadCount = value;
		}
	}

	public bool Finished
	{
		get
		{
			return this.AllPushed && this.ThreadCount <= 0;
		}
	}

	public global::ThreadPool.ThreadStatus Status
	{
		get
		{
			return __Status;
		}
		set
		{
			__Status = value;
		}
	}

	private bool AllPushed
	{
		get
		{
			object _PushLock = this.__PushLock;
			ObjectFlowControl.CheckForSyncLockOnValueType(_PushLock);
			bool result;
			lock (_PushLock)
			{
				result = (this.__AllPushed | this.Status == global::ThreadPool.ThreadStatus.Stopped);
			}
			return result;
		}
	}

	public bool Paused
	{
		get
		{
			if (!this.Finished)
			{
				return __Status == global::ThreadPool.ThreadStatus.Paused;
			}
			bool result = false;
			return result;
		}
		set
		{
			if (!this.Finished)
			{
				if (value)
				{
					__Status = global::ThreadPool.ThreadStatus.Paused;
				}
				else
				{
					__Status = global::ThreadPool.ThreadStatus.Started;
				}
			}
		}
	}

	public ThreadPool(int maxThreadCount)
	{
		this.__CountLock = RuntimeHelpers.GetObjectValue(new object());
		this.__PushLock = RuntimeHelpers.GetObjectValue(new object());
		this.MaxThreadCount = maxThreadCount;
		if (this.MaxThreadCount <= 0)
		{
			this.MaxThreadCount = 1;
		}
		this.__ActiveThreads = new List<Thread>(this.MaxThreadCount);
		global::ThreadPool.OnStartedEventHandler onStartedEvent = this.OnStartedEvent;
		if (onStartedEvent != null)
		{
			onStartedEvent();
		}
	}

	public void Open(Thread thread = null)
	{
		if (thread != null)
		{
			this.__ActiveThreads.Add(thread);
		}
	}

	public void AllJobsPushed()
	{
		object _PushLock = this.__PushLock;
		ObjectFlowControl.CheckForSyncLockOnValueType(_PushLock);
		lock (_PushLock)
		{
			this.__AllPushed = true;
			if (this.Finished)
			{
				ThreadPool.OnFinishedEventHandler onFinishedEvent = this.OnFinishedEvent;
				if (onFinishedEvent != null)
				{
					onFinishedEvent();
				}
			}
		}
	}

    public void Close(Thread thread = null)
    {
        int num2 = 0;
        int num = 0;
        int num3 = 0;
        bool Label_004C_go = false;
        Label_RETY:
        try
        {
            if (Label_004C_go)
            {
                goto Label_004C;
            }
            Label_0000:
                ProjectData.ClearProjectError();
                num = 1;
            Label_0007:
                num3 = 2;
                if (!this.__ActiveThreads.Contains(thread))
                {
                    goto Label_0089;
                }
            Label_0017:
                num3 = 3;
                this.__ActiveThreads.Remove(thread);
                goto Label_0089;
            Label_0028:
                num2 = 0;
                switch ((num2 + 1))
                {
                    case 1:
                        goto Label_0000;

                    case 2:
                        goto Label_0007;

                    case 3:
                        goto Label_0017;

                    case 4:
                    case 5:
                        goto Label_0089;

                    default:
                        goto Label_007E;
                }
            Label_004C:
                num2 = num3;
                switch (num)
                {
                    case 0:
                        goto Label_007E;

                    case 1:
                        goto Label_0028;
                }
                Label_004C_go = false;
                
        }
        catch (Exception exception1) //when (?)
        {
            ProjectData.SetProjectError(exception1);
            Label_004C_go = true;
            goto Label_RETY;
        }
        Label_007E:
            throw ProjectData.CreateProjectError(-2146828237);
        Label_0089:
            if (num2 != 0)
            {
                ProjectData.ClearProjectError();
            }
    }

	public void WaitForThreads()
	{
		while (this.ThreadCount >= this.MaxThreadCount || this.Status == global::ThreadPool.ThreadStatus.Paused)
		{
			if (this.Status == global::ThreadPool.ThreadStatus.Stopped)
			{
				return;
			}
			Thread.Sleep(100);
			Application.DoEvents();
		}
	}

	public bool FreeSlot()
	{
		return this.ThreadCount < this.MaxThreadCount;
	}

	public void AbortThreads()
    {
        int num2 = 0;
        bool Label_009E_try = false;

        Label_009E:
            try
            {
                int num3;
                Label_0000:
                    ProjectData.ClearProjectError();
                    int num = 1;
                    goto Label_002F;
                Label_0009:
                    num3 = 4;
                    Thread item = this.__ActiveThreads[0];
                Label_0018:
                    num3 = 5;
                    item.Abort();
                Label_0020:
                    num3 = 6;
                    this.__ActiveThreads.Remove(item);
                Label_002F:
                    num3 = 3;
                    if (this.__ActiveThreads.Count > 0)
                    {
                        goto Label_0009;
                    }
                Label_003F:
                    num3 = 8;
                    this.Status = ThreadStatus.Stopped;
                Label_0048:
                    num3 = 9;
                    this.__AllPushed = true;
                Label_0052:
                    num3 = 10;
                    this.__ActiveThreads.Clear();
                    goto Label_00DB;
                Label_0062:
                    num2 = 0;
                    switch ((num2 + 1))
                    {
                        case 1:
                            goto Label_0000;

                        case 2:
                        case 3:
                        case 7:
                            goto Label_002F;

                        case 4:
                            goto Label_0009;

                        case 5:
                            goto Label_0018;

                        case 6:
                            goto Label_0020;

                        case 8:
                            goto Label_003F;

                        case 9:
                            goto Label_0048;

                        case 10:
                            goto Label_0052;

                        case 11:
                            goto Label_00DB;

                        default:
                            goto Label_00D0;
                    }
                    // Normalement  Label_009E:
                    if (Label_009E_try)
                    {
                        num2 = num3;
                        switch (num)
                        {
                            case 0:
                                goto Label_00D0;

                            case 1:
                                goto Label_0062;
                        }
                    }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                goto Label_009ERED;
            }
	    Label_00D0:
	        throw ProjectData.CreateProjectError(-2146828237);
	    Label_00DB:
	        if (num2 != 0)
	        {
	            ProjectData.ClearProjectError();
	        }
        Label_009ERED:
            Label_009E_try = true;
            goto Label_009E;
    }
}
