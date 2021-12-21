using PluginBase;
using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TRT8Plugin
{
    public class TRT8Plugin : ICommand
    {
        private int engine_handle { get; set; }

        [DllImport("deepgpu_engine.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static extern int EngineExecutorInit(IntPtr engineFileName);

        [DllImport("deepgpu_engine.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EngineExecutorInfer4RankLMTask(int handle, int batch_size, IntPtr click_cls_output, IntPtr cls_output, IntPtr res, int portCount);

        [DllImport("deepgpu_engine.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern void EngineExecutorDestroy(int engine_handle);

        public void Init(string post_processing_engine_filename)
        {

            GCHandle h_post_trt_engine = GCHandle.Alloc(Encoding.ASCII.GetBytes(post_processing_engine_filename), GCHandleType.Pinned);
            Console.WriteLine("Debug: ");
            Console.WriteLine(post_processing_engine_filename);
            engine_handle = EngineExecutorInit(h_post_trt_engine.AddrOfPinnedObject());
            h_post_trt_engine.Free();

            Console.WriteLine("Complete Init Model");
        }

        public void Dispose()
        {
            EngineExecutorDestroy(engine_handle);
            Console.WriteLine("Destroy TRT Engine");
        }

        public string Name => "TRT8";

        public string Description => "TRT8 Enginne Loading.";


        public int InitEngine()
        {
            Init("./trt8/post_trt.engine");
            Dispose();
            return 0;
        }
    }
}