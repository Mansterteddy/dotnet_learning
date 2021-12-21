using System;
using DLISInferenceV7;
using DLISInferenceV8;

namespace trt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            RankLMV7InferCsAPI.Init("./trt7/post_trt_v7.ini");

            /*
            RankLMV8InferCsAPI v8_infer = new RankLMV8InferCsAPI();
            v8_infer.Init("./trt8/post_trt.engine");
            v8_infer.Dispose();*/
        }
    }
}
