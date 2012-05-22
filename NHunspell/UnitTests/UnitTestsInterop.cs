using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Text;



[TestFixture]
public class InteropTests
{
    IntPtr dllHandle = IntPtr.Zero;

    public delegate void InteropVoidDelegate();
    public InteropVoidDelegate InteropVoid;

    public delegate void InteropCharDelegate([MarshalAs(UnmanagedType.LPStr)] string parameter);
    public InteropCharDelegate InteropChar;

    public  delegate void InteropWCharDelegate([MarshalAs(UnmanagedType.LPWStr)] string parameter);
    public InteropWCharDelegate InteropWChar;

    public  delegate void InteropWCharAndMultByteConvertDelegate([MarshalAs(UnmanagedType.LPWStr)] string parameter);
    public InteropWCharAndMultByteConvertDelegate InteropWCharAndMultByteConvert;

    public delegate void InteropWCharAndMultByteConvertNewDeleteDelegate([MarshalAs(UnmanagedType.LPWStr)] string parameter);
    public InteropWCharAndMultByteConvertNewDeleteDelegate InteropWCharAndMultByteConvertNewDelete;

    public delegate IntPtr InteropStaticWCharReturnDelegate();
    public InteropStaticWCharReturnDelegate InteropStaticWCharReturn;

    public delegate IntPtr InteropStaticCharReturnDelegate();
    public InteropStaticCharReturnDelegate InteropStaticCharReturn;

    public delegate string InteropAlocatedWCharReturnDelegate();
    public InteropAlocatedWCharReturnDelegate InteropAlocatedWCharReturn;

    public delegate IntPtr InteropNewWCharReturnDelegate();
    public InteropNewWCharReturnDelegate InteropNewWCharReturn;

    public delegate void InteropRefWCharReturnDelegate([MarshalAs(UnmanagedType.LPWStr)] ref StringBuilder parameter);
    public InteropRefWCharReturnDelegate InteropRefWCharReturn;

    public delegate void InteropStringArrayDelegate(out IntPtr buffer );

    public InteropStringArrayDelegate InteropStringArray;

    [TestFixtureSetUp]
    public void Init()
    {
        try
        {
            // Initialze the dynamic marshall Infrastructure to call the 32Bit (x86) or the 64Bit (x64) Dll respectively 
            SYSTEM_INFO info = new SYSTEM_INFO();
            GetSystemInfo(ref info);

            // Load the correct DLL according to the processor architecture
            switch (info.wProcessorArchitecture)
            {
                case PROCESSOR_ARCHITECTURE.Intel:
                    dllHandle = LoadLibrary("ManagedUnmanagedInteropTestsx32.dll");
                    if (dllHandle == null)
                        throw new DllNotFoundException("Interop x86 Code (ManagedUnmanagedInteropTestsx32.dll) not found");
                    break;

                case PROCESSOR_ARCHITECTURE.Amd64:
                    dllHandle = LoadLibrary("ManagedUnmanagedInteropTestsx64.dll");
                    if (dllHandle == null)
                        throw new DllNotFoundException("Interop x64 Code (ManagedUnmanagedInteropTestsx64.dll) not found");
                    break;

                default:
                    throw new NotSupportedException("NHunspell is not available for ProcessorArchitecture: " + info.wProcessorArchitecture.ToString());
            }

            
            InteropVoid = (InteropVoidDelegate)GetDelegate("InteropVoid", typeof(InteropVoidDelegate));
            InteropWChar = (InteropWCharDelegate)GetDelegate("InteropWChar", typeof(InteropWCharDelegate));
            InteropChar = (InteropCharDelegate)GetDelegate("InteropChar", typeof(InteropCharDelegate));

            InteropWCharAndMultByteConvert = (InteropWCharAndMultByteConvertDelegate)GetDelegate("InteropWCharAndMultByteConvert", typeof(InteropWCharAndMultByteConvertDelegate));
            InteropWCharAndMultByteConvertNewDelete = (InteropWCharAndMultByteConvertNewDeleteDelegate)GetDelegate("InteropWCharAndMultByteConvertNewDelete", typeof(InteropWCharAndMultByteConvertNewDeleteDelegate));

            InteropStaticCharReturn = (InteropStaticCharReturnDelegate)GetDelegate("InteropStaticCharReturn", typeof(InteropStaticCharReturnDelegate));
            InteropStaticWCharReturn = (InteropStaticWCharReturnDelegate)GetDelegate("InteropStaticWCharReturn", typeof(InteropStaticWCharReturnDelegate));

            InteropAlocatedWCharReturn = (InteropAlocatedWCharReturnDelegate)GetDelegate("InteropAlocatedWCharReturn", typeof(InteropAlocatedWCharReturnDelegate));
            InteropNewWCharReturn = (InteropNewWCharReturnDelegate)GetDelegate("InteropNewWCharReturn", typeof(InteropNewWCharReturnDelegate));
            InteropRefWCharReturn = (InteropRefWCharReturnDelegate)GetDelegate("InteropRefWCharReturn", typeof(InteropRefWCharReturnDelegate));

            InteropStringArray = (InteropStringArrayDelegate)GetDelegate("InteropStringArray", typeof(InteropStringArrayDelegate));

        }
        catch (Exception e)
        {
            if (dllHandle == IntPtr.Zero)
                FreeLibrary(dllHandle);

            throw e;
        }
    }


