using System;
using System.Text;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DLISInferenceV8
{
    public class RankLMV8InferCsAPI
    {
        private int engine_handle { get; set; }

        [DllImport("trt8/deepgpu_engine.dll")]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static extern int EngineExecutorInit(IntPtr engineFileName);

        [DllImport("trt8/deepgpu_engine.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EngineExecutorInfer4RankLMTask(int handle, int batch_size, IntPtr click_cls_output, IntPtr cls_output, IntPtr res, int portCount);

        [DllImport("trt8/deepgpu_engine.dll")]
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
    }
}
