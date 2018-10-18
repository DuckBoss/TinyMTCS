# TinyMTCS
C#/C-Sharp Port of the TinyMT Project from RobertGBryan's VC++ port (who ported it to VC++ from the original authors Saito/Matsumoto).

## TinyMTCS - Tiny Mersenne Twister C-Sharp
This is a 2^127-1 period PRNG.

## About TinyMTCS
This implementation is a direct C# port of the VS++ port by <a href="https://github.com/RobertGBryan">RobertGBryan</a>. <br>
RobertGBryan ported the original C based TinyMT project by Saito and Matsumoto from 2011 to VS++. <br>
I then ported RobertGBryan's VS++ port to C#/C-Sharp. <br>

For more information on TinyMT(Saito/Matsumoto), visit : <br>
<a  href="http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/TINYMT/index.html">http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/TINYMT/index.html</a>. <br>
For more information on TinyMTCPP (RobertGBryan), visit : <br>
<a href="https://github.com/RobertGBryan/TinyMTCPP"> https://github.com/RobertGBryan/TinyMTCPP</a> <br>

## Usage Example
```
using TinyMTCS;

//default values...
public static UInt32 Seed = 1;
public static UInt32 Mat1 = 0x8f7011ee;
public static UInt32 Mat2 = 0xfc78ff1f;
public static UInt32 TMat = 0x3793fdff;

public static void Main(string[] args)
{  
  //Initialize the TinyMT32 class and pass through variables...
  TinyMT32 TMT = new TinyMT32(Seed, Mat1, Mat2, TMat);
  UInt32 result = TMT.GetRandInt();
  
  //Print result...
  Console.WriteLine(result);
}
```

## Key Features
- Sequences are 2^(127)-1.
- Minimal memory impact of 127 bits. (original Mersenne Twister uses 2.5 KiB - 2^(19937)-1)
- Ideal for memory-saving situations.

## Important Changes
- 'TinyMT32_Mul' set to (1.0f / 16777216.0f) as intended by the original TinyMT project.
