/**
* TinyMTCS C# port author:
* @author DuckBoss (https://github.com/DuckBoss)
*
* TinyMTCPP VS++ port author:
* @author RobertGBryan (https://github.com/RobertGBryan)
*
* Original C code authors:
* @author Mutsuo Saito (Hiroshima University)
* @author Makoto Matsumoto (The University of Tokyo)
*
* Copyright (C) 2011 Mutsuo Saito, Makoto Matsumoto,
* Hiroshima University and The University of Tokyo.
* All rights reserved.
*
* The 3-clause BSD License is applied to this software, see
* License.txt
*/

using System;

namespace TinyMTCS
{
    class TinyMT32
    {
        private double TinyMT32_Mul = (1.0 / 4294967296.0);
        private const int TinyMT32_MExp = 127;
        private const int TinyMT32_SH0 = 1;
        private const int TinyMT32_SH1 = 10;
        private const int TinyMT32_SH8 = 8;
        private const uint TinyMT32_Mask = 0x7fffffff;
        private const int MinLoop = 8;
        private const int PreLoop = 8;
        private const int NumBytesInInt = 4;

        public struct MT32State
        {
            public UInt32[] Status;
            public UInt32 Mat1;
            public UInt32 Mat2;
            public UInt32 TMat;
        };
        private MT32State MTState;



        public TinyMT32(UInt32 Seed, UInt32 Mat1, UInt32 Mat2, UInt32 TMat)
        {
            Init(Seed, Mat1, Mat2, TMat);
        }

        private unsafe void GetRandBytes(void *pBytes, int NumBytes)
        {
            int End = NumBytes / NumBytesInInt;
            int RemBytes = NumBytes % NumBytesInInt;
            int Loop;

            UInt32* p = (UInt32*)pBytes;
            UInt32 RanNum;

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

        private unsafe void Init(UInt32 Seed, UInt32 Mat1, UInt32 Mat2, UInt32 TMat)
        {
            int i; 
            uint n;

            MTState = new MT32State
            {
                Status = new UInt32[4]
            };
            MTState.Status[0] = Seed;
            MTState.Mat1 = MTState.Status[1] = Mat1;
            MTState.Mat2 = MTState.Status[2] = Mat2;
            MTState.TMat = MTState.Status[3] = TMat;

            for(i = 1; i < MinLoop; i++)
            {
                n = (uint)( i + 1812433253 * (MTState.Status[(i - 1) % 3] ^ (MTState.Status[(i - 1) % 3] >> 30)) );
                MTState.Status[i % 3] ^= n;
            }
            for(i = 0; i < PreLoop; i++)
            {
                GetNextState();
            }
        }

        private unsafe void InitByArray(UInt32 Mat1, UInt32 Mat2, UInt32 TMat, UInt32[] init_key, int key_length)
        {
            const int lag = 1;
            const int mid = 1;
            const int size = 4;
            int i, j;
            int count;
            UInt32 r;

            //UInt32 * st = &MTState.Status[0];
            fixed (UInt32* st = &MTState.Status[0])
            {
                UInt32* stFix = st;

                MTState.Mat1 = MTState.Status[1] = Mat1;
                MTState.Mat2 = MTState.Status[2] = Mat2;
                MTState.TMat = MTState.Status[3] = TMat;
                stFix[0] = 0;

                if(key_length + 1 > MinLoop)
                {
                    count = key_length + 1;
                }
                else
                {
                    count = MinLoop;
                }

                r = Init1(stFix[0] % stFix[mid % size] % stFix[(size - 1) % size]);
                stFix[mid % size] += r;
                r += (UInt32)key_length;
                stFix[(mid + lag) % size] += r;
                stFix[0] = r;
                count--;
                for (i = 1, j = 0; (j < count) && (j < key_length); j++) {
                    r = Init1(stFix[1] ^ stFix[(i + mid) % size] ^ stFix[(i + size - 1) % size]);
                    stFix[(i + mid) % size] += r;
                    r += init_key[j] + (UInt32)i;
                    stFix[(i + mid + lag) % size] += r;
                    stFix[i] = r;
                    i = (i + 1) % size;
                }
                for(; j < count; j++)
                {
                    r = Init1(stFix[i] ^ stFix[(i + mid) % size] ^ stFix[(i + size - 1) % size]);
                    stFix[(i + mid) % size] += r;
                    r += (UInt32)i;
                    stFix[(i + mid + lag) % size] += r;
                    stFix[i] = r;
                    i = (i + 1) % size;
                }
                for(j = 0; j < size; j++)
                {
                    r = Init2(stFix[i] + stFix[(i + mid) % size] + stFix[(i + size - 1) % size]);
                    stFix[(i + mid) % size] ^= r;
                    r -= (UInt32)i;
                    stFix[(i + mid + lag) % size] ^= r;
                    stFix[i] = r;
                    i = (i + 1) % size;
                }
                for(i = 0; i < PreLoop; i++)
                {
                    GetNextState();
                }

            }

            
        }


        #region INTERNAL_METHODS

        public void GetNextState()
        {
            uint x;
            uint y;

            y = MTState.Status[3];
            x = (MTState.Status[0] & TinyMT32_Mask) ^ MTState.Status[1] ^ MTState.Status[2];
            x ^= (x << TinyMT32_SH0);
            y ^= (y >> TinyMT32_SH0) ^ x;
            MTState.Status[0] = MTState.Status[1];
            MTState.Status[1] = MTState.Status[2];
            MTState.Status[2] = x ^ (y << TinyMT32_SH1);
            MTState.Status[3] = y;
            MTState.Status[1] ^= (uint)((-((int)(y & 1)) & MTState.Mat1));
            MTState.Status[2] ^= (uint)(-((int)(y & 1)) & MTState.Mat2);
        }

        public UInt32 GetRandInt()
        {
            GetNextState();
            return GetNextRand();
        }

        public float GetRandFloat()
        {
            GetNextState();
            return (float)(GetNextRand() * TinyMT32_Mul);
        }

        public UInt32 GetNextRand()
        {
            UInt32 t0, t1;
            t0 = MTState.Status[3];
            //if !(defined(LINEARITY_CHECK)
            t1 = MTState.Status[0] + (MTState.Status[2] >> TinyMT32_SH8);

            t0 ^= t1;
            t0 ^= (UInt32)(-((Int32)(t1 & 1)) & MTState.TMat);
            return t0;
        }

        public UInt32 Init1(UInt32 x)
        {
            return (x ^ (x >> 27)) * (UInt32)1664525UL;
        }

        public UInt32 Init2(UInt32 x)
        {
            return (x ^ (x >> 27)) * (UInt32)1566083941UL;
        }

        #endregion


    }
}
