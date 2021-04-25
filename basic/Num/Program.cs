using System;
using System.Buffers;

namespace Num
{
    class Program
    {
        public static void addValue(int a)
        {
            a += 10;
        }

        public static void substractValue(ref int b)
        {
            b -= 5;
        }

        static void UseSharedArray(int x)
        {
            var shared = ArrayPool<int>.Shared;
            var rentedArray = shared.Rent(5);

            rentedArray[0] = 0;
            rentedArray[1] = 1 * x;
            rentedArray[2] = 2 * x;
            rentedArray[3] = 3 * x;
            rentedArray[4] = 4 * x;

            for(int i = 0; i < 5; i++)
            {
                Console.WriteLine("ARRAY {0}: {1}", x, rentedArray[i]);
            }

            shared.Return(rentedArray);
        }

        static void Main(string[] args)
        {

            int a = 10, b = 12;

            Console.WriteLine("Inital value of a is {0}", a);
            Console.WriteLine("Initial value of b is {0}", b);
            Console.WriteLine();

            addValue(a);
            Console.WriteLine("Value of a after addition" + " operation is {0}", a);

            substractValue(ref b);
            Console.WriteLine("Value of b after substraction" + " operation is {0}", b);

            UseSharedArray(1);
            UseSharedArray(2);
            UseSharedArray(3);

        }
    }
}
