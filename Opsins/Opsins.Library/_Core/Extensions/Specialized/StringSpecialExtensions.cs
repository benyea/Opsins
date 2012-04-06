using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Opsins.Specialized
{
    /// <summary>
    /// String特殊扩展
    /// --汉字拼音
    /// --HTML
    /// </summary>
    public static class StringSpecialExtensions
    {
        #region 中文拼音

        /// <summary>
        /// 拼音值数组
        /// </summary>
        private static readonly int[] PinyinValue = new int[]
        {
            #region PinYin Value
            -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
            -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
            -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
            -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
            -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
            -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
            -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
            -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
            -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
            -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
            -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
            -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
            -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
            -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
            -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
            -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
            -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
            -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
            -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
            -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
            -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
            -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
            -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
            -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
            -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
            -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
            -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
            -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
            -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
            -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
            -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
            -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
            -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
            #endregion
        };

        /// <summary>
        /// 拼音名数组
        /// </summary>
        private static readonly string[] PinyinName = new string[]
        {
            #region PinYing Name
            "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
            "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
            "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
            "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
            "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
            "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
            "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
            "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
            "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
            "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
            "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
            "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
            "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
            "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
            "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
            "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
            "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
            "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
            "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
            "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
            "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
            "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
            "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
            "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
            "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
            "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
            "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
            "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
            "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
            "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
            "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
            "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
            "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
            #endregion
        };


        /// <summary>
        /// 把汉字转换成拼音(全拼)
        /// </summary>
        /// <param name="target">汉字字符串</param>
        /// <returns>转换后的拼音(全拼)字符串</returns>
        public static string ChineseToPinyin(this string target)
        {
            #region 汉字转换成拼音
            // 匹配中文字符
            Regex regex = new Regex("^[\u4e00-\u9fa5]$");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = target.ToCharArray();

            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString += noWChar[j];
                    }
                    else
                    {
                        // 修正部分文字
                        if (chrAsc == -9254)  // 修正“圳”字
                            pyString += "Zhen";
                        else
                        {
                            for (int i = (PinyinValue.Length - 1); i >= 0; i--)
                            {
                                if (PinyinValue[i] <= chrAsc)
                                {
                                    pyString += PinyinName[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                // 非中文字符
                else
                {
                    pyString += noWChar[j].ToString();
                }
            }
            return pyString;
            #endregion
        }

        /// <summary>
        /// 中文字符串转为拼间首字母串
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string GetChineseSpell(this string target)
        {
            int len = target.Length;
            StringBuilder myStr = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                myStr.Append(GetAlephSpell(target.Substring(i, 1)));
            }
            return myStr.ToString();
        }


        /// <summary>
        /// 中文字符转为拼间首字母
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        private static string GetAlephSpell(this string target)
        {
            byte[] arrCn = Encoding.Default.GetBytes(target);
            if (arrCn.Length > 1)
            {
                int area = (short)arrCn[0];
                int pos = (short)arrCn[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            return target;
        }

        #endregion

        #region 汉字与16进制转换

        /// <summary>
        /// 由16进制转为字符串（如：B2E2 -> 测 ）
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string HexToString(this string target)
        {
            byte[] oribyte = new byte[target.Length / 2];
            for (int i = 0; i < target.Length; i += 2)
            {
                string str = Convert.ToInt32(target.Substring(i, 2), 16).ToString();
                oribyte[i / 2] = Convert.ToByte(target.Substring(i, 2), 16);
            }
            return System.Text.Encoding.Default.GetString(oribyte);
        }

        /// <summary>
        /// 字符串转为16进制字符串（如：测 -> B2E2 ）
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string StringToHex(this string target)
        {
            int i = target.Length;
            string temp;
            string end = "";
            byte[] array = new byte[2];
            int i1, i2;
            for (int j = 0; j < i; j++)
            {
                temp = target.Substring(j, 1);
                array = System.Text.Encoding.Default.GetBytes(temp);
                if (array.Length.ToString() == "1")
                {
                    i1 = Convert.ToInt32(array[0]);
                    end += Convert.ToString(i1, 16);
                }
                else
                {
                    i1 = Convert.ToInt32(array[0]);
                    i2 = Convert.ToInt32(array[1]);
                    end += Convert.ToString(i1, 16);
                    end += Convert.ToString(i2, 16);
                }
            }
            return end.ToUpper();
        }

        /// <summary>
        /// 汉字转换到16进制
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string ChsToHex(this string target)
        {
            if ((target.Length % 2) != 0)
            {
                target += " ";//空格
                //throw new ArgumentException("s is not valid chinese string!");
            }

            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");
            byte[] bytes = chs.GetBytes(target);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
            }
            target = str;
            return target;
        }

        /// <summary>
        /// 16进制转换成汉字
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string HexToChs(this string target)
        {
            if (target == null)
                throw new ArgumentNullException("hex");
            if (target.Length % 2 != 0)
            {
                target += "20";//空格
                //throw new ArgumentException("hex is not a valid number!", "hex");
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[target.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(target.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            // 获得 GB2312，Chinese Simplified。 
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");
            target = chs.GetString(bytes);
            return target;
        }

        #endregion

        #region UTF与汉字编码

        /// <summary>
        /// unicode转换为汉字
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string UnicodeToChs(this string target)
        {
            return target.ConvertToByEncode("unicode");
        }

        /// <summary>
        /// 转换指定的编码字符串
        /// </summary>
        /// <param name="target">字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string ConvertToByEncode(this string target, string encode)
        {
            var tmpStr = new StringBuilder();
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] == '\\' && target[i + 1] == 'u')
                {
                    string s1 = target.Substring(i + 2, 2);
                    string s2 = target.Substring(i + 4, 2);
                    int t1 = Convert.ToInt32(s1, 16);
                    int t2 = Convert.ToInt32(s2, 16);
                    var array = new byte[2];
                    array[0] = (byte)t2;
                    array[1] = (byte)t1;
                    string s = Encoding.GetEncoding(encode).GetString(array);
                    tmpStr.Append(s);
                    i = i + 5;
                }
                else { tmpStr.Append(target[i]); }
            }
            return tmpStr.ToString();
        }

        #endregion

        #region GB2312转换为UTF-8

        /// <summary>
        /// 单字GB2312转UTF8 URL编码
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string Gb2312ToUtf8(this string target)
        {
            if (string.IsNullOrEmpty(target)) return string.Empty;
            string[] myWord = target.Split('%');
            byte[] myByte = new byte[] { Convert.ToByte(myWord[1], 16), Convert.ToByte(myWord[2], 16) };
            Encoding GB = Encoding.GetEncoding("GB2312");
            Encoding U8 = Encoding.UTF8;
            myByte = Encoding.Convert(GB, U8, myByte);
            char[] Chars = new char[U8.GetCharCount(myByte, 0, myByte.Length)];
            U8.GetChars(myByte, 0, myByte.Length, Chars, 0);
            return new string(Chars);
        }

        #endregion

        #region 全角 半角 互转换

        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="target">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>        
        public static string ToSdc(this string target)
        {
            //半角转全角：
            char[] c = target.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 转半角的函数(DBC case)
        /// </summary>
        /// <param name="target">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDsc(this string target)
        {
            char[] c = target.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        #endregion

        #region 字体转换

        /// <summary>
        /// 语言
        /// </summary>
        enum Language
        {
            SimplifiedChinese = 0x02000000,
            TraditionalChinese = 0x04000000,
        }

        [DllImport("kernel32.dll", EntryPoint = "LCMapStringA")]
        static extern int LCMapString(int locale, int dwMapFlags, byte[] lpSrcStr, int cchSrc, byte[] lpDestStr, int cchDest);

        /// <summary>
        /// 繁体转简体API
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string BigToGb2312(this string target)
        {

            byte[] src = Encoding.Default.GetBytes(target);
            byte[] dest = new byte[src.Length];
            LCMapString(0x0804, Language.SimplifiedChinese.GetHashCode(), src, -1, dest, src.Length);
            return Encoding.Default.GetString(dest);
        }

        /// <summary>
        /// 简体转繁体API
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string Gb2312ToBig(this string target)
        {

            byte[] src = Encoding.Default.GetBytes(target);
            byte[] dest = new byte[src.Length];
            LCMapString(0x0804, Language.TraditionalChinese.GetHashCode(), src, -1, dest, src.Length);
            return Encoding.Default.GetString(dest);
        }

        /// <summary>
        /// 繁体转简体
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string BigToGb2312Ex(this string target)
        {
            return Microsoft.VisualBasic.Strings.StrConv(target, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);
        }


        /// <summary>
        /// 简体转繁体
        /// </summary>
        /// <param name="target">字符串</param>
        /// <returns></returns>
        public static string Gb2312ToBigEx(this string target)
        {
            return Microsoft.VisualBasic.Strings.StrConv(target, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
        }

        #endregion
    }
}
