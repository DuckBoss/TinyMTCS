using System;

namespace TinyMTCS
{
    class TinyMT32_Test
    {

        public static UInt32 Seed = 1;
        public static UInt32 Mat1 = 0x8f7011ee;
        public static UInt32 Mat2 = 0xfc78ff1f;
        public static UInt32 TMat = 0x3793fdff;

        public static void Main(string[] args)
        {
            
            Seed = (UInt32)(Guid.NewGuid().GetHashCode());
            Mat1 = (UInt32)(Guid.NewGuid().GetHashCode());
            Mat2 = (UInt32)(Guid.NewGuid().GetHashCode());
            TMat = (UInt32)(Guid.NewGuid().GetHashCode());
            

            TinyMT32 TMT = new TinyMT32(Seed, Mat1, Mat2, TMat);
            UInt32 result = TMT.GetRandInt();

            Console.WriteLine(result);
        }



    }
}
