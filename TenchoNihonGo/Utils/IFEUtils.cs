using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TenchoNihonGo.Utils
{
    class CFELanguage
    {
        // request for conversion (dwRequest)
        public const int FELANG_REQ_CONV = 0x00010000;
        public const int FELANG_REQ_RECONV = 0x00020000;
        public const int FELANG_REQ_REV = 0x00030000;

        // Conversion mode (dwCMode)
        public const int FELANG_CMODE_MONORUBY = 0x00000002; //mono-ruby
        public const int FELANG_CMODE_NOPRUNING = 0x00000004; //no pruning
        public const int FELANG_CMODE_KATAKANAOUT = 0x00000008; //katakana output
        public const int FELANG_CMODE_HIRAGANAOUT = 0x00000000; //default output
        public const int FELANG_CMODE_HALFWIDTHOUT = 0x00000010; //half-width output
        public const int FELANG_CMODE_FULLWIDTHOUT = 0x00000020; //full-width output
        public const int FELANG_CMODE_BOPOMOFO = 0x00000040; //
        public const int FELANG_CMODE_HANGUL = 0x00000080; //
        public const int FELANG_CMODE_PINYIN = 0x00000100; //
        public const int FELANG_CMODE_PRECONV = 0x00000200; //do conversion as follows:
        // - roma-ji to kana
        // - autocorrect before conversion
        // - periods, comma, and brackets
        public const int FELANG_CMODE_RADICAL = 0x00000400; //
        public const int FELANG_CMODE_UNKNOWNREADING = 0x00000800; //
        public const int FELANG_CMODE_MERGECAND = 0x00001000; // merge display with same candidate
        public const int FELANG_CMODE_ROMAN = 0x00002000; //
        public const int FELANG_CMODE_BESTFIRST = 0x00004000; // only make 1st best
        public const int FELANG_CMODE_USENOREVWORDS = 0x00008000; // use invalid revword on REV/RECONV.

        public const int FELANG_CMODE_NONE = 0x01000000; // IME_SMODE_NONE
        public const int FELANG_CMODE_PLAURALCLAUSE = 0x02000000; // IME_SMODE_PLAURALCLAUSE
        public const int FELANG_CMODE_SINGLECONVERT = 0x04000000; // IME_SMODE_SINGLECONVERT
        public const int FELANG_CMODE_AUTOMATIC = 0x08000000; // IME_SMODE_AUTOMATIC
        public const int FELANG_CMODE_PHRASEPREDICT = 0x10000000; // IME_SMODE_PHRASEPREDICT
        public const int FELANG_CMODE_CONVERSATION = 0x20000000; // IME_SMODE_CONVERSATION
        public const int FELANG_CMODE_NAME = FELANG_CMODE_PHRASEPREDICT; // Name mode (MSKKIME)
        public const int FELANG_CMODE_NOINVISIBLECHAR = 0x40000000; // remove invisible chars (e.g. tone mark)

        private IFELanguage thisIFE;

        public CFELanguage()
        {
            thisIFE = (IFELanguage)Activator.CreateInstance(Type.GetTypeFromProgID("MSIME.Japan"));
        }
        public CFELanguage(string progID)
        {
            thisIFE = (IFELanguage)Activator.CreateInstance(Type.GetTypeFromProgID(progID));
        }
        ~CFELanguage()
        {
            thisIFE = null;
        }
        public int Open()
        {
            return thisIFE.Open();
        }
        public int Close()
        {
            return thisIFE.Close();
        }
        public string GetJMorphResult(uint dwRequest, uint dwCMode, int cwchInput, string pwchInput, IntPtr pfCInfo)
        {
            string strOutput = "";
            string[] astrOutput = pwchInput.Split(new String[] { "\r\n" }, StringSplitOptions.None);

            foreach (string strEach in astrOutput)
            {
                try
                {
                    if (strEach.Length != 0)
                    {
                        IntPtr ppResult;
                        thisIFE.GetJMorphResult(dwRequest, dwCMode, strEach.Length, strEach, pfCInfo, out ppResult);
                        string strTmp = Marshal.PtrToStringUni(Marshal.ReadIntPtr(ppResult, 4), Marshal.ReadInt16(ppResult, 8));
                        strOutput += strTmp;
                    }
                    strOutput += "\r\n";
                }
                catch
                {
                    string[] astrOutput2 = strEach.Split(new String[] { "、" }, StringSplitOptions.None);
                    foreach (string strEach2 in astrOutput2)
                    {
                        try
                        {
                            IntPtr ppResult;
                            thisIFE.GetJMorphResult(dwRequest, dwCMode, strEach2.Length, strEach2, pfCInfo, out ppResult);
                            string strTmp = Marshal.PtrToStringUni(Marshal.ReadIntPtr(ppResult, 4), Marshal.ReadInt16(ppResult, 8));
                            strOutput += strTmp;
                            strOutput += "、";
                        }
                        catch
                        {
                            string[] astrOutput3 = strEach2.Split(new String[] { "　" }, StringSplitOptions.None);
                            foreach (string strEach3 in astrOutput3)
                            {
                                try
                                {
                                    IntPtr ppResult;
                                    thisIFE.GetJMorphResult(dwRequest, dwCMode, strEach3.Length, strEach3, pfCInfo, out ppResult);
                                    string strTmp = Marshal.PtrToStringUni(Marshal.ReadIntPtr(ppResult, 4), Marshal.ReadInt16(ppResult, 8));
                                    strOutput += strTmp;
                                    strOutput += "　";
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                }
            }

            return strOutput;
        }
        public int GetConversionModeCaps(IntPtr pdwCaps)
        {
            return thisIFE.GetConversionModeCaps(pdwCaps);
        }
        public string GetPhonetic(string bsInput, int nStart, int nLength)
        {
            string pbsPhonetic;
            thisIFE.GetPhonetic(bsInput, nStart, nLength, out pbsPhonetic);
            return pbsPhonetic;
        }
        public string GetConversion(string bsInput, int nStart, int nLength)
        {
            string pbsConversion;
            thisIFE.GetConversion(bsInput, nStart, nLength, out pbsConversion);
            return pbsConversion;
        }

        //**************************************************************************************
        // IFELanguage Interface
        //**************************************************************************************
        [ComImport]
        [Guid("019F7152-E6DB-11D0-83C3-00C04FDDB82E")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IFELanguage
        {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int Open();
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int Close();
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int GetJMorphResult([In] uint dwRequest, [In] uint dwCMode, [In] int cwchInput, [In, MarshalAs(UnmanagedType.LPWStr)] string pwchInput, [In] IntPtr pfCInfo, [Out] out IntPtr ppResult);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int GetConversionModeCaps([In] IntPtr pdwCaps);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int GetPhonetic([In, MarshalAs(UnmanagedType.BStr)] string bsInput, [In] int nStart, [In] int nLength, [Out, MarshalAs(UnmanagedType.BStr)] out string pbsPhonetic);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            int GetConversion([In, MarshalAs(UnmanagedType.BStr)] string bsInput, [In] int nStart, [In] int nLength, [Out, MarshalAs(UnmanagedType.BStr)] out string pbsConversion);
        }
    }
    class CLSRoman
    {
        
        public static string GetRomaji(string strText)
        {
            string strRoman = "sha|shu|sho|cha|chu|cho|wi|we|wo|a|i|u|e|o|ka|ki|ku|ke|ko|sa|shi|su|se|so|ta|chi|tsu|te|to|na|ni|nu|ne|no|ha|hi|fu|he|ho|ma|mi|mu|me|mo|ya|yu|yo|ra|ri|ru|re|ro|wa|wi|we|wo|nn|ga|gi|gu|ge|go|za|ji|zu|ze|zo|da|ji|zu|de|do|ba|bi|bu|be|bo|pa|pi|pu|pe|po|vu";
            string strKatakana = "シャ|シュ|ショ|チャ|チュ|チョ|ウィ|ウェ|ウォ|ア|イ|ウ|エ|オ|カ|キ|ク|ケ|コ|サ|シ|ス|セ|ソ|タ|チ|ツ|テ|ト|ナ|ニ|ヌ|ネ|ノ|ハ|ヒ|フ|ヘ|ホ|マ|ミ|ム|メ|モ|ヤ|ユ|ヨ|ラ|リ|ル|レ|ロ|ワ|ヰ|ヱ|ヲ|ン|ガ|ギ|グ|ゲ|ゴ|ザ|ジ|ズ|ゼ|ゾ|ダ|ヂ|ヅ|デ|ド|バ|ビ|ブ|ベ|ボ|パ|ピ|プ|ペ|ポ|ヴ";

            string strRoman_youon = "a|i|u|e|o|ya|yu|yo|wa";
            string strKatakana_youon = "ァ|ィ|ゥ|ェ|ォ|ャ|ュ|ョ|ヮ";

            string[] astrRoman = strRoman.Split('|');
            string[] astrKatakana = strKatakana.Split('|');

            string[] astrRoman_youon = strRoman_youon.Split('|');
            string[] astrKatakana_youon = strKatakana_youon.Split('|');

            int nCount = 0;
            foreach (string strThis in astrKatakana)
            {
                strText = strText.Replace(strThis, astrRoman[nCount]);
                nCount++;
            }
            nCount = 0;
            foreach (string strThis in astrKatakana_youon)
            {
                int nIndexof = strText.IndexOf(strThis);
                while (nIndexof != -1)
                {
                    strText = strText.Remove(nIndexof - 1, 1 + strThis.Length);
                    strText = strText.Insert(nIndexof - 1, astrRoman_youon[nCount]);
                    nIndexof = strText.IndexOf(strThis);
                }
                nCount++;
            }

            {
                string strThis = "ー";
                int nIndexof = strText.IndexOf(strThis);
                while (nIndexof != -1)
                {
                    string strChouOn = strText.Substring(nIndexof - 1, 1);
                    strText = strText.Remove(nIndexof, strThis.Length);
                    strText = strText.Insert(nIndexof, strChouOn);
                    nIndexof = strText.IndexOf(strThis);
                }
            }

            {
                string strThis = "ッ";
                int nIndexof = strText.IndexOf(strThis);
                while (nIndexof != -1)
                {
                    string strChouOn = strText.Substring(nIndexof + strThis.Length, 1);
                    strText = strText.Remove(nIndexof, strThis.Length);
                    strText = strText.Insert(nIndexof, strChouOn);
                    nIndexof = strText.IndexOf(strThis);
                }
            }

            return strText;
        }

        public static string GetXieyin(string strText)
        {
            string strKanji = "下|修|小|洽|秋|桥|牛|尿|谬|喵|俩|流|料|加|就|叫|飘|哇|喂|唔|外|我|擦|踢|发|非|喂|滴|吐|肚|外|些|切|猜|借|我|操|佛|啊|一|屋|欸|噢|卡|KI|哭|开|扣|撒|西|死|塞|嫂|塔|其|此|帖|逃|那|你|努|耐|闹|哈|嘿|服|嗨|好|吗|米|亩|买|猫|呀|由|哟|啦|理|路|来|咯|瓦|为|外|哦|嗯|嘎|给|姑|改|高|杂|技|自|在|遭|打|击|资|蝶|刀|吧|哔|不|败|保|啪|屁|噗|拍|抛";
            string strKatakana = "シャ|シュ|ショ|チャ|チュ|チョ|ニュ|ニョ|ミュ|ミョ|リャ|リュ|リョ|ジャ|ジュ|ジョ|ピョ|ヴァ|ヴィ|ヴ|ヴェ|ヴォ|ツァ|ティ|ファ|フィ|ウィ|ディ|トゥ|ドゥ|ウェ|シェ|チェ|ツェ|ジェ|ウォ|ツォ|フォ|ア|イ|ウ|エ|オ|カ|キ|ク|ケ|コ|サ|シ|ス|セ|ソ|タ|チ|ツ|テ|ト|ナ|ニ|ヌ|ネ|ノ|ハ|ヒ|フ|ヘ|ホ|マ|ミ|ム|メ|モ|ヤ|ユ|ヨ|ラ|リ|ル|レ|ロ|ワ|ヰ|ヱ|ヲ|ン|ガ|ギ|グ|ゲ|ゴ|ザ|ジ|ズ|ゼ|ゾ|ダ|ヂ|ヅ|デ|ド|バ|ビ|ブ|ベ|ボ|パ|ピ|プ|ペ|ポ";

            string strKanji_youon = "啊|一|屋|欸|噢|呀|由|哟|哇|·|→";
            string strKatakana_youon = "ァ|ィ|ゥ|ェ|ォ|ャ|ュ|ョ|ヮ|ッ|ー";

            string[] astrKanji = strKanji.Split('|');
            string[] astrKatakana = strKatakana.Split('|');

            string[] astrKanji_youon = strKanji_youon.Split('|');
            string[] astrKatakana_youon = strKatakana_youon.Split('|');

            int nCount = 0;
            foreach (string strThis in astrKatakana)
            {
                strText = strText.Replace(strThis, astrKanji[nCount]);
                nCount++;
            }
            nCount = 0;
            foreach (string strThis in astrKatakana_youon)
            {
                strText = strText.Replace(strThis, astrKanji_youon[nCount]);
                nCount++;
            }
            return strText;
        }

        public static string GetKatakana(string strText)
        {
            string strHiragana = "あ|い|う|え|お|か|き|く|け|こ|さ|し|す|せ|そ|た|ち|つ|て|と|な|に|ぬ|ね|の|は|ひ|ふ|へ|ほ|ま|み|む|め|も|や|ゆ|よ|ら|り|る|れ|ろ|わ|ゐ|ゑ|を|ん|が|ぎ|ぐ|げ|ご|ざ|じ|ず|ぜ|ぞ|だ|ぢ|づ|で|ど|ば|び|ぶ|べ|ぼ|ぱ|ぴ|ぷ|ぺ|ぽ|ぁ|ぃ|ぅ|ぇ|ぉ|ゃ|ゅ|ょ|ゎ|っ";
            string strKatakana = "ア|イ|ウ|エ|オ|カ|キ|ク|ケ|コ|サ|シ|ス|セ|ソ|タ|チ|ツ|テ|ト|ナ|ニ|ヌ|ネ|ノ|ハ|ヒ|フ|ヘ|ホ|マ|ミ|ム|メ|モ|ヤ|ユ|ヨ|ラ|リ|ル|レ|ロ|ワ|ヰ|ヱ|ヲ|ン|ガ|ギ|グ|ゲ|ゴ|ザ|ジ|ズ|ゼ|ゾ|ダ|ヂ|ヅ|デ|ド|バ|ビ|ブ|ベ|ボ|パ|ピ|プ|ペ|ポ|ァ|ィ|ゥ|ェ|ォ|ャ|ュ|ョ|ヮ|ッ";

            string[] astrHiragana = strHiragana.Split('|');
            string[] astrKatakana = strKatakana.Split('|');

            int nCount = 0;
            foreach (string strThis in astrKatakana)
            {
                strText = strText.Replace(strThis, astrHiragana[nCount]);
                nCount++;
            }
            return strText;
        }
    }

}
