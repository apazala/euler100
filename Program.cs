﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EulerHacker100cs
{
    class Acoeff
    {
        public long a0;
        public List<long> aperiod;
        public Acoeff(long a0)
        {
            this.a0 = a0;
            aperiod = new List<long>();
        }

        public long GetK(int k)
        {

            if (k <= aperiod.Count) return (k == 0) ? a0 : aperiod[k - 1];

            return aperiod[(k - 1) % aperiod.Count];
        }

    }
    class Program
    {
        static Acoeff ContinuedFractionPeriod(long d)
        {
            long a0, a, b;
            a0 = (int)Math.Sqrt(d);

            long c = d - a0 * a0;
            if (c == 0) return null;

            Acoeff ac = new Acoeff(a0);
            
            b = a0;
            long a02 = a0 << 1;
            do
            {
                a = (a0 + b) / c;
                b = a * c - b;
                c = (d - b * b) / c;
                ac.aperiod.Add(a);
            } while (a != a02);

            return ac;
        }

        private static void Solve(int p, int q, long d)
        {
            Acoeff ac = ContinuedFractionPeriod(p*(long)q);
            if (ac == null)
            {
                Console.WriteLine("No solution");
                return;
            }

            BigInteger u1 = 0;
            BigInteger u = 1;
            BigInteger u2;

            BigInteger v1 = 1;
            BigInteger v = 0;
            BigInteger v2;

            int period = ac.aperiod.Count;
            int ktarget = ((period & 1) > 0 ? 2 * period - 1 : period - 1);

            //minimal Pellanas
            for (int k = 0; k <= ktarget; k++)
            {
                u2 = u1; u1 = u; v2 = v1; v1 = v;
                long ak = ac.GetK(k);
                u = u1 * ak + u2;
                v = v1 * ak + v2;
            }

            BigInteger u11 = u, v11 = v, dy1 = p*(BigInteger)q*v;

            
            BigInteger m = (u + p * v + 1) / 2;
            BigInteger n = (u + q * v + 1) / 2;
            while (n < d)
            {
                //next iteration
                u1 = u; v1 = v;
                u = u11 * u1 + dy1 * v1;
                v = v11 * u1 + u11 * v1;

                m = (u + p * v + 1) / 2;
                n = (u + q * v + 1) / 2;
            }

            Console.WriteLine($"{m} {n}");

        }

        static void Main(string[] args)
        {
            
            int p = 1;
            int q = 2;
            long d = 1_000_000_000_000l;

            Solve(p, q, d);
            

            return;
        }


    }
}