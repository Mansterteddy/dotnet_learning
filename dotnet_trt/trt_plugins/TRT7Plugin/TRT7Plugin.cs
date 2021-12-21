using PluginBase;
using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;


namespace TRT7Plugin
{
    public class TRT7Plugin : ICommand
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


        public string Name => "TRT7";

        public string Description => "TRT7 Enginne Loading.";

        public int InitEngine()
        {
            Init("./trt7/post_trt_v7.ini");
            return 0;
        }

    }
}