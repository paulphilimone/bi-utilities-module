using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mz.betainteractive.encoding.encryptation {
    public class BetaEncryptation {
        private char[] keys = { '0', '1', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        Random randomKey = new Random();

        private char[] hexNumbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        private char[] lowerCaseLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        private Dictionary<String, char> mapHexToChar;
        private Dictionary<String, char> mapLettersToChar;
        private Dictionary<char, String> mapCharToHex;
        private Dictionary<char, String> mapCharToLetters;

        public BetaEncryptation() {
            mapHexToChar = new Dictionary<string, char>();
            mapCharToHex = new Dictionary<char, string>();

            mapLettersToChar = new Dictionary<string, char>();
            mapCharToLetters = new Dictionary<char, string>();

            //init hex map
            int bit = 0;
            int posicao = 0;

            for (int i = 0; i < hexNumbers.Length; i++) {
                char value = hexNumbers[i];
                String key = bit + "" + posicao;

                mapHexToChar.Add(key, value);
                mapCharToHex.Add(value, key);

                posicao = (bit == 1) ? ++posicao : posicao;
                bit = (bit == 0) ? 1 : 0;
            }

            //test
            bit = 0;
            posicao = 0;

            //we end in 16th character, because we dont have more combinations
            for (int i = 0; i < hexNumbers.Length; i++) {
                char value = lowerCaseLetters[i];
                String key = bit + "" + posicao;

                mapLettersToChar.Add(key, value);
                mapCharToLetters.Add(value, key);

                posicao = (bit == 1) ? ++posicao : posicao;
                bit = (bit == 0) ? 1 : 0;
                //System.out.println(value + " - " + toStringBits(value.charValue()) + " - " + key);
            }
        }

        public static BetaEncryptation getEncryptation() {
            return new BetaEncryptation();
        }

        public void Encrypt(string SerialNumber, string CreationDate, out string Encrypted, out string ProductKey) {
            Random ra = new Random();

            string SN = SerialNumber;
            string CD = CreationDate;
            string Key = SN + CD;
            string BetaKey = "$BSGEKv1%";//Beta Sisga Enkript v1.0
            int LenSN = SN.Length;
            int LenCD = CD.Length;
            int random = ra.Next(256);

            string encbase = BetaKey + LenSN + "&" + LenCD + "&" + random + "?";
            string enckey = ""; //Encrypted String
            string encrypted = "";
            string pKey = "";
            char[] keychars = Key.ToCharArray();

            for (int pos = 0; pos < keychars.Length; pos++) {
                char chr = keychars[pos];
                byte chb = (byte)chr;

                //inverter bits, i - inverted               
                byte chbi = (byte)(chb ^ 255);
                char chri = (char)chbi;

                byte n = chbi;

                char en1 = (char)Sum(n, random + pos);
                //orig: char en2 = (char)Sub(n, random + pos);
                //For the second char we, generate a key
                char en2 = getRandomKey();
                string encr = en1 + "" + en2;
                enckey += encr;
                pKey += en2 + "";
                Console.WriteLine(chr + " <-> " + chb + " to " + encr + " <-> " + (byte)en1 + "," + (byte)en2);
            }

            encrypted = encbase + enckey;

            Encrypted = encrypted;
            ProductKey = pKey;

            Console.WriteLine("Normal key   : " + Key);
            Console.WriteLine("Encrypted key: " + encrypted);

            //return encrypted;
        }

        public bool Decrypt(string encrypted_key, out string SerialNumber, out string CreationDate) {

            try {

                string BetaKey = "$BSGEKv1%";
                string encrypted = encrypted_key; //"$BSGEKv1%13&19&226?±í±ë²ê±ç²æ³å¶æ·å¶âºä»ã»á·Û¼Þ¿ß¿ÝÀÜÅßÃÛÁ×ÈÜÃÕÇ×ØæÈÔÄÎÁÉÇÍÌÐÄÆÌÌÇÅ";
                //Get encbase
                string encbase = encrypted.Remove(encrypted.IndexOf('?'));
                string enckey = encrypted.Substring(encrypted.IndexOf('?') + 1);
                string Key = "";

                Console.WriteLine("encbase: " + encbase);
                Console.WriteLine("enckey : " + enckey);

                string[] snum = encbase.Replace(BetaKey, "").Split('&');

                int LenSN = Convert.ToInt32(snum[0]);
                int LenCD = Convert.ToInt32(snum[1]);
                int random = Convert.ToInt32(snum[2]);

                Console.WriteLine("LenSN  = " + LenSN);
                Console.WriteLine("LenCD  = " + LenCD);
                Console.WriteLine("random = " + random);

                char[] keychars = enckey.ToCharArray();

                //pos+=2, porque a chave encriptada é 2xMaior
                //Cada caracter equivale a dois caracteres
                //e só preciso o 1º caracter, ignoramos o 2º

                for (int pos = 0, p = 0; pos < keychars.Length; pos += 2, p++) {
                    char en1 = keychars[pos];
                    char en2 = keychars[pos + 1];//ignoramos
                    //As somas, queremos "n":
                    //char en1 = (char)Sum(n, random + pos);
                    //char en2 = (char)Sub(n, random + pos);
                    byte n = Sub((byte)en1, random + p);
                    //Invertemos n, com um XOR
                    byte chbi = (byte)(n ^ 255);

                    char chr = (char)chbi;

                    Console.WriteLine(en1 + " to " + chr);
                    Key += chr;
                }

                string SN = Key.Substring(0, LenSN);        //Serial Number
                string CD = Key.Substring(SN.Length);

                Console.WriteLine("SN: " + SN);
                Console.WriteLine("CD: " + CD);

                SerialNumber = SN;
                CreationDate = CD;

            } catch (Exception ex) {
                SerialNumber = "";
                CreationDate = "";
                return false;
            }

            return true;
        }

        private char getRandomKey() {
            int x = randomKey.Next(keys.Length);
            return keys[x];
        }

        private byte Sum(byte number, int add) {
            return (byte)((number + (add % 256)) % 256);
        }

        private byte Sub(byte number, int add) {
            byte n = (byte)((number - (add % 256)) % 256);
            if (n < 0) {
                return (byte)(256 + n);
            } else {
                return n;
            }
        }

        /**
     * Retorna array de 32bits
     * Retorna uma array de 0 e 1s invertida
     * Onde a posição 0 da array representa o bit(31)
     */
        private int[] GetBits(int num) {
            int[] bit = new int[32];

            for (int i = bit.Length - 1, x = 0; i >= 0; i--, x++) {
                //the bit pos we want <-|    |-> just one bit
                bit[x] = ((num >> i) & 1);
            }

            return bit;
        }

        /**
         * Retorna array de 8bits
         * Retorna uma array de 0 e 1s invertida
         * Onde a posição 0 da array representa o bit(7)
         */
        public static int[] GetBits(byte num) {
            int[] bit = new int[8];

            for (int i = bit.Length - 1, x = 0; i >= 0; i--, x++) {
                //the bit pos we want <-|    |-> just one bit
                bit[x] = ((num >> i) & 1);
            }

            return bit;
        }

        public static int[] GetBits(char carater) {
            return GetBits((byte)carater);
        }

        public static byte GetByte(int[] bit) {
            byte num = 0;

            for (int i = 0, x = bit.Length - 1; i < bit.Length; i++, x--) {
                num = (byte)(num | (bit[i] << x));
            }

            return num;
        }

        private int[] GetBits(int num, int number_of_bits) {

            if (number_of_bits > 32) {
                number_of_bits = 32;
            }

            int[] bit = new int[number_of_bits];

            for (int i = bit.Length - 1, x = 0; i >= 0; i--, x++) {
                //the bit pos we want <-|    |-> just one bit
                bit[x] = ((num >> i) & 1);
            }

            return bit;
        }

        /**
         * Recebe uma array de bits cuja a posição 0 da array
         * representa o ultimo bit
         */
        private int GetNumber(int[] bit) {
            int num = 0;

            for (int i = 0, x = bit.Length - 1; i < bit.Length; i++, x--) {
                num = num | (bit[i] << x);
            }

            return num;
        }

        public String EncodePassword(String senha) {
            String encoded = "";//+EncKeyGen();

            for (int i = 0; i < senha.Length; i++) {
                char c = senha.ToCharArray()[i];
                encoded += EncodeChar(c, i) + EncodeChar(c, i * 100) + EncodeChar(c, i * 47);
            }

            return encoded;
        }

        public String DecodePassword(String encoded) {
            String decoded = "";

            int start = 0, i = 0;

            try {
                while (start < encoded.Length) {
                    String str = encoded.Substring(start, 8);
                    decoded += DecodeChar(str, i);
                    start = start + (8 * 3);
                    i++;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                //ex.printStackTrace();
                return null;
            }

            return decoded;
        }

        private String EncodeChar(char c, int position) {
            //int[] rnd = {0,1,}
            byte value = (byte)(position ^ 170);
            int[] bits = GetBits(c);
            int[] conversionType = GetBits(value);

            String encoded = "";

            for (int i = 0; i < conversionType.Length; i++) {
                char character = '\0';
                String key = bits[i] + "" + i;

                if (conversionType[i] == 0) {
                    //convert bit to letter
                    mapLettersToChar.TryGetValue(key, out character);
                } else {
                    //convert bit to hex
                    mapHexToChar.TryGetValue(key, out character);
                }

                encoded += character;
            }

            return encoded;
        }

        private char DecodeChar(String str, int position) {
            byte value = (byte)(position ^ 170);
            int[] bits = new int[8];
            int[] conversionType = GetBits(value);


            for (int i = 0; i < conversionType.Length; i++) {
                char character = str.ToCharArray()[i];
                String key = "";

                if (conversionType[i] == 0) {
                    //convert letter to bit
                    mapCharToLetters.TryGetValue(character, out key);
                } else {
                    //convert hex to bit
                    mapCharToHex.TryGetValue(character, out key);
                }

                int bit = Convert.ToInt32(key.ToCharArray()[0] + "");
                int pos = Convert.ToInt32(key.ToCharArray()[1] + "");

                bits[pos] = bit;
            }

            char c = (char)GetByte(bits);

            return c;
        }

        private string EncKeyGen() {
            int n = randomKey.Next(10);

            string key = "" + n;

            for (int i = 1; i <= n; i++) {
                bool upper = (n % 2 == 0);

                int x = randomKey.Next(keys.Length);
                String c = "" + keys[x];

                key += c.ToLower();
            }

            return key;
        }

    }

    [Serializable]
    public class BTSKey {
        public string Company;
        public string EncryptedKey;
        public string ProductKey;
        public bool Used;

        public BTSKey(string company, string encrypted, string key) {
            this.Company = company;
            this.EncryptedKey = encrypted;
            this.ProductKey = key;
            this.Used = false;
        }
    }

    public class BTSKeyBindChanger : System.Runtime.Serialization.SerializationBinder {
        public override Type BindToType(string assemblyName, string typeName) {
            // Define the new type to bind to 
            Type typeToDeserialize = null;
            // Get the assembly names

            string currentAssembly = "BetaEncryptation";
            typeName = "BTSKeyV1.0";

            // Create the new type and return it 
            typeToDeserialize = Type.GetType(string.Format("{0}, {1}", typeName, currentAssembly));
            return typeToDeserialize;
        }
    }
}
