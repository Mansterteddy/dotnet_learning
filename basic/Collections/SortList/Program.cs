using System;
using System.Collections.Generic;

namespace SortList
{

    public class DuplicateKeyComparer : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            if (x == y)
                return 1;
            else
                return x.CompareTo(y);
        }
    };


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SortedList<double, int> sorted_list = new SortedList<double, int>(new DuplicateKeyComparer());

            sorted_list.Add(-4.4, 4);
            sorted_list.Add(2.2, 2);
            sorted_list.Add(5.5, 5);
            sorted_list.Add(1.1, 1);
            sorted_list.Add(3.3, 3);
            
            sorted_list.Add(3.3, 5);
            sorted_list.Add(-4.4, 7);

            foreach (KeyValuePair<double, int> kvp in sorted_list)
            {
                Console.WriteLine("key = {0}, value = {1}", kvp.Key, kvp.Value);
            }

        }
    }
}
