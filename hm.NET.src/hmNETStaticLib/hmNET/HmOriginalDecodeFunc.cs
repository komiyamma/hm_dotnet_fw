using System;
using System.Collections.Generic;

internal sealed partial class hmNETDynamicLib
{
    internal sealed partial class Hidemaru
    {
        internal class HmOriginalDecodeFunc
        {
            static bool IsSTARTUNI_inline(uint byte4)
            {
                return (byte4 & 0xF4808000) == 0x04808000;
            }

            static long MakeWord(long low, long high)
            {
                return ((long)high << 8) | low;
            }
            static char GetUnicodeInText(byte[] pchSrc)
            {
                long value = MakeWord(
                    (pchSrc[1] & 0x7F | ((pchSrc[3] & 0x01) << 7)),
                    (pchSrc[2] & 0x7F | ((pchSrc[3] & 0x02) << 6))
                );

                byte[] byteArray = BitConverter.GetBytes(value);

                byte[] charByte = { byteArray[0], byteArray[1] };

                char wch = BitConverter.ToChar(charByte, 0);

                return wch;
            }

            public static string DecodeOriginalEncodeVector(List<byte> OriginalEncodeData)
            {
                try
                {
                    string result = "";

                    byte[] byteArray = OriginalEncodeData.ToArray();

                    // 一時バッファー用
                    List<byte> tmp_buffer = new List<byte>();
                    int len = OriginalEncodeData.Count;

                    int lastcheckindex = len - 4; // IsSTARTUNI_inline には 4バイト必要
                    if (lastcheckindex < 0)
                    {
                        lastcheckindex = 0;
                    }
                    for (int i = 0; i < len; i++)
                    {
                        // 一般の文字としてはほぼ利用されないスターマーク。
                        if (i <= lastcheckindex && byteArray[i] == '\x1A')
                        {
                            uint StarUni = BitConverter.ToUInt32(byteArray, i);

                            if (IsSTARTUNI_inline(StarUni))
                            {
                                // 今までの分はスターユニコードではないので、通常のSJISとみなし、utf16に変換して足し込み
                                if (tmp_buffer.Count > 0)
                                {
                                    result += System.Text.Encoding.GetEncoding(932).GetString(tmp_buffer.ToArray());
                                    tmp_buffer.Clear();
                                }

                                byte[] starByteArray = BitConverter.GetBytes(StarUni);
                                char wch = GetUnicodeInText(starByteArray);
                                i = i + 3; // 1バイトではなく4バイト消化したので、計算する
                                result += wch;
                                continue;
                            }
                        }
                        tmp_buffer.Add(byteArray[i]);
                    }

                    if (tmp_buffer.Count > 0)
                    {
                        result += System.Text.Encoding.GetEncoding(932).GetString(tmp_buffer.ToArray());
                        tmp_buffer.Clear();
                    }

                    return result;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e);
                }

                return "";
            }
        }
    }
}