    [Test]
    public void ArrayInteropTests()
    {
        var stopwatch = new Stopwatch();
        const int loopCount = 100000;

        string[] stringarray = { "ONE", "TWO", "THREE", "FOUR" };

        Console.WriteLine("");


        stopwatch.Reset();
        stopwatch.Start();
        int size = 0;
        for (int i = 0; i < loopCount; ++i)
        {
            IntPtr buffer;
            InteropStringArray(out buffer);

            var p0 = Marshal.ReadIntPtr(buffer,IntPtr.Size * 0);
            // var p1 = Marshal.ReadIntPtr(buffer, IntPtr.Size * 1);
            // var p2 = Marshal.ReadIntPtr(buffer, IntPtr.Size * 2);

            var s0 = Marshal.PtrToStringUni(p0);
            // var s1 = Marshal.PtrToStringUni(p1);
            // var s2 = Marshal.PtrToStringUni(p2);

            
        }
        stopwatch.Stop();
        Console.WriteLine("InteropArray....: " + stopwatch.ElapsedTicks.ToString());


    }



    [Test]
    public void StringInteropTests()
    {
        var stopwatch = new Stopwatch();
        string marshallString = "TEST TEST TEST";
        const int loopCount = 100000;


        IntPtr handle = IntPtr.Zero;
        HandleRef handleref = new HandleRef(marshallString, handle);

        Console.WriteLine("");


        // Warmup
        for (int i = 0; i < loopCount; ++i)
            InteropVoid();

        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < loopCount; ++i)
            InteropVoid();
        stopwatch.Stop();
        Console.WriteLine("InteropVoid.....: " + stopwatch.ElapsedTicks.ToString());


        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < loopCount; ++i)
            InteropChar(marshallString);
        stopwatch.Stop();
        Console.WriteLine("InteropChar.....: " + stopwatch.ElapsedTicks.ToString());

        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < loopCount; ++i)
            InteropWChar(marshallString);
        stopwatch.Stop();
        Console.WriteLine("InteropWChar....: " + stopwatch.ElapsedTicks.ToString());

        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < loopCount; ++i)
            InteropWCharAndMultByteConvert(marshallString);
        stopwatch.Stop();
        Console.WriteLine("InteropWCharMB..: " + stopwatch.ElapsedTicks.ToString());


        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < loopCount; ++i)
            InteropWCharAndMultByteConvertNewDelete(marshallString);
        stopwatch.Stop();
        Console.WriteLine("InteropWCharMBND: " + stopwatch.ElapsedTicks.ToString());


        Console.WriteLine("");
        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < loopCount; ++i)
        {
            var result = InteropStaticWCharReturn();
            var managedString = Marshal.PtrToStringUni(result);
            if( managedString.Length == 0 )
                throw new InvalidOperationException();

        }
        stopwatch.Stop();
        Console.WriteLine("InteropSWCR.....: " + stopwatch.ElapsedTicks.ToString());

        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < loopCount; ++i)
        {
            var result = InteropStaticWCharReturn();
            short charValue;
            string managedString ="c";

            while ((charValue = Marshal.ReadInt16(result)) != 0)
            {
                result = new IntPtr(result.ToInt64() + 2);
            }

            if (managedString.Length == 0)
                throw new InvalidOperationException();

        }
        stopwatch.Stop();
        Console.WriteLine("InteropSWCR.....: " + stopwatch.ElapsedTicks.ToString());



        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < loopCount; ++i)
        {
            var result = InteropAlocatedWCharReturn();
            if( result.Length == 0 )
                throw new InvalidOperationException();
        }
        stopwatch.Stop();
        Console.WriteLine("InteropAWCR.....: " + stopwatch.ElapsedTicks.ToString());


        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < loopCount; ++i)
        {
            var result = InteropNewWCharReturn();
            var managedString = Marshal.PtrToStringUni(result);
            if (managedString.Length == 0)
                throw new InvalidOperationException();
        }
        stopwatch.Stop();
        Console.WriteLine("InteropNWCR.....: " + stopwatch.ElapsedTicks.ToString());

        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < loopCount; ++i)
        {
            StringBuilder builder = new StringBuilder("TEST TEST TEST XX");
            InteropRefWCharReturn(ref builder);

            var managedString = builder.ToString();
            if (managedString.Length == 0)
                throw new InvalidOperationException();
        }
        stopwatch.Stop();
        Console.WriteLine("InteropRWCR.....: " + stopwatch.ElapsedTicks.ToString());


    }




    Delegate GetDelegate(string procName, Type delegateType)
    {
        IntPtr procAdress = GetProcAddress(dllHandle, procName);
        if (procAdress == IntPtr.Zero)
            throw new EntryPointNotFoundException("Function: " + procName);

        return Marshal.GetDelegateForFunctionPointer(procAdress, delegateType);

    }


    [DllImport("kernel32.dll")]
    internal static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);

    internal enum PROCESSOR_ARCHITECTURE : ushort
    {
        Intel = 0,
        MIPS = 1,
        Alpha = 2,
        PPC = 3,
        SHX = 4,
        ARM = 5,
        IA64 = 6,
        Alpha64 = 7,
        Amd64 = 9,
        Unknown = 0xFFFF
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct SYSTEM_INFO
    {
        internal PROCESSOR_ARCHITECTURE wProcessorArchitecture;
        internal ushort wReserved;
        internal uint dwPageSize;
        internal IntPtr lpMinimumApplicationAddress;
        internal IntPtr lpMaximumApplicationAddress;
        internal IntPtr dwActiveProcessorMask;
        internal uint dwNumberOfProcessors;
        internal uint dwProcessorType;
        internal uint dwAllocationGranularity;
        internal ushort dwProcessorLevel;
        internal ushort dwProcessorRevision;
    }


    [DllImport("kernel32.dll")]
    internal static extern IntPtr LoadLibrary(string fileName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool FreeLibrary(IntPtr hModule);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);


}