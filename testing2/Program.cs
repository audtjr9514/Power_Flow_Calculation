using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;


namespace project
{
    class Program
    {
        // 행렬 A의 상수 선언 최대 100 x 9
        const int MAT_A_COL = 100;   // 행렬 A의 행 개수
        const int MAT_A_LOW = 9;    // 행렬 A의 열 개수
        // 행렬 B의 상수 선언 최대 100 x 6
        const int MAT_B_COL = 100;   // 행렬 B의 행 개수
        const int MAT_B_LOW = 6;    // 행렬 B의 열 개수

        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\myoun\\Desktop\\조류 계산\\testing2\\57.DAT";

            int n = 0, s = 0;
            double[] x1 = new double[MAT_A_LOW]; // 행렬A을 한 줄 담기 위한 임시 배열
            double[] x2 = new double[MAT_B_LOW]; // 행렬B를 한 줄 담기 위한 임시 배열  
            double[,] BUS1 = new double[MAT_A_COL, MAT_A_LOW];  // 행렬 A
            double[,] BUS2 = new double[MAT_B_COL, MAT_B_LOW];  // 행렬 B
            using (FileStream fsIn = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fsIn)) // 읽어온 파일을 스트림 리더로 받는다        
                {
                    while (sr.Peek() > -1)
                    {
                        string temp_str = (sr.ReadLine().ToString()).Trim().Replace("\t", " ");  // 줄 단위로 읽는다 _ Trim (양 옆 공백 제거) _ 탭 문자를 기본 공백으로 대체
                        string input = ""; // 진짜 체크할 문자열 초기화
                        foreach (string str in temp_str.Split(' ')) // temp_str을 공백문자를 기준으로 잘라 반복
                            if (str != " " && str != "")    // 만약 str이 공백이 아니고 아무것도 없지 않다면
                                input = input + str + "\t"; // input에 str과 탭을 붙힌다.


                        if (temp_str.Equals("999999"))     // 그 줄이 999999라면 다음 반복
                            continue;
                        if (temp_str.Equals(""))           // 그 줄이 아무것도 없다면 다음 반복
                            continue;

                        string[] token = input.Split('\t'); // 탭 단위로 토큰 끊기
                        if (token.Length == MAT_A_LOW + 1)      // 토큰의 갯수가 9개라면
                        {
                            for (int j = 0; j < token.Length - 1; j++)  // 각 토큰을 x1의 배열에 넣는다.
                                x1[j] = Convert.ToDouble(token[j].ToString());
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", x1[0], x1[1], x1[2], x1[3], x1[4], x1[5], x1[6], x1[7], x1[8]);
                            for (int j = 0; j < 9; j++)     // 배열을 행렬 A의 각 1줄에 넣는다.
                                BUS1[n, j] = x1[j];
                            n++;
                        }
                        else if (token.Length == MAT_B_LOW + 1) // 토큰의 갯수가 6개라면
                        {
                            for (int j = 0; j < token.Length - 1; j++)
                                x2[j] = Convert.ToDouble(token[j].ToString());
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", x2[0], x2[1], x2[2], x2[3], x2[4], x2[5]);
                            for (int j = 0; j < 6; j++)
                                BUS2[s, j] = x2[j];
                            s++;
                        }
                    }
                }
            }

            Console.WriteLine("\n행렬 A");
            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < MAT_A_LOW; k++)
                    Console.Write("{0}\t", BUS1[i, k]);
                Console.WriteLine();
            }
            Console.WriteLine("\n행렬 B");
            for (int i = 0; i < s; i++)
            {
                for (int k = 0; k < MAT_B_LOW; k++)
                    Console.Write("{0}\t", BUS2[i, k]);
                Console.WriteLine();
            }
        }
    }
}