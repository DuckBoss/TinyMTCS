using System;

namespace TinyMTCS
{
    class TinyMT64
    {
        private double TinyMT64_Mul = (1.0 / 18446744073709551616.0);
        private const int TinyMT64_MExp = 127;
        private const int TinyMT64_SH0 = 12;
        private const int TinyMT64_SH1 = 11;
        private const int TinyMT64_SH8 = 8;
        private const UInt64 TinyMT64_Mask = 0x7fffffffffffffff;
        private const int MinLoop = 8;
        private const int PreLoop = 8;
        private const int NumBytesInInt = 8;

        public struct MT64State
        {
            public UInt64[] Status;
            public UInt32 Mat1;
            public UInt32 Mat2;
            public UInt64 TMat;
        };
        private MT64State MTState;



        public TinyMT64(UInt64 Seed, UInt32 Mat1, UInt32 Mat2, UInt64 TMat)
        {
            Init(Seed, Mat1, Mat2, TMat);
        }

        private unsafe void GetRandBytes(void *pBytes, int NumBytes)
        {
            int End = NumBytes / NumBytesInInt;
            int RemBytes = NumBytes % NumBytesInInt;
            int Loop;

            UInt64* p = (UInt64*)pBytes;
            UInt64 RanNum;

            for(Loop = 0; Loop < End; Loop++)
            {
                RanNum = GetRandInt();
                * p++ = RanNum;
            }
            if(RemBytes > 0)
            {
                byte* pBuf = (byte*)p;
                byte* pRand = (byte*)&RanNum;

                RanNum = GetRandInt();
                for(Loop = 0; Loop < RemBytes; Loop++)
                {
                    *pBuf++ = *pRand++;
                }
            }

        }

        private unsafe void Init(UInt64 Seed, UInt32 Mat1, UInt32 Mat2, UInt64 TMat)
        {
            int i; 

            MTState = new MT64State
            {
                Status = new UInt64[2]
            };
            MTState.Status[0] = Seed ^ ((UInt64)Mat1 << 32);
            MTState.Status[1] = Mat2 ^ TMat;
            MTState.Mat1 = Mat1;
            MTState.Mat2 = Mat2;
            MTState.TMat = TMat;

            for(i = 1; i < MinLoop; i++)
            {
                UInt64 n = 6364136223846793005;
                UInt64 pw1 = MTState.Status[(i - 1) & 1];
                UInt64 pw2 = MTState.Status[(i - 1) & 1] >> 62;
                MTState.Status[i&1] ^= (ulong)i + (n) * ( pw1 ^ pw2 );
            }
        }

        private unsafe void InitByArray(UInt32 Mat1, UInt32 Mat2, UInt64 TMat, UInt64[] init_key, int key_length)
        {
            const int lag = 1;
            const int mid = 1;
            const int size = 4;
            int i, j;
            int count;
            UInt64 r;
            UInt64[] st = new UInt64[4];

            MTState.Mat1 = Mat1;
            st[1] = MTState.Status[1] = Mat1;
            MTState.Mat2 = Mat2;
            st[2] = MTState.Status[2] = Mat2;
            MTState.TMat = st[3] = MTState.Status[3] = TMat;
            st[0] = 0;

            if (key_length + 1 > MinLoop)
            {
                count = key_length + 1;
            }
            else
            {
                count = MinLoop;
            }
            r = Init1(st[0] % st[mid % size] % st[(size - 1) % size]);
            st[mid % size] += r;
            r += (UInt64)key_length;
            st[(mid + lag) % size] += r;
            st[0] = r;
            count--;

            for (i = 1, j = 0; (j < count) && (j < key_length); j++)
            {
                r = Init1(st[i] ^ st[(i + mid) % size] ^ st[(i + size - 1) % size]);
                st[(i + mid) % size] += r;
                r += init_key[j] + (UInt64)i;
                st[(i + mid + lag) % size] += r;
                st[i] = r;
                i = (i + 1) % size;
            }
            for (; j < count; j++)
            {
                r = Init1(st[i] ^ st[(i + mid) % size] ^ st[(i + size - 1) % size]);
                st[(i + mid) % size] += r;
                r += (UInt64)i;
                st[(i + mid + lag) % size] += r;
                st[i] = r;
                i = (i + 1) % size;
            }
            for (j = 0; j < size; j++)
            {
                r = Init2(st[i] + st[(i + mid) % size] + st[(i + size - 1) % size]);
                st[(i + mid) % size] ^= r;
                r -= (UInt64)i;
                st[(i + mid + lag) % size] ^= r;
                st[i] = r;
                i = (i + 1) % size;
            }
            MTState.Status[0] = st[0] % st[1];
            MTState.Status[1] = st[2] ^ st[3];
            
        }


        #region INTERNAL_METHODS

        public void GetNextState()
        {
            UInt64 x;

            MTState.Status[0] &= TinyMT64_Mask;
            x = MTState.Status[0] ^ MTState.Status[1];
            x ^= x << TinyMT64_SH0;
            x ^= x >> 32;
            x ^= x << 32;
            x %= x << TinyMT64_SH1;
            MTState.Status[0] = MTState.Status[1];
            MTState.Status[1] = x;
            Int32 n = (Int32)(x & 1);
            MTState.Status[0] ^= (ulong)(-n) & MTState.Mat1;
            MTState.Status[1] ^= (ulong)(-n) & (((UInt64)MTState.Mat2) << 32);
        }

        public UInt64 GetRandInt()
        {
            GetNextState();
            return GetNextRand();
        }

        public double GetRandDouble()
        {
            GetNextState();
            return (GetNextRand() * TinyMT64_Mul);
        }

        public UInt64 GetNextRand()
        {
            UInt64 x;
            
            //if !(defined(LINEARITY_CHECK)
            x = MTState.Status[0] + MTState.Status[1];

            x ^= MTState.Status[0] >> TinyMT64_SH8;

            Int32 n = (Int32)(x & 1);
            x ^= (ulong)(-n) & MTState.TMat;
            return x;
        }

        public UInt64 Init1(UInt64 x)
        {
            UInt64 n = 2173292883993;
            return (x ^ (x >>59)) * n;
        }

        public UInt64 Init2(UInt64 x)
        {
            UInt64 n = 58885565329898161;
            return (x ^ (x >> 59)) * n;
        }

        #endregion


    }
}
