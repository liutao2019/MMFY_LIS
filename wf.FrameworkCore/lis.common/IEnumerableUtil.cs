using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace dcl.common
{
    public class IEnumerableUtil
    {
        public static int Count(IEnumerable list)
        {
            int count = 0;
            if (list != null)
            {
                foreach (var item in list)
                {
                    count++;
                }
            }
            return count;
        }

        public static decimal Max(IEnumerable<decimal> list)
        {
            decimal maxValue = 0;

            int count = 0;
            foreach (decimal item in list)
            {
                if (count == 0)
                {
                    maxValue = item;
                }
                else
                {
                    if (item > maxValue)
                    {
                        maxValue = item;
                    }
                }
                count++;
            }
            return maxValue;
        }

        public static decimal Min(IEnumerable<decimal> list)
        {
            decimal minValue = 0;

            int count = 0;
            foreach (decimal item in list)
            {
                if (count == 0)
                {
                    minValue = item;
                }
                else
                {
                    if (item < minValue)
                    {
                        minValue = item;
                    }
                }
                count++;
            }
            return minValue;
        }

        public static T First<T>(IEnumerable<T> list)
        {
            //if (Count(list) == 0)
            //{
            //    throw new Exception("数组集合为零");
            //}

            foreach (T item in list)
            {
                return item;
            }

            throw new Exception("数组集合为零");
        }
    }
}
