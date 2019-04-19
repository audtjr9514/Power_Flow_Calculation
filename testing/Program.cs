using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            StreamReader file = new System.IO.StreamReader("C:\\Users\\myoun\\Desktop\\메추라기\\기러기\\testing\\3.DAT");
            int[,] hang1 = new int[3, 9];
            int[,] hang2 = new int[3, 6];
            while ((line = file.ReadLine()) != null)            // 파일을 줄 단위로 읽어 온다 _ 만약 그 줄이 비어 있지 않다면 반복을 해라
            {
                if (line.Equals("\t999999")) continue;          // 만일 그 줄이 \t999999 라면 다음 반복으로
                if (line.Equals("\t")) continue;                // 만일 그 줄이 \t 라면 다음 반복으로
                if (line.Equals("")) continue;                  // 만일 그 줄이 아무것도 없다면 다음 반복으로
                string replace = line.Replace("\t", "");        //
                string replace2 = replace.Replace("   ", " ");
                string replaceSTR = replace2.Replace("  ", " ");

                string[] token = replaceSTR.Split(' ');
                for (int i = 0; i < token.Length; i++)
                {
                    if (token.Length == 9)
                        continue;
                    Console.Write("{0}\t", token[i]);
                }
                Console.WriteLine();
            }
        }
    }
}
