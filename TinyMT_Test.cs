using System;

namespace TinyMTCS
{
    class TinyMT_Test
    {

        public static UInt32 Seed = 1;
        public static UInt32 Mat1 = 0x8f7011ee;
        public static UInt32 Mat2 = 0xfc78ff1f;
        public static UInt32 TMat = 0x3793fdff;

        public static UInt64 Seed_64 = 1;
        public static UInt32 Mat1_64 = 0xfa051f40;
        public static UInt32 Mat2_64 = 0xffd0fff4;
        public static UInt64 TMat_64 = 0x58d02ffeffbfffbc;

        public static void Main(string[] args)
        {
            
            Seed = (UInt32)(Guid.NewGuid().GetHashCode());
            Mat1 = (UInt32)(Guid.NewGuid().GetHashCode());
            Mat2 = (UInt32)(Guid.NewGuid().GetHashCode());
            TMat = (UInt32)(Guid.NewGuid().GetHashCode());

            Seed_64 = (UInt64)(Guid.NewGuid().GetHashCode());
            Mat1_64 = (UInt32)(Guid.NewGuid().GetHashCode());
            Mat2_64 = (UInt32)(Guid.NewGuid().GetHashCode());
            TMat_64 = (UInt64)(Guid.NewGuid().GetHashCode());



            TinyMT32 TMT32 = new TinyMT32(Seed, Mat1, Mat2, TMat);
            UInt32 result_32 = TMT32.GetRandInt();
            Console.WriteLine($"TinyMT32 - {result_32}");

            TinyMT64 TMT64 = new TinyMT64(Seed_64, Mat1_64, Mat2_64, TMat_64);
            UInt64 result_64 = TMT64.GetRandInt();
            Console.WriteLine($"TinyMT64 - {result_64}");
        }



    }
}
