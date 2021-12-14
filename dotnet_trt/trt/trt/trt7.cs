using System;
using System.Text;
using System.Runtime.InteropServices;

namespace DLISInferenceV7
{
    public class RankLMV7InferCsAPI
    {

        [DllImport("trt7/post_trt_v7.dll", EntryPoint = "RankLM_Post_Processing_Init_V7")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern void RankLM_Post_Processing_Init_V7(IntPtr configFileName);

        [DllImport("trt7/post_trt_v7.dll", EntryPoint = "RankLM_Post_Processing_Inference_V7")]
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

    }
}
