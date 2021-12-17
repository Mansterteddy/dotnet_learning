using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;

namespace process_c
{
    class Program
    {

        [DllImport("deepgpu_engine.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static extern int EngineExecutorInit(IntPtr engineFileName);

        [DllImport("deepgpu_engine.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EngineExecutorInfer4RankLMTask(int handle, int batch_size, IntPtr click_cls_output, IntPtr cls_output, IntPtr res, int portCount);

        [DllImport("deepgpu_engine.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern void EngineExecutorDestroy(int engine_handle);

        // Process C:
        static void Main(string[] args)
        {
            try
            {
                var post_processing_engine_filename = "./trt8/post_trt.engine";
                GCHandle h_post_trt_engine = GCHandle.Alloc(Encoding.ASCII.GetBytes(post_processing_engine_filename), GCHandleType.Pinned);
                Console.WriteLine("Debug: " + post_processing_engine_filename);
                int engine_handle = EngineExecutorInit(h_post_trt_engine.AddrOfPinnedObject());
                h_post_trt_engine.Free();

                Console.WriteLine("Complete Init Model");


                using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("testmap"))
                {

                    Mutex mutex = Mutex.OpenExisting("testmapmutex");
                    mutex.WaitOne();

                    using (MemoryMappedViewStream stream = mmf.CreateViewStream(2, 0))
                    {
                        BinaryWriter writer = new BinaryWriter(stream);
                        writer.Write(1);
                    }
                    mutex.ReleaseMutex();
                }

                EngineExecutorDestroy(engine_handle);
                Console.WriteLine("Destroy TRT Engine");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Memory-mapped file does not exist. Run Process A first, then B.");
            }
        }
    }
}
