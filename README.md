# TinyMTCS
C#/C-Sharp (32/64-bit) Port of the TinyMT Project from RobertGBryan's VC++ port (who ported it to VC++ from the original authors Saito/Matsumoto).

## TinyMTCS - Tiny Mersenne Twister C-Sharp
This is both a 32-bit and 64-bit implementation of the TinyMT project. It is a 2^(127)-1 period PRNG.

## About TinyMTCS
This implementation is a direct C# port of the VS++ port by <a href="https://github.com/RobertGBryan">RobertGBryan</a>. <br>
RobertGBryan ported the original C based TinyMT project by Saito and Matsumoto from 2011 to VS++. <br>
I then ported RobertGBryan's VS++ port to C#/C-Sharp. <br>

For more information on TinyMT(Saito/Matsumoto), visit : <br>
<a  href="http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/TINYMT/index.html">http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/TINYMT/index.html</a>. <br>
For more information on TinyMTCPP (RobertGBryan), visit : <br>
<a href="https://github.com/RobertGBryan/TinyMTCPP"> https://github.com/RobertGBryan/TinyMTCPP</a> <br>

## Usage Example
- 32-Bit Example:
```
using TinyMTCS;

//default values...
public static UInt32 Seed_32 = 1;
public static UInt32 Mat1_32 = 0x8f7011ee;
public static UInt32 Mat2_32 = 0xfc78ff1f;
public static UInt32 TMat_32 = 0x3793fdff;

public static void Main(string[] args)
{  
  //Initialize the TinyMT32 class and pass through variables...
  TinyMT32 TMT32 = new TinyMT32(Seed_32, Mat1_32, Mat2_32, TMat_32);
  UInt32 result_#2 = TMT32.GetRandInt();
  
  //Print result...
  Console.WriteLine(result_32);
}
```
- 64-Bit Example:
```
using TinyMTCS;

//default values...
public static UInt64 Seed_64 = 1;
public static UInt32 Mat1_64 = 0xfa051f40;
public static UInt32 Mat2_64 = 0xffd0fff4;
public static UInt64 TMat_64 = 0x58d02ffeffbfffbc;

public static void Main(string[] args)
{  
  //Initialize the TinyMT32 class and pass through variables...
  TinyMT64 TMT64 = new TinyMT64(Seed_64, Mat1_64, Mat2_64, TMat_64);
  UInt64 result_64 = TMT64.GetRandInt();
  
  //Print result...
  Console.WriteLine(result_64);
}
```

## Key Features
- The periods of generated sequences are 2^(127)-1.
- The size of internal state space is 127 bits.
- The state transition function is F2-linear.
- The output function is not F2-linear.
- TinyMTDC generates distinct parameter sets for TinyMT.
- TinyMTDC can generate a large number of parameter sets. (over 2^(32) x 2^(16))
- Parameter generation of TinyMTDC is fast.

## Important Notes
- TinyMTCS requires the compiler to compile unsafe code.
