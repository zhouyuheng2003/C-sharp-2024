using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomSelectorForQuestions
{
    internal class RandomNumberGenerator
    {
        private static RandomNumberGenerator instance;
        private RandomNumberGenerator() { }
        private long seed, tot, x;
        static public void init() { instance = new RandomNumberGenerator(); }
        static public void setSeed(long seed) { instance.seed = seed; reset(); }
        static public void reset() { instance.x = instance.seed; instance.tot = 0; }
        static public long getSeed() { return instance.tot; }
        static public int getNext() {
            instance.x = ( instance.x * 314159269 + 453806245 ) % 2147483648;
            instance.tot++;
            return (int)instance.x; 
        }
    }
}
