﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// Performs encoding of input strings to provide protection against
    /// Cross-Site Scripting (XSS) attacks in various contextes.
    /// AntiXss library from Microsoft 
    /// <remarks>
    /// The Anti-Cross Site Scripting Library utilizes the Principle 
    /// of Inclusions, sometimes referred to as <i>white-listing</i> to 
    /// provide protection against Cross-Site Scripting attacks.  With
    /// white-listing protection algorithms looks for valid inputs and 
    /// automatically treats everything outside that set as a 
    /// potential attack.  This library can be used as a defense in
    /// depth approach with other mitigation techniques and is suitable
    /// for applications with high security requirements.
    /// </remarks>
    /// </summary>
    public class AntiXss
    {
        #region MEMBERS
        ///---------------------------------------------------------------------
        /// <summary>
        ///     Empty string for Visual Basic Script contextes
        /// </summary>
        ///---------------------------------------------------------------------
        private const string EmptyStringVBS = "\"\"";

        ///---------------------------------------------------------------------
        /// <summary>
        ///     Empty string for Java Script contextes
        /// </summary>
        ///---------------------------------------------------------------------
        private const string EmptyStringJavaScript = "''";

        #region WHITELIST_CHAR_ARRAY_INITIALIZATION
        /// <summary>
        /// Initializes character Html encoding array
        /// </summary>
        private static char[][] WhitelistCodes = InitWhitelistCodes();

        private static char[][] InitWhitelistCodes()
        {
            char[][] allCharacters = new char[65536][];
            char[] thisChar;
            for (int i = 0; i < allCharacters.Length; i++)
            {
                if (
                    (i >= 97 && i <= 122) ||        // a-z
                    (i >= 65 && i <= 90) ||         // A-Z
                    (i >= 48 && i <= 57) ||         // 0-9
                    i == 32 ||                      // space
                    i == 40 ||                      // (
                    i == 41 ||                      // )
                    i == 46 ||                      // .
                    i == 44 ||                      // ,
                    i == 61 ||                      // =
                    i == 43 ||                      // +
                    i == 45 ||                      // -
                    i == 58 ||                      // :
                    i == 59 ||                      // ;
                    i == 91 ||                      // [
                    i == 93 ||                      // ]
                    i == 95 ||                      // _
                    i == 63 ||                      // ?
                    i == 123 ||                     // {
                    i == 125 ||                     // }
                    (i >= 256 && i <= 591) ||       // Latin,Extended-A,Latin Extended-B        
                    (i >= 880 && i <= 2047) ||      // Greek and Coptic,Cyrillic,Cyrillic Supplement,Armenian,Hebrew,Arabic,Syriac,Arabic,Supplement,Thaana,NKo
                    (i >= 2304 && i <= 6319) ||     // Devanagari,Bengali,Gurmukhi,Gujarati,Oriya,Tamil,Telugu,Kannada,Malayalam,Sinhala,Thai,Lao,Tibetan,Myanmar,eorgian,Hangul Jamo,Ethiopic,Ethiopic Supplement,Cherokee,Unified Canadian Aboriginal Syllabics,Ogham,Runic,Tagalog,Hanunoo,Buhid,Tagbanwa,Khmer,Mongolian   
                    (i >= 6400 && i <= 6687) ||     // Limbu, Tai Le, New Tai Lue, Khmer, Symbols, Buginese
                    (i >= 6912 && i <= 7039) ||     // Balinese         
                    (i >= 7680 && i <= 8191) ||     // Latin Extended Additional, Greek Extended        
                    (i >= 11264 && i <= 11743) ||   // Glagolitic, Latin Extended-C, Coptic, Georgian Supplement, Tifinagh, Ethiopic Extended    
                    (i >= 12352 && i <= 12591) ||   // Hiragana, Katakana, Bopomofo       
                    (i >= 12688 && i <= 12735) ||   // Kanbun, Bopomofo Extended        
                    (i >= 12784 && i <= 12799) ||   // Katakana, Phonetic Extensions         
                    (i >= 40960 && i <= 42191) ||   // Yi Syllables, Yi Radicals        
                    (i >= 42784 && i <= 43055) ||   // Latin Extended-D, Syloti, Nagri        
                    (i >= 43072 && i <= 43135) ||   // Phags-pa         
                    (i >= 44032 && i <= 55215) ||   // Hangul Syllables 
                    (i >= 19968 && i <= 40899)      // Mixed japanese/chinese/korean
                )
                {
                    allCharacters[i] = null;
                }
                else
                {
                    string iString = i.ToString();
                    int iStringLen = iString.Length;
                    thisChar = new char[iStringLen];     // everything else
                    for (int j = 0; j < iStringLen; j++)
                    {
                        thisChar[j] = iString[j];
                    }
                    allCharacters[i] = thisChar;
                }
            }
            return allCharacters;
        }
        #endregion

        #endregion

        #region Encoding Methods

        #region HTMLEncode - string input
        ///---------------------------------------------------------------------
        /// <summary>
        /// Encodes input strings for use in HTML.
        /// </summary>
        /// <param name="input">String to be encoded</param>
        /// <returns>
        ///     Encoded string for use in HTML.
        /// </returns>
        /// <remarks>
        /// This function will encode all but known safe characters.  Encoded characters are encoded using the &amp;#DECIMAL; notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>,</term><description>Comma</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Space</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description>alert&#40;&#39;XSS Attack&#33;&#39;&#41;&#59;</description></item>
        /// <item><term>user@contoso.com</term><description>user&#64;contoso.com</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross Site Scripting Library</description></item>
        /// </list></remarks>
        ///---------------------------------------------------------------------
        public static string HtmlEncode(string input)
        {
            if (String.IsNullOrEmpty(input))
                return string.Empty;

            // Use a new char array.
            int len = 0;
            int tLen = input.Length;
            char[] returnMe = new char[tLen * 8];
            char[] thisChar;
            int thisCharID;
            for (int i = 0; i < tLen; i++)
            {
                thisCharID = (int)input[i];

                if (WhitelistCodes[thisCharID] != null)
                {
                    // character needs to be encoded
                    thisChar = WhitelistCodes[thisCharID];
                    returnMe[len++] = '&';
                    returnMe[len++] = '#';
                    for (int j = 0; j < thisChar.Length; j++)
                    {
                        returnMe[len++] = thisChar[j];
                    }
                    returnMe[len++] = ';';
                }
                else
                {
                    // character does not need encoding
                    returnMe[len++] = input[i];
                }
            }
            return new String(returnMe, 0, len);
        }
        #endregion

        #region HTMLEncode - string input, KnownColor clr
        ///---------------------------------------------------------------------
        /// <summary>
        /// Encodes input string and embeds in a SPAN tag for use in HTML.
        /// </summary>
        /// <param name="input">String to be encoded</param>
        /// <param name="clr">KnownColor like System.Drawing.KnownColor.CadetBlue</param>
        /// <returns>
        ///     Encoded string embebded within SPAN tag and style settings for use in HTML.
        /// </returns>
        /// <remarks>
        /// This function will encode all but known safe characters.  Encoded characters are encoded using the &amp;#DECIMAL; notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>,</term><description>Comma</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Space</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description><div style='background-color : #ffffff'>alert&#40;&#39;XSS Attack&#33;&#39;&#41;&#59;</div></description></item>
        /// <item><term>user@contoso.com</term><description>user&#64;contoso.com</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross Site Scripting Library</description></item>
        /// </list></remarks>
        ///---------------------------------------------------------------------
        //public static string HtmlEncode(string input)
        //{
        //    //HTMLEncode will handle the encoding
        //    // This check is for making sure that bgcolor is required or not.
        //    //if (HttpContext.Current.Request.QueryString["MarkAntiXssOutput"] != null)
        //    //{
        //    //    string returnInput = "<span name='#markantixssoutput' style ='background-color : " + Color.FromKnownColor(clr).Name + "'>" + HtmlEncode(input) + "</span>";
        //    //    return returnInput;
        //    //}
        //    //else
        //    return HtmlEncode(input);

        //}
        #endregion

        #region HTMLAttributeEncode_Method
        ///---------------------------------------------------------------------
        /// <summary>
        /// Encodes input strings for use in HTML attributes.
        /// </summary>
        /// <param name="input">String to be encoded</param>
        /// <returns>
        ///     Encoded string for use in HTML attributes.
        /// </returns>
        /// <remarks>
        /// This function will encode all but known safe characters.  Encoded characters are encoded using the &amp;#DECIMAL; notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>,</term><description>Comma</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description>alert&#40;&#39;XSS&#32;Attack&#33;&#39;&#41;&#59;</description></item>
        /// <item><term>user@contoso.com</term><description>user&#64;contoso.com</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross&#32;Site&#32;Scripting&#32;Library</description></item>
        /// </list></remarks>
        ///---------------------------------------------------------------------
        public static string HtmlAttributeEncode(string input)
        {
            if (String.IsNullOrEmpty(input))
                return string.Empty;

            // Use a new char array.
            int len = 0;
            int tLen = input.Length;
            char[] returnMe = new char[tLen * 8]; // worst case length scenario
            char[] thisChar;
            int thisCharID;
            for (int i = 0; i < tLen; i++)
            {
                thisCharID = (int)input[i];
                if (WhitelistCodes[thisCharID] != null)
                {
                    // character needs to be encoded
                    thisChar = WhitelistCodes[thisCharID];

                    returnMe[len++] = '&';
                    returnMe[len++] = '#';
                    if (thisCharID == 32)
                    {
                        returnMe[len++] = '3';
                        returnMe[len++] = '2';
                    }
                    else
                    {
                        for (int j = 0; j < thisChar.Length; j++)
                        {
                            returnMe[len++] = thisChar[j];
                        }
                    }
                    returnMe[len++] = ';';
                }
                else
                {
                    // character does not need encoding
                    returnMe[len++] = input[i];
                }
            }
            return new String(returnMe, 0, len);
        }

        #endregion

        #region URLEncode_Method
        ///---------------------------------------------------------------------
        /// <summary>
        /// Encodes input strings for use in universal resource locators (URLs).
        /// </summary>
        /// <param name="input">String to be encoded</param>
        /// <returns>
        ///     Encoded string for use in URLs.
        /// </returns>
        /// <remarks>
        /// This function will encode all but known safe characters.  Encoded characters are encoded using the %SINGLE_BYTE_HEX and %uDOUBLE_BYTE_HEX notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description>alert%28%27XSS%20Attack%21%27%29%3b</description></item>
        /// <item><term>user@contoso.com</term><description>user%40contoso.com</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross%20Site%20Scripting%20Library</description></item>
        /// </list></remarks>
        ///---------------------------------------------------------------------
        public static string UrlEncode(string input)
        {
            if (String.IsNullOrEmpty(input))
                return string.Empty;

            // Use a new char array.
            int len = 0;
            int tLen = input.Length;
            int thisCharID;

            string thisChar;
            char ch;
            Encoding inputEncoding = null;

            // Use a new char array.
            char[] returnMe = new char[tLen * 24];

            for (int i = 0; i < tLen; i++)
            {
                thisCharID = (int)input[i];
                thisChar = input[i].ToString();
                if ((WhitelistCodes[thisCharID] != null)
                    || (thisCharID == 32) || (thisCharID == 44)    //escaping SPACE and COMMA for URL Encoding
                     )
                {
                    // Character needs to be encoded to default UTF-8.
                    inputEncoding = Encoding.UTF8;
                    byte[] inputEncodingBytes = inputEncoding.GetBytes(thisChar);
                    int noinputEncodingBytes = inputEncodingBytes.Length;
                    for (int index = 0; index < noinputEncodingBytes; index++)
                    {
                        ch = (char)inputEncodingBytes[index];

                        // character needs to be encoded. Infact the byte cannot be greater than 256.
                        if (ch <= 256)
                        {
                            returnMe[len++] = '%';
                            String hex = ((int)ch).ToString("x").PadLeft(2, '0');
                            returnMe[len++] = hex[0];
                            returnMe[len++] = hex[1];
                        }
                    }
                }
                else
                {
                    // character does not need encoding
                    returnMe[len++] = input[i];
                }
            }
            return new String(returnMe, 0, len);
        }

        ///---------------------------------------------------------------------
        /// <summary>
        /// Encodes input strings for use in universal resource locators (URLs).
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="codepage">Codepage number of the input</param>
        /// <returns>
        ///     Encoded string for use in URLs.
        /// </returns>
        /// <remarks>
        /// This function will encodes the output as per the encoding parameter (codepage) passed to it. It will encode all but known safe characters.  Encoded characters are encoded using the %SINGLE_BYTE_HEX and %DOUBLE_BYTE_HEX notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSSあAttack!');</term><description>alert%28%27XSS%82%a0Attack%21%27%29%3b</description></item>
        /// <item><term>user@contoso.com</term><description>user%40contoso.com</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross%20Site%20Scripting%20Library</description></item>
        /// </list></remarks>
        ///---------------------------------------------------------------------
        public static string UrlEncode(string input, int codepage)
        {
            if (String.IsNullOrEmpty(input))
                return string.Empty;

            int len = 0;
            int thisCharID;
            int tLen = input.Length;

            char ch;
            string thisChar;
            Encoding inputEncoding = null;

            // Use a new char array.
            char[] returnMe = new char[tLen * 24]; // worst case length scenario            

            for (int i = 0; i < tLen; i++)
            {
                thisCharID = (int)input[i];
                thisChar = input[i].ToString();
                if ((WhitelistCodes[thisCharID] != null)
                    || (thisCharID == 32) || (thisCharID == 44)    //escaping SPACE and COMMA for URL Encoding
                     )
                {
                    // character needs to be encoded
                    inputEncoding = Encoding.GetEncoding(codepage);
                    byte[] inputEncodingBytes = inputEncoding.GetBytes(thisChar);
                    int noinputEncodingBytes = inputEncodingBytes.Length;
                    for (int index = 0; index < noinputEncodingBytes; index++)
                    {
                        ch = (char)inputEncodingBytes[index];

                        // character needs to be encoded. Infact the byte cannot be greater than 256.
                        if (ch <= 256)
                        {
                            returnMe[len++] = '%';
                            String hex = ((int)ch).ToString("x").PadLeft(2, '0');
                            returnMe[len++] = hex[0];
                            returnMe[len++] = hex[1];
                        }
                    }
                }
                else
                {
                    // character does not need encoding
                    returnMe[len++] = input[i];
                }
            }

            return new String(returnMe, 0, len);
        }

        #endregion

        #region XMLEncode_Method
        ///---------------------------------------------------------------------
        /// <summary>
        /// Encodes input strings for use in XML.
        /// </summary>
        /// <param name="input">String to be encoded</param>
        /// <returns>
        ///     Encoded string for use in XML.
        /// </returns>
        /// <remarks>
        /// This function will encode all but known safe characters.  Encoded characters are encoded using the &amp;#DECIMAL; notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>,</term><description>Comma</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Space</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description>alert&#40;&#39;XSS Attack&#33;&#39;&#41;&#59;</description></item>
        /// <item><term>user@contoso.com</term><description>user&#64;contoso.com</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross Site Scripting Library</description></item>
        /// </list></remarks>
        ///---------------------------------------------------------------------
        public static string XmlEncode(string input)
        {
            // HtmlEncode will handle input
            return HtmlEncode(input);
        }
        #endregion

        #region XMLAttributeEncode_Method
        ///---------------------------------------------------------------------
        /// <summary>
        /// Encodes input strings for use in XML attributes.
        /// </summary>
        /// <param name="input">String to be encoded</param>
        /// <returns>
        ///     Encoded string for use in XML attributes.
        /// </returns>
        /// <remarks>
        /// This function will encode all but known safe characters.  Encoded characters are encoded using the &amp;#DECIMAL; notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>,</term><description>Comma</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description>alert&#40;&#39;XSS&#32;Attack&#33;&#39;&#41;&#59;</description></item>
        /// <item><term>user@contoso.com</term><description>user&#64;contoso.com</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross&#32;Site&#32;Scripting&#32;Library</description></item>
        /// </list></remarks>
        ///---------------------------------------------------------------------
        public static string XmlAttributeEncode(string input)
        {
            //HtmlEncodeAttribute will handle input
            return HtmlAttributeEncode(input);
        }
        #endregion

        #region JavaScriptEncode_Method
        ///---------------------------------------------------------------------
        /// <summary>
        /// Encodes input strings for use in JavaScript.
        /// </summary>
        /// <param name="input">String to be encoded</param>
        /// <returns>
        ///     Encoded string for use in JavaScript.
        /// </returns>
        /// <remarks>
        /// This function will encode all but known safe characters.  Encoded characters are encoded using the \xSINGLE_BYTE_HEX and \uDOUBLE_BYTE_HEX notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>,</term><description>Comma</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Space</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description>'alert\x28\x27XSS Attack\x21\x27\x29\x3b'</description></item>
        /// <item><term>user@contoso.com</term><description>'user\x40contoso.com'</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>'Anti-Cross Site Scripting Library'</description></item>
        /// </list></remarks>
        ///---------------------------------------------------------------------
        public static string JavaScriptEncode(string input)
        {
            return JavaScriptEncode(input, true);
        }

        ///---------------------------------------------------------------------
        /// <summary>
        /// Encodes input strings for use in JavaScript.
        /// </summary>
        /// <param name="input">String to be encoded</param>
        /// /// <param name="flagforQuote">bool flag to determin to emit quote or not. true - emit quote. false = no quote.</param>
        /// <returns>
        ///     Encoded string for use in JavaScript and does not return the output with en quotes.
        /// </returns>
        /// <remarks>
        /// This function will encode all but known safe characters.  Encoded characters are encoded using the \xSINGLE_BYTE_HEX and \uDOUBLE_BYTE_HEX notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>,</term><description>Comma</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Space</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description>'alert\x28\x27XSS Attack\x21\x27\x29\x3b'</description></item>
        /// <item><term>user@contoso.com</term><description>'user\x40contoso.com'</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>'Anti-Cross Site Scripting Library'</description></item>
        /// </list></remarks>
        ///---------------------------------------------------------------------
        public static string JavaScriptEncode(string input, bool flagforQuote)
        {
            // Input validation: empty or null string condition
            if (String.IsNullOrEmpty(input))
            {
                if (flagforQuote)
                    return (EmptyStringJavaScript);
                else
                    return "";
            }
            // Use a new char array.
            int len = 0;
            int tLen = input.Length;
            char[] returnMe = new char[tLen * 8]; // worst case length scenario
            char[] thisChar;
            char ch;
            int thisCharID;

            // First step is to start the encoding with an apostrophe if flag is true.
            if (flagforQuote)
                returnMe[len++] = '\'';

            for (int i = 0; i < tLen; i++)
            {
                thisCharID = (int)input[i];
                ch = input[i];
                if (WhitelistCodes[thisCharID] != null)
                {
                    // character needs to be encoded
                    thisChar = WhitelistCodes[thisCharID];
                    if (thisCharID > 127)
                    {
                        returnMe[len++] = '\\';
                        returnMe[len++] = 'u';
                        String hex = ((int)ch).ToString("x").PadLeft(4, '0');
                        returnMe[len++] = hex[0];
                        returnMe[len++] = hex[1];
                        returnMe[len++] = hex[2];
                        returnMe[len++] = hex[3];
                    }
                    else
                    {
                        returnMe[len++] = '\\';
                        returnMe[len++] = 'x';
                        String hex = ((int)ch).ToString("x").PadLeft(2, '0');
                        returnMe[len++] = hex[0];
                        returnMe[len++] = hex[1];
                    }

                }
                else
                {
                    // character does not need encoding
                    returnMe[len++] = input[i];
                }
            }

            // Last step is to end the encoding with an apostrophe if flag is true.
            if (flagforQuote)
                returnMe[len++] = '\'';

            return new String(returnMe, 0, len);
        }
        #endregion

        #region VisualBasicScriptEncode_Method
        ///---------------------------------------------------------------------
        /// <summary>
        /// Encodes input strings for use in Visual Basic Script.
        /// </summary>
        /// <param name="input">String to be encoded</param>
        /// <returns>
        ///     Encoded string for use in Visual Basic Script.
        /// </returns>
        /// <remarks>
        /// This function will encode all but known safe characters.  Encoded characters are 
        /// encoded using the &#38;chrw(DECIMAL) notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>,</term><description>Comma</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Space</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description>"alert"&#38;chrw(40)&#38;chrw(39)&#38;"XSS Attack"&#38;chrw(33)&#38;chrw(39)&#38;chrw(41)&#38;chrw(59)</description></item>
        /// <item><term>user@contoso.com</term><description>"user"&#38;chrw(64)&#38;"contoso.com"</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>"Anti-Cross Site Scripting Library"</description></item>
        /// </list></remarks>
        ///---------------------------------------------------------------------
        public static string VisualBasicScriptEncode(string input)
        {

            // Input validation: empty or null string condition
            if (String.IsNullOrEmpty(input))
                return (EmptyStringVBS);

            // Use a new char array.
            int len = 0;
            int tLen = input.Length;
            char[] returnMe = new char[tLen * 12]; // worst case length scenario
            char ch2;
            string temp;
            int thisCharID;

            //flag to surround double quotes around safe characters
            bool bInQuotes = false;

            for (int i = 0; i < tLen; i++)
            {
                thisCharID = (int)input[i];
                ch2 = input[i];
                if (WhitelistCodes[thisCharID] != null)
                {
                    // character needs to be encoded

                    // surround in quotes
                    if (bInQuotes)
                    {
                        // get out of quotes
                        returnMe[len++] = '"'; ;
                        bInQuotes = false;
                    }

                    // adding "encoded" characters
                    temp = "&chrw(" + ((uint)ch2).ToString() + ")";
                    foreach (char ch in temp)
                    {
                        returnMe[len++] = ch;
                    }
                }
                else
                {
                    // character does not need encoding

                    //surround in quotes
                    if (!bInQuotes)
                    {
                        // add quotes to start
                        returnMe[len++] = '&';
                        returnMe[len++] = '"';
                        bInQuotes = true;
                    }

                    returnMe[len++] = input[i];
                }
            }
            // if we're inside of quotes, close them
            if (bInQuotes)
                returnMe[len++] = '"';

            // finally strip extraneous "&" from beginning of the string, if necessary and RETURN
            if (returnMe.Length > 0 && returnMe[0] == '&')
                return new String(returnMe, 1, len - 1);
            else
                return new String(returnMe, 0, len);
        }

        #endregion

        #endregion
    }
}
