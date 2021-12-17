using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;

namespace process_b
{
    class Program
    {
        [DllImport("post_trt_v7.dll", EntryPoint = "RankLM_Post_Processing_Init_V7", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern void RankLM_Post_Processing_Init_V7(IntPtr configFileName);

        [DllImport("post_trt_v7.dll", EntryPoint = "RankLM_Post_Processing_Inference_V7", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern void RankLM_Post_Processing_Inference_V7(IntPtr cls_vector, IntPtr fresh_index_list, int batch_size,
                            IntPtr onedcg_output, IntPtr onedcg_logits,
                            IntPtr effortless_output, IntPtr effortless_logits,
                            IntPtr location_output, IntPtr location_logits,
                            IntPtr qc_output, IntPtr qc_logits,
                            IntPtr fresh_output, IntPtr fresh_logits,
                            IntPtr click_score);

        public static void Init(string post_processing_filename)
        {
            GCHandle h_post_processing_text_bytes = GCHandle.Alloc(Encoding.ASCII.GetBytes(post_processing_filename), GCHandleType.Pinned);
            RankLM_Post_Processing_Init_V7(h_post_processing_text_bytes.AddrOfPinnedObject());
            h_post_processing_text_bytes.Free();
        }

        // Process B:
        static void Main(string[] args)
        {
            try
            {
                Init("./trt7/post_trt_v7.ini");

                using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("testmap"))
                {

                    Mutex mutex = Mutex.OpenExisting("testmapmutex");
                    mutex.WaitOne();

                    using (MemoryMappedViewStream stream = mmf.CreateViewStream(1, 0))
                    {
                        BinaryWriter writer = new BinaryWriter(stream);
                        writer.Write(0);
                    }
                    mutex.ReleaseMutex();
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Memory-mapped file does not exist. Run Process A first.");
            }
        }
    }
}
