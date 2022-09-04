using System;
using System.Collections.Generic;

internal sealed partial class hmNETDynamicLib
{
    internal sealed partial class Hidemaru
    {
        internal class HmOriginalEncodeFunc
        {
#if SMALLFILESIZE
            /*
            public static readonly ulong[] encode_zen_han_map_compress = {
0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xbc9ef3f247fe9e7f,0xf040000545d580ff,0x5ff5f5fd3dd57b0f,0xf3dfffffffffffff,0xffffffffffbfffff,0xffffffffffffffff,0xffffffffffffffff,0xfffffffffffffd3f,0xfebffffdfffffff,0xffffe97f4577f7c8,0xfffbffffffffffff,0xffffffffffffffff,0xffcfffffffffffff,0xfb99fffffff7fff7,0xc305d01301450008,0x8400007f2dbbdfde,0xec617fffffffffff,0xff0076798681f800,0x241dfffb7d4fe060,0x859ffffffffcff00,0x7fffffc080000001,0xfff10ff9ffbfffce,0xffed46e48069ffff,0xffe401f7061b78c8,0x83ee65e0fc004000,0xffe95f43bbe00ef5,0,0,0x213e7040,0xfffffffffff,0xe18780010000c42c,0x7f847f003be3c004,0x410000000100000c,0x7804000031003140,0x6000000000000008,0x619c40000000c400,0x600004014210800c,0x7d840000338b8000,0x400810000000000d,0x7804020071837400,0x2000000000000000,0x8004000002000000,0x8000040000000003,0x83bc0600304400e0,0x400000000000000d,0x204000000004000,0x4000000000000000,0x6000032000000,0x10000,0x203a0002100000,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xceffcf66fffbfdcc,0xff7ffffffff87fff,0xffffff7ffffffffb,0x4ef9780000000000,0x80028000107bf66,0x603800e384007800,0x2604010608806408,0x4ec5b45549dff,0,0,0,0,0x1090000000840000,0,0x8400fc0096000000,0x869200c00000,0xab10aa00009292,0xbe0000077d7f6230,0xfa4026dacc89,0x1213b5a6230208ac,0xfffe00000,0x700000021ff000,0x187fc0007ff0001f,0xe0000037fc0a,0x7f0000007fe0003,0xc0fcc00400200014,0x3c005e103ffffff,0xf0000000000279e7,0xf03c0e0c0f00000f,0x300002000020003,0xf39ce7b83bffffcf,0xfffffcfdfbdb8000,0,0,0x2800000dfc,0x27fee5c0000ffc0,0xafbeffeef8ffffff,0xffffffffbfffff00,0xfaffffffffe000a3,0x1c000fff605b5c00,0,0xc0849ff87800,0x10a118224900fde0,0x52c07ae0eec0eec0,0x8400080300000000,0,0,0,0,0,0,0,0,0x7f0081c200ff,0x8418,0xffffff8548c0,0x9ffb87fdbfffffff,0xffffffffffeeffdf,0xfdfffbffffffffff,0xffe4000000000003,0x7f555fff555fffff,0x5555fffffffd555,0xfc3fff7d555555ff,0xfff5555555557fdf,0xfcc0ff00ffff,0xfc00ff000000fff0,0xff0000000007,0xfbc7f3f7ffc70006,0xaffffdffffffffff,0xffff0267fbffcfff,0xfffefff8ffffffe5,0xffffffff8000,0xffffffffffffff86,0x39ca1fffffc0ffc0,0x830ffffffffffff,0xffffffffffe00000,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffc90a,0x27ffe00ff0600101,0x8000009018221981,0x2021ffffec0fc,0xf00fc0800000,0x22000000,0xffe00000fffff000,0,0,0xffffffffffffffff,0xffffffffffffffff,0xfffffc00fffffdff,0x7fffffffffff0006,0x600000000000001,0xe900000000670000,0xa000883d80,0,0,0x50180fffc00,0,0x628000183ff0000,0,0,0,0,0xc03000000000c0,0x4d4cccc0000000,0xffffe1e000000000,0xf0001007fc,0x140000002,0,0,0x22003ea0b,0x4000600700000,0x760005c3c5014,0xc0000000,0x50000,0x404508000088404,0xf2ccd21afea84e7f,0x7f7d7f1d77c73dd4,0x775ff775803d037,0xfac867e779048012,0x2065710828018000,0,0xaaffffffff,0xfffc23acfcc3ffce,0xe800000000000000,0,0,0,0,0,0,0xf7fffc0500000000,0x7fffffffffffffff,0xfffff01e7fffffff,0xfffffffffffffe1e,0,0,0,0,0x6040,0,0xf800000,0,0x1004088033100210,0x6441000000001e,0x3000e40000000,0x804000000000000,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0xd1f6cfc242a462d9,0x341707c720005002,0xa6fa6cdbee9e9220,0xeb370fc75c06a318,0xd067c02c00018aec,0x1307d77e00419619,0x12b56ab4c0170601,0x3866c9b55c0316a3,0x6556a637efc0240,0xb3c186280408ae84,0x840448e0000c3d02,0x246546a215060c50,0xe8402ea140202139,0xdffea8282cfe4788,0xa66df744aef8ff06,0xe5c021cc36cc6e6,0x9bb24d040d6293f0,0x193d0a121c61fc05,0x11c46819927d4c4a,0xd54425d7bb0034e3,0xe7858764d82150f1,0xd7e479aa8243fd91,0x280012a186c6b210,0x30fe0b557cfff3a0,0x58ffc015825e02de,0xa2e2400100a0d71c,0x8a3ba0082cd9308e,0xe9c0008066c62025,0xb018a00000350c2,0x8e09130c1000001a,0x9b7000ef01fa8200,0xd00082918000146,0x2400b9000416a80,0x200a801008b808a0,0x83210800a4020a08,0xf08251b29002b065,0x98d5328903848190,0xa123c0404e600001,0x20d01000b8289000,0x93923a0990000968,0xa63004444821cc24,0xc03069e22050a442,0x1400110b009230f2,0x452855cb0c1be17c,0x207186f8257e6182,0x9c3ddb4b5ea1884,0x782412541c882072,0x250d86806b00211,0x504040a984410608,0xc240002022280001,0x20300000000e,0x6058830052400030,0x5802002028a202,0x9400cfbd1e50d4a0,0x95dd05fd3e029ec1,0xbf48b8976fda96a3,0x6ff75222a88440ff,0xc6b7d402cc0b4240,0x8c80042d853ba,0x5000040c0e58634,0x1124ff121400000,0x201ab008004d1200,0x29188004200a01ec,0x4cb21657927d16,0x3218427404159384,0x9d0a0d0178003eff,0x5921072883140780,0x70e19235f0c8dbbb,0x587ddf9147454d8a,0x44aa534c637cd149,0xfdb8f1c2c0e6194c,0x140c03ce8810c495,0x3a65cc7f14020,0x7234a465bcfd8500,0x5c7c2b3193603e8c,0x70054abd180fb70,0xd2c74431c188890f,0x29be14e53593df02,0x2122cae0095a2222,0x13fc800012002baf,0x80eefa374223b891,0xc1d614929082424b,0xb8607f9a0218445c,0x27edf9dcc00f5703,0x61c8404101910027,0x4d850801d22330,0x90cb2291f5f82c12,0x9a41a23050420e20,0x2051308028c0a22,0x20020080814102a0,0x108034260c56a058,0x650628a0f3096a26,0x8400008c18390fd3,0x48747c63208ad80,0x31cac1594d1d814c,0x5e192150e7cc4030,0xb36b1c0b8d874e19,0x785121472fc39a20,0x59c243c4a21228cb,0x4940026c8240bdff,0x90d70f1703be4ba5,0x4243d24bfe25f50b,0x558852ce2411bd0,0xca23d8a6294b5c13,0x78001302607028cc,0xfa86f84d11000b03,0x54051915a385d668,0xaf52da1d1032060,0xcc9f207876a08a45,0x201c6ee0b71dce,0x60224c9881026813,0x513e90c621942055,0x24383953707286e4,0xb9000c162103083,0x88068108b02a1090,0x7000503328a00030,0xc202205d10021e6,0x114a98517a6118d1,0x9a74068208317d39,0x86163a9100191000,0x81591018000983,0xe002852fa0a1b026,0x728cb270601250ff,0x4c68157474a1d000,0x818215304703696,0x4c0000681a6b09c0,0x58a165800092488,0x871964c24aba75f8,0x5f01475df5f00ea,0x1026029901132203,0xa009828d24212325,0x2c585c0600092983,0x62415c00b0186083,0x440008990a0788a8,0xea82410058205440,0xf2000c91c81b5622,0x455144b70388a002,0x840801011808c,0x4622040001f10f0,0x91004500004044,0x841086842000208,0x34a06482f4a0004,0x8a10c4100884042,0x50c2ad01870e045b,0x204801000aca63f,0x839820d561402846,0xe1002200a1096242,0x3a02a150e04c01cc,0x20031d0c730b03,0xc03010010a0a5b0,0x950020000403014,0xa0e6220294264482,0x1740000026623e21,0x4000047bb8619202,0x1050000321008004,0x210808b342e380,0x5c0e54ab1699f1b8,0xa7d487caf759b81,0x23734824141074ce,0x35242cd23040b82b,0x40540001e9008810,0x2388228861e8a2be,0xbb92927a020289e1,0x32a4231b5d222892,0x49d800138e4001a8,0x8300003056900043,0x124002840c925d,0x104013835471008,0x20082c00c7002821,0x402000408192828,0x430e55201161042a,0x4630c82001890804,0x410260400238802a,0xc1c09727a4840121,0xcc27081403229067,0xc05064815b488010,0x809609e98002611f,0xb247810070508a65,0x5880378100638221,0x1db5746df73582e1,0x2ab140d286881640,0x6ddea04050acc20,0x3152def40244880,0xcb5900048e441300,0x8104790151300187,0x900d8a818c081402,0x7054a5916d967046,0xa422228ba1012ab2,0xae348df8e01bb461,0x3e9b821a72827644,0x411079b01fedb7,0x13166aec8c92810,0x4c70201372126576,0x52342e660364805d,0x30ebba1800000000,0,0x2a0,0x50ecc8181f2a5c0,0x37aa082658c322c4,0x46282c00c2509058,0xde18a5c840801215,0x22022a36081bb47,0xf5812b4646d6820,0x1a0a02764c01488c,0x27e0003010415042,0x212dc010612c8e1,0xb0a1142c98c094a7,0xa2c450e195a4183a,0x65eea39b007a17c0,0x810000e52ab36382,0x142045061d50d4,0x400795b57105870,0x7e42038810916ec0,0x8461a08020001519,0x5621223a0b04404,0x452a12898051eb14,0x191e1000a068448c,0x2c20110725f4560,0x28108849400428d9,0x4a74c268000a0809,0x82005da1420c0404,0xd0f215e010f40102,0x89a0c9580afb8060,0x4045840c0c600172,0x2330132020058001,0x68c2b01104050,0x38140018718200,0xb560853084f00d2,0xb2e460a804400911,0x5a154192a20a81,0x2004000120111034,0x8b10a00080012352,0x507460071004250,0xaa0c31567090a507,0x609423422812cd01,0x7c010ccd40803cce,0x2928b00e04300290,0x580c02038a252903,0x53b113a043693025,0x8000202c13000880,0xb0aab39514245b38,0x4280ec12b25ef048,0x2d4c54a2df8ca04b,0x291d223beb1653a2,0xe90a8b74c2981042,0x404b12b90219e905,0x902ab26000000000,0,0x121,0x42aa8420603800e8,0x246e0886e1ffbb9d,0xf9a6503aba24883c,0xdb5ba0000000000,0xb14db00,0x4420004010801022,0x2019023550b11409,0xe1800700208c00,0x29e8844198002a08,0x4d3458404039c002,0x6bd20113010009e,0x14683c5d026110d3,0x2e4e010978000000,0x1b1187e139,0x2c024820267589e4,0xd617df67f10266ca,0x6577fecad5c727ad,0xf961400012a14480,0x4022001268840504,0x200024400104a000,0x7e2a8034683580,0x2154a10828310ca0,0xc3dfc3bf5f060609,0x226cc0200480969,0x8cd2c1722b004182,0x948056b801040140,0x1b540896430f9,0x220180e6f69b2430,0x8940b18800a840a,0x80b20090e8040,0x1280c3864c800080,0x110098e0401046a0,0x4c807032a020850f,0x4000000000000000,0,0,0x100,0xb1717ac0bd84205a,0x112c0e8864000000,0x840a32011,0xa81d801f3e28b7a4,0x6b70ddc91a1ebd8,0xcf5e465830b0a350,0x8ba7524a0920b0d6,0x3ac5664aead44868,0x4e15808892941800,0x1290100800063611,0x7689f1a0480c099c,0x21f0d920511d800,0xa14200,0x1a0000192057280e,0x1468b886c98a0006,0x2458e50000000000,0x3c2048e1808,0x4e80e49520066091,0x4403304000908102,0xb40605312c700000,0x848088,0x880158cb32374910,0x20000a135e36018,0x7c0748a000012c00,0x1003730180b64850,0x82616a107880e400,0x8aecbc104c07a072,0x380212081000280,0,0x5589a00,0x13c2241112b80013,0x4a80c04ec819a020,0x1410020085128b0,0x6a011040020520e4,0x7200000000,0x4c5,0x40987505066400fb,0x80fe8de84810b5c,0x1821400c0080012,0x332801f3282300,0x4040c80000000,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0xedce785ce2fd,0x7c000001210180,0x140018000210328,0x5a080008000700,0x75812000800000,0x6100000000000,0xff53005000511fff,0x705dc41f00000000,0,0xfffffffffd3fc001,0xffd0c13d1fffa6bf,0xddfb1cd577d6fc00,0xda,0xc3c0,0x20008040907300,0,0xffffc030,0,0,0,0x3bd880206000000,0,0x8008,0,0,0x1200009237df,0x7d71fff0800f7fb,0xce9ffa1bb7ef3316,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffffffffffffff,0xffffff0000000000,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0xf000,0x400000,0,0,0x800000000,0x3fffffffc0000,0,0,0,0x600000072d407678,0x829cccccccccffee,0xfffccccc3fc57fff,0xc00007fff3f00003,0,0x3f0000000,0,0x3800,0xf,0,0,0,0xfe000000,0xfbff,0xff9e799fffff8000,0x7f9999f9ee1ff9,0xffffffffffffffff,0xfffffffe7fffffff,0xffffffffffffffff,0xfffffffffdffffff
            };

            private static bool GetZenHanMap(int i)
            {
                if (0 <= i && i <= 0xFFFF)
                {
                    int shift = 64 - i % 64 - 1;
                    int ix = i / 64;

                    ulong temp = encode_zen_han_map_compress[ix];
                    temp = temp >> shift;
                    temp = temp & 0x1;
                    return temp == 1 ? true : false;
                }

                return false;
            }

            private static byte nZenkaku(char wch)
            {
                int ix = wch;
                if (GetZenHanMap(ix))
                {
                    return 0;
                }
                return 8;
            }


            private static List<byte> ToOriginalHmStarUnicode(char wch)
            {
                List<byte> ret = new List<byte>();
                ret.Add(0x1A);
                byte byte2ix = (byte)((byte)0x80 | (byte)(0xFF & wch));
                ret.Add(byte2ix);
                byte byte3ix = (byte)((byte)0x80 | (wch >> 8) & 0xFF);
                ret.Add(byte3ix);
                byte byte4ix = (byte)((byte)((byte)(wch & 0x80) >> 7) + (byte)((wch & 0x8000) >> 14) + (byte)4 + (byte)nZenkaku(wch));
                ret.Add(byte4ix);
                return ret;
            }

            // wchar_tに直接対応していないような古い秀丸では、この特殊な変換マップによる変換をしてバイトコードとして渡す必要がある。
            public static byte[] EncodeWStringToOriginalEncodeVector(string original_string)
            {
                List<byte> r = new List<byte>();
                foreach (char ch in original_string)
                {
                    List<byte> byte4 = ToOriginalHmStarUnicode(ch);
                    foreach (byte b in byte4)
                    {
                        r.Add(b);
                    }
                }

                r.Add(0);
                return r.ToArray();
            }
#else

            // wchar_tに直接対応していないような古い秀丸では、この特殊な変換マップによる変換をしてバイトコードとして渡す必要がある。
            public static byte[] EncodeWStringToOriginalEncodeVector(string original_string)
            {
                List<Byte> r = new List<byte>();

                foreach (char ch in original_string)
                {
                    // 文字コードがそんまま、マップでのIndexになっている。
                    int ix = (int)ch;

                    // 文字コードに対応する秀丸での4バイトの値(UInt32)を得る4バイト
                    UInt32 encode_code = HmOriginalEncodeMap.encode_map[ix];

                    // 単バイトで収まるものは単バイト
                    if (encode_code <= 0xFF)
                    { // １文字で収まる
                        r.Add((Byte)encode_code);
                    }
                    // そうでないもののは４バイト、それぞれ分割で追加
                    else
                    {
                        byte[] bytes4 = BitConverter.GetBytes(encode_code);
                        foreach (byte b in bytes4)
                        {
                            if (b == 0) { break; }
                            r.Add(b);
                        }
                    }
                }

                r.Add(0);
                return r.ToArray();
            }
#endif
        }
    }
}
