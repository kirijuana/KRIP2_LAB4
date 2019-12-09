using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace KRIP2_LAB4_EP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            int[] s = new int[64] { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };
            // T
            uint[] K = new uint[64] {
            0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
            0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
            0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
            0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
            0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
            0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
            0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
            0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
            0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
            0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
            0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
            0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
            0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
            0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
            0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
            0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
        };
            String open_text = (textBox5.Text);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in System.Text.Encoding.Unicode.GetBytes(open_text))
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            string binaryStr_orig = sb.ToString();
            string binaryStr = sb.ToString() + "1"; //дописываем единичный бит
            int check = 0;
            if (448 == (binaryStr.Length % 512))
            {
                check = 512;
            }
            else
            {
                check = 448 - binaryStr.Length;
            }

            for (int i = 0; i < check; i++)
            {
                binaryStr = binaryStr + "0";
            }

            // шаг 2
            double length_2 = 0;
            if (binaryStr_orig.Length > Math.Pow(2, 64) - 1)
            {
                length_2 = binaryStr_orig.Length % Math.Pow(2, 64);
            }
            else
            {
                length_2 = binaryStr_orig.Length;
            }

            String str_64 = Convert.ToString((int)length_2, 2);

            if (str_64.Length < 64)
            {
                check = 64 - str_64.Length;
                for (int i = 0; i < check; i++)
                {
                    str_64 = "0" + str_64;
                }
            }

            for (int i = 32; i < 64; i++) // запись младших битов
            {
                binaryStr = binaryStr + str_64[i];
            }

            for (int i = 0; i < 32; i++) // запись страших битов
            {
                binaryStr = binaryStr + str_64[i];
            }

            // шаг 3
            uint a0 = 0x67452301;   // A
            uint b0 = 0xefcdab89;   // B
            uint c0 = 0x98badcfe;   // C
            uint d0 = 0x10325476;   // D
            var binaryStr_copy = new byte[binaryStr.Length];
            // шаг 4
            for (int i = 0; i < binaryStr.Length; i++)
            {
                if (binaryStr[i] == '1')
                {
                    binaryStr_copy[i] = 1;
                }
                else
                {
                    binaryStr_copy[i] = 0;
                }
            }

            for (int i = 0; i < binaryStr.Length / 64; ++i)
            {
               
                uint[] M = new uint[16];
                for (int j = 0; j < 16; ++j)
                    M[j] = BitConverter.ToUInt32(binaryStr_copy, (i * 64) + (j * 4));

                
                uint A = a0, B = b0, C = c0, D = d0, F = 0, g = 0;

               
                for (uint k = 0; k < 64; ++k)
                {
                    if (k <= 15)
                    {
                        F = (B & C) | (~B & D);
                        g = k;
                    }
                    else if (k >= 16 && k <= 31)
                    {
                        F = (D & B) | (~D & C);
                        g = ((5 * k) + 1) % 16;
                    }
                    else if (k >= 32 && k <= 47)
                    {
                        F = B ^ C ^ D;
                        g = ((3 * k) + 5) % 16;
                    }
                    else if (k >= 48)
                    {
                        F = C ^ (B | ~D);
                        g = (7 * k) % 16;
                    }

                    var dtemp = D;
                    D = C;
                    C = B;
                    B = B + ((A + F + K[k] + M[g]) << s[k]) | ((A + F + K[k] + M[g]) >> (32 - s[k]));
                    A = dtemp;
                }

                a0 += A;
                b0 += B;
                c0 += C;
                d0 += D;
            }
            textBox9.Text = String.Join("", BitConverter.GetBytes(a0).Select(y => y.ToString("x2"))) + String.Join("", BitConverter.GetBytes(b0).Select(y => y.ToString("x2"))) + String.Join("", BitConverter.GetBytes(c0).Select(y => y.ToString("x2"))) + String.Join("", BitConverter.GetBytes(d0).Select(y => y.ToString("x2")));
            String digest = textBox9.Text;
            // шаг 5
            BigInteger E = BigInteger.Parse(textBox1.Text), N = BigInteger.Parse(textBox7.Text);

            char[] alph = "-абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ".ToCharArray();

            // шифрование

            string BinaryValue = null;
            BigInteger x1 = 0;
            while (E > 0)
            {

                x1 = E % 2;
                if (E == 0)
                    break;
                BinaryValue = BinaryValue + x1.ToString();
                E /= 2;
                //    x = Math.Truncate(x);
            }

           // String digest = (textBox5.Text);
            byte[] asciiBytes_open = Encoding.Unicode.GetBytes(open_text);
            byte[] asciiBytes_shifr = new byte[asciiBytes_open.Length];

            for (int i = 0; i < digest.Length/*asciiBytes_open.Length*/; i++)
            {

                int n = BinaryValue.Length;
                BigInteger c = 1, s_1 = (BigInteger)digest[i];
                for (int j = 0; j < alph.Length; j++)
                {
                    if (digest[i] == alph[j])
                    {
                        s_1 = j;
                    }
                }

                for (int j = 0; j < n; j++)
                {
                    if (BinaryValue[j] == '1')
                    {
                        c = (c * s_1) % N;
                    }
                    s_1 = (s_1 * s_1) % N;
                }
                if (i + 1 == digest.Length)
                    textBox6.Text = textBox6.Text + c;
                else
                    textBox6.Text = textBox6.Text + c + ", ";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // расшифрование
            BigInteger D = BigInteger.Parse(textBox4.Text), N = BigInteger.Parse(textBox7.Text);
            char[] alph = "-абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ".ToCharArray();
            String shifr_text = textBox6.Text;
            string BinaryValue_2 = null;
            BigInteger x2 = 0;
            while (D > 0)
            {
                x2 = D % 2;
                if (D == 0)
                    break;
                BinaryValue_2 = BinaryValue_2 + x2.ToString();
                D /= 2;
            }

            BigInteger[] intArray = shifr_text.Split(',').Select(x => BigInteger.Parse(x)).ToArray();


            for (int i = 0; i < intArray.Length; i++)
            {

                int n = BinaryValue_2.Length;

                BigInteger m = 1, s = intArray[i];
                for (int j = 0; j < n; j++)
                {
                    if (BinaryValue_2[j] == '1')
                    {
                        m = (m * s) % N;
                    }
                    s = (s * s) % N;
                }
                textBox9.Text = textBox9.Text + alph[(int)m];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BigInteger P = BigInteger.Parse(textBox3.Text), E = 0, Q = BigInteger.Parse(textBox2.Text);
            BigInteger N = 0, Z = 0;
            BigInteger ost = 0, ost_2 = 0, mul = 0, a1 = 0, b1 = 0;
            N = P * Q;
            textBox7.Text = N.ToString();
            Z = (P - 1) * (Q - 1);
            textBox8.Text = Z.ToString();

            bool[] table;
            List<BigInteger> primearray;
            if (Z < 3000000)
            {
                table = new bool[(ulong)Z];
                primearray = new List<BigInteger>();
                for (BigInteger i = 0; i < Z; i++)
                    table[(ulong)i] = true;
                for (BigInteger i = 2; i * i < Z; i++)
                    if (table[(ulong)i])
                        for (var j = 2 * i; j < Z; j += i)
                            table[(ulong)j] = false;
                for (BigInteger i = 1; i < Z; i++)
                {
                    if (table[(ulong)i])
                    {
                        primearray.Add(i);
                    }
                }
            }
            else
            {
                table = new bool[(ulong)3000000];
                primearray = new List<BigInteger>();
                for (BigInteger i = 0; i < 3000000; i++)
                    table[(ulong)i] = true;
                for (BigInteger i = 2; i * i < 3000000; i++)
                    if (table[(ulong)i])
                        for (var j = 2 * i; j < 3000000; j += i)
                            table[(ulong)j] = false;
                for (BigInteger i = 1; i < 3000000; i++)
                {
                    if (table[(ulong)i])
                    {
                        primearray.Add(i);
                    }
                }
            }

            //проверка на взаимную простоту
            BigInteger[] U_1 = new BigInteger[3];
            BigInteger[] V_1 = new BigInteger[3];
            BigInteger[] T_1 = new BigInteger[3];
            do
            {

                Random rnd = new Random();

                int value = rnd.Next(0, primearray.Count);
                E = primearray[value];

                U_1[0] = Z;
                U_1[1] = 1;
                U_1[2] = 0;
                V_1[0] = E;
                V_1[1] = 0;
                V_1[2] = 1;
                ost = Z; ost_2 = 0; mul = 1; a1 = Z; b1 = E;
                BigInteger q_1 = 0;
                while (V_1[0] != 0)
                {

                    q_1 = U_1[0] / V_1[0];
                    T_1[0] = U_1[0] % V_1[0];
                    T_1[1] = U_1[1] - q_1 * V_1[1];
                    T_1[2] = U_1[2] - q_1 * V_1[2];
                    U_1[0] = V_1[0];
                    U_1[1] = V_1[1];
                    U_1[2] = V_1[2];
                    V_1[0] = T_1[0];
                    V_1[1] = T_1[1];
                    V_1[2] = T_1[1];
                }
                textBox1.Text = E.ToString();

            } while (U_1[0] != 1);


            // вычисление D
            BigInteger[] U = new BigInteger[2];
            BigInteger[] V = new BigInteger[2];
            BigInteger[] T = new BigInteger[2];

            U[0] = Z;
            U[1] = 0;
            V[0] = E;
            V[1] = 1;
            //    ost = Z; ost_2 = 0; mul = 1; a1 = Z; b1 = E;
            BigInteger D = 0, q = 0;
            while (V[0] != 0)
            {
                D = T[1];
                q = U[0] / V[0];
                T[0] = U[0] % V[0];
                T[1] = U[1] - q * V[1];
                U[0] = V[0];
                U[1] = V[1];
                V[0] = T[0];
                V[1] = T[1];
            }
            if (D < 0)
                D += a1;
            if (D == E)
                D = D + Z;
            textBox4.Text = D.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool[] table;
            List<BigInteger> primearray;

            table = new bool[(ulong)3000000];
            primearray = new List<BigInteger>();
            for (BigInteger i = 0; i < 3000000; i++)
                table[(ulong)i] = true;
            for (BigInteger i = 2; i * i < 3000000; i++)
                if (table[(ulong)i])
                    for (var j = 2 * i; j < 3000000; j += i)
                        table[(ulong)j] = false;
            for (BigInteger i = 1; i < 3000000; i++)
            {
                if (table[(ulong)i])
                {
                    primearray.Add(i);
                }
            }
            Random rnd = new Random();
            int value = rnd.Next(0, primearray.Count);
            textBox2.Text = primearray[value].ToString();
            value = rnd.Next(0, primearray.Count);
            textBox3.Text = primearray[value].ToString();
        }
    }
}
