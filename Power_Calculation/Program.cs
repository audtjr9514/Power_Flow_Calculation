﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace project
{
    class Program
    {
        // 행렬 A의 상수 선언 최대 500 x 9
        const int MAT_A_COL = 500;   // 행렬 A의 행 개수
        const int MAT_A_LOW = 9;    // 행렬 A의 열 개수
        // 행렬 B의 상수 선언 최대 500 x 6
        const int MAT_B_COL = 500;   // 행렬 B의 행 개수
        const int MAT_B_LOW = 6;    // 행렬 B의 열 개수

        public static int Bn = 0, Ln = 0, i = 0, k = 0, j = 0;
        public static double t = 0;
        public static double[] x1 = new double[MAT_A_LOW]; // 행렬A을 한 줄 담기 위한 임시 배열
        public static double[] x2 = new double[MAT_B_LOW]; // 행렬B를 한 줄 담기 위한 임시 배열  
        public static double[,] BUSDATA = new double[MAT_A_COL, MAT_A_LOW];  // BUS 데이터 행렬
        public static double[] P = new double[MAT_A_COL]; //유효전력
        public static double[] Q = new double[MAT_A_COL]; // 무효전력
        public static double[] Px = new double[MAT_A_COL]; //유효전력에서 발전량-부하량(고정)
        public static double[] Qx = new double[MAT_A_COL]; // 무효전력에서 발전량-부하량(고정)
        public static double[] Pg = new double[MAT_A_COL]; // 유효전력 발전량
        public static double[] Qg = new double[MAT_A_COL]; // 무효전력 발전량
        public static double[] Pl = new double[MAT_A_COL]; // 유효전력 부하량
        public static double[] Ql = new double[MAT_A_COL]; // 무효전력 부하량
        public static double[] dP = new double[MAT_A_COL]; // △p값
        public static double[] dQ = new double[MAT_A_COL]; // △q값
        public static double[] Vp = new double[MAT_A_COL]; // V값
        public static double[] Theta = new double[MAT_A_COL]; // 위상각(rad)
        public static int[] BUS = new int[MAT_A_COL]; // 모선번호
        public static int[] type = new int[MAT_A_COL]; // 모선 타입
        public static double[,] LINEDATA = new double[MAT_B_COL, MAT_B_LOW];  // 선로 데이터 행렬
        public static double[,] YBUS_G = new double[MAT_B_COL, MAT_B_COL]; //선로 어드미턴스  실수
        public static double[,] YBUS_B = new double[MAT_B_COL, MAT_B_COL]; // 선로 어드미턴스 허수부
        public static double[] start = new double[MAT_B_COL]; //선로 시작점
        public static double[] end = new double[MAT_B_COL]; //선로 끝점
        public static double[] R = new double[MAT_B_COL]; //선로 저항
        public static double[] X = new double[MAT_B_COL];//선로 임피던스
        public static double[] B = new double[MAT_B_COL]; //선로 대지어드미턴스
        public static double[] y_real = new double[MAT_B_COL]; //실수
        public static double[] y_imag = new double[MAT_B_COL]; //복소수 j값            
        public static int gen = 0; // 발전모선 개수
        public static int load = 0; // 부하모선 개수
        public static int slack_no = 0; // 슬랙모선 번호
        public static int[] gen_no = new int[MAT_A_COL];//발전모선 번호
        public static int[] load_no = new int[MAT_A_COL];//부하모선 번호
        public static double[] dS = new double[MAT_A_COL]; //△p,q 행렬화
        public static double[,] J11 = new double[MAT_A_COL, MAT_A_COL]; //J11
        public static double[,] J21 = new double[MAT_A_COL, MAT_A_COL]; //J21
        public static double[,] J12 = new double[MAT_A_COL, MAT_A_COL]; //J12
        public static double[,] J22 = new double[MAT_A_COL, MAT_A_COL]; //J22
        public static double[,] JAC0 = new double[MAT_A_COL, MAT_A_COL]; //자코비안 행렬
        public static double[,] JACO_inverse = new double[MAT_A_COL, MAT_A_COL];// 자코비안 역행렬 
        public static double[,] A = new double[MAT_A_COL, 2 * MAT_A_COL]; //역행렬을 만들기 위한 기본행렬
        public static double[] dx = new double[MAT_A_COL]; //△V,위상각 행렬화

        static void Main(string[] args)
        {
            Console.Write("데이터를 골라주세요 : ");
            string filePath = Console.ReadLine();


            using (FileStream fsIn = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fsIn)) // 읽어온 파일을 스트림 리더로 받는다        
                {
                    while (sr.Peek() > -1)
                    {
                        string temp_str = (sr.ReadLine().ToString()).Trim().Replace("\t", " ");  // 줄 단위로 읽는다 _ Trim (양 옆 공백 제거) _ 탭 문자를 기본 공백으로 대체
                        string input = ""; // 진짜 체크할 문자열 초기화                                          

                        if (temp_str.Equals("999999"))     // 그 줄이 999999라면 다음 반복
                            continue;
                        if (temp_str.Equals(""))           // 그 줄이 아무것도 없다면 다음 반복
                            continue;
                        foreach (string str in temp_str.Split(' ')) // temp_str을 공백문자를 기준으로 잘라 반복
                            if (str != " " && str != "")    // 만약 str이 공백이 아니고 아무것도 없지 않다면
                                input = input + str + "\t"; // input에 str과 탭을 붙힌다.

                        string[] token = input.Split('\t'); // 탭 단위로 토큰 끊기
                        if (token.Length == MAT_A_LOW + 1)      // 토큰의 갯수가 9개라면
                        {
                            for (j = 0; j < token.Length - 1; j++)  // 각 토큰을 x1의 배열에 넣는다.
                                x1[j] = Convert.ToDouble(token[j].ToString());
                            for (j = 0; j < 9; j++)     // 배열을 행렬 A의 각 1줄에 넣는다.
                                BUSDATA[Bn, j] = x1[j];
                            Bn++;

                        }
                        else if (token.Length == MAT_B_LOW + 1) // 토큰의 갯수가 6개라면
                        {
                            for (j = 0; j < token.Length - 1; j++)
                                x2[j] = Convert.ToDouble(token[j].ToString());
                            for (j = 0; j < 6; j++)
                                LINEDATA[Ln, j] = x2[j];
                            Ln++;
                        }
                    }
                }
            }

            Console.WriteLine("\n버스\t타입\t전압\t위상\tPl\tQl\tPg\tQg\tShunt");
            for (i = 0; i < Bn; i++)
            {
                for (k = 0; k < MAT_A_LOW; k++)
                    Console.Write("{0}\t", BUSDATA[i, k]);
                Console.WriteLine();
            }
            Console.WriteLine("\nStart\tEnd\tR\tL\tB\t탭비");
            for (i = 0; i < Ln; i++)
            {
                for (k = 0; k < MAT_B_LOW; k++)
                    Console.Write("{0}\t", LINEDATA[i, k]);
                Console.WriteLine();
            }

            for (i = 0; i < Ln; i++)
            {
                start[i] = LINEDATA[i, 0];
                end[i] = LINEDATA[i, 1];
                R[i] = LINEDATA[i, 2];
                X[i] = LINEDATA[i, 3];
                B[i] = LINEDATA[i, 4];
            }
            for (i = 0; i < Bn; i++)
            {
                BUS[i] = Convert.ToInt16(BUSDATA[i, 0]);
                type[i] = Convert.ToInt16(BUSDATA[i, 1]);
                Vp[i] = BUSDATA[i, 2];
                Theta[i] = BUSDATA[i, 3];
                Pl[i] = BUSDATA[i, 4] / 100; // pu값으로 바꾸기 위한 /100
                Ql[i] = BUSDATA[i, 5] / 100;
                Pg[i] = BUSDATA[i, 6] / 100;
                Qg[i] = BUSDATA[i, 7] / 100;
            }


            for (i = 0; i < Ln; i++)
            {
                y_real[i] = R[i] / (R[i] * R[i] + X[i] * X[i]); //실수부 식
                y_imag[i] = -X[i] / (R[i] * R[i] + X[i] * X[i]); //허수부 식
            }

            for (i = 0; i < Bn; i++)
            {
                for (j = 0; j < Bn; j++)
                {
                    YBUS_G[i, j] = 0;
                    YBUS_B[i, j] = 0;
                }
            }
            for (i = 0; i < Bn; i++)
            {
                for (j = 0; j < Bn; j++)
                {
                    if (i == j)  // 대각요소k
                    {
                        for (k = 0; k < Ln; k++) // YBUS데이터 행렬 개수
                        {
                            if ((start[k] == i + 1) || (end[k] == i + 1))
                            {
                                YBUS_G[i, i] = YBUS_G[i, i] + y_real[k];
                                YBUS_B[i, i] = YBUS_B[i, i] + y_imag[k] + B[k];
                            }
                        }
                    }
                    else   // 비대각 요소
                    {
                        for (k = 0; k < Ln; k++)
                        {
                            if ((start[k] == i + 1) && (end[k] == j + 1))
                            {
                                YBUS_G[i, j] = YBUS_G[i, j] - y_real[k];
                                YBUS_B[i, j] = YBUS_B[i, j] - y_imag[k];
                                YBUS_G[j, i] = YBUS_G[i, j];
                                YBUS_B[j, i] = YBUS_B[i, j];
                            }
                        }
                    }
                }
            }
            Console.WriteLine("\nYBUS행렬");
            for (i = 0; i < Bn; i++)
            {
                for (j = 0; j < Bn; j++)
                {
                    if (YBUS_B[i, j] < 0)
                    {
                        Console.Write("{0:F4}  {1:F4}j\t", YBUS_G[i, j], YBUS_B[i, j]);
                    }
                    else
                    {
                        Console.Write("{0:F4} + {1:F4}j\t", YBUS_G[i, j], YBUS_B[i, j]);
                    }
                }
                Console.WriteLine();
                Console.WriteLine("\n");
            }

            for (i = 0; i < Bn; i++)
            {
                if (type[i] == 1)
                {
                    slack_no = BUS[i];
                }
                else if (type[i] == 2)
                {
                    load_no[load] = BUS[i];
                    load++;
                }
                else if (type[i] == 3)
                {
                    gen_no[gen] = BUS[i];
                    gen++;
                }
            }
            int jaco_num = (2 * (Bn - 1)) - gen;

            while (true)
            {
                for (i = 0; i < Bn; i++)
                {
                    P[i] = 0.0; //  P와 Q는 바뀌기 때문에 매번 초기화 한다.
                    Q[i] = 0.0;
                }
                for (i = 0; i < Bn; i++)//P,Q계산
                {
                    for (j = 0; j < Bn; j++)
                    {
                        P[i] += Math.Abs(Vp[i]) * Math.Abs(Vp[j]) * ((YBUS_G[i, j] * Math.Cos(Theta[i] - Theta[j])) + (YBUS_B[i, j] * Math.Sin(Theta[i] - Theta[j])));
                        Q[i] += Math.Abs(Vp[i]) * Math.Abs(Vp[j]) * ((YBUS_G[i, j] * Math.Sin(Theta[i] - Theta[j])) - (YBUS_B[i, j] * Math.Cos(Theta[i] - Theta[j])));
                    }
                }
                for (i = 0; i < Bn; i++)
                {
                    Console.Write("P{0} = {1:F7}\nQ{2} = {3:F7}\n", i + 1, P[i], i + 1, Q[i]);
                    Console.WriteLine();
                }

                for (i = 0; i < Bn; i++)
                {
                    Px[i] = Pg[i] - Pl[i];
                    Qx[i] = Qg[i] - Ql[i];
                }
                for (i = 0; i < Bn; i++)//dP,dQ계산
                {
                    dP[i] = Px[i] - P[i];
                    dQ[i] = Qx[i] - Q[i];
                }
                for (i = 0; i < Bn; i++)
                {
                    Console.Write("dP{0} = {1:F7}\ndQ{2} = {3:F7}\n", i + 1, dP[i], i + 1, dQ[i]);
                    Console.WriteLine();
                }

                for (i = 0; i < Bn - 1; i++)
                {
                    if (i < slack_no - 1)
                    {
                        dS[i] = dP[i];
                    }
                    else
                    {
                        dS[i] = dP[i + 1];
                    }
                }

                for (i = Bn - 1; i < jaco_num; i++)
                {
                    dS[i] = dQ[load_no[i - (Bn - 1)] - 1];
                }

                for (i = 0; i < Bn; i++)
                {
                    for (j = 0; j < Bn; j++)
                    {
                        if (i == j)
                        {
                            J11[i, j] = (-1 * Q[i]) - ((Vp[i] * Vp[i]) * YBUS_B[i, j]);
                            J21[i, j] = P[i] - ((Vp[i] * Vp[i]) * YBUS_G[i, j]);
                            J12[i, j] = (P[i] / Vp[i]) + (Vp[i] * YBUS_G[i, j]);
                            J22[i, j] = (Q[i] / Vp[i]) - (Vp[i] * YBUS_B[i, j]);
                        }
                        else
                        {
                            J11[i, j] = Vp[i] * Vp[j] * (YBUS_G[i, j] * Math.Sin(Theta[i] - Theta[j]) - YBUS_B[i, j] * Math.Cos(Theta[i] - Theta[j]));
                            J21[i, j] = -Vp[i] * Vp[j] * (YBUS_G[i, j] * Math.Cos(Theta[i] - Theta[j]) - YBUS_B[i, j] * Math.Sin(Theta[i] - Theta[j]));
                            J12[i, j] = Vp[i] * ((YBUS_G[i, j] * Math.Cos(Theta[i] - Theta[j])) + (YBUS_B[i, j] * Math.Sin(Theta[i] - Theta[j])));
                            J22[i, j] = Vp[i] * ((YBUS_G[i, j] * Math.Sin(Theta[i] - Theta[j])) - (YBUS_B[i, j] * Math.Cos(Theta[i] - Theta[j])));

                        }
                    }
                }
                //J11
                for (i = 0; i < Bn - 1; i++)
                {
                    for (j = 0; j < Bn - 1; j++)
                    {
                        if (i < slack_no - 1)
                        {
                            if (j < slack_no - 1)
                            {
                                JAC0[i, j] = J11[i, j];
                            }
                            else
                            {
                                JAC0[i, j] = J11[i, j + 1];
                            }
                        }
                        else
                        {
                            if (j < slack_no - 1)
                            {
                                JAC0[i, j] = J11[i + 1, j];
                            }
                            else
                            {
                                JAC0[i, j] = J11[i + 1, j + 1];
                            }
                        }
                    }
                }
                //J12
                for (i = 0; i < Bn - 1; i++)
                {
                    for (j = 0; j < load; j++)
                    {
                        if (i < slack_no - 1)
                        {
                            JAC0[i, Bn - 1 + j] = J12[i, load_no[j] - 1];
                        }
                        else
                        {
                            JAC0[i, Bn - 1 + j] = J12[i + 1, load_no[j] - 1];
                        }
                    }
                }
                //J21
                for (i = 0; i < load; i++)
                {
                    for (j = 0; j < Bn - 1; j++)
                    {
                        if (i < slack_no - 1)
                        {
                            JAC0[Bn - 1 + i, j] = J21[load_no[i] - 1, j];
                        }
                        else
                        {
                            JAC0[Bn - 1 + i, j] = J21[load_no[i] - 1, j + 1];
                        }
                    }
                }
                //J22
                for (i = 0; i < load; i++)
                {
                    for (j = 0; j < load; j++)
                    {
                        JAC0[Bn - 1 + i, Bn - 1 + j] = J22[load_no[i] - 1, load_no[j] - 1];
                    }
                }
                Console.WriteLine("\n\n자코비안 행렬");
                for (i = 0; i < jaco_num; i++)
                {
                    for (j = 0; j < jaco_num; j++)
                    {
                        Console.Write("{0:F3}\t\t", JAC0[i, j]);
                    }
                    Console.WriteLine();
                }

                for (i = 0; i < jaco_num; i++)
                {
                    for (j = jaco_num; j < (2 * jaco_num); j++)
                    {
                        if (i == j - jaco_num)
                            A[i, j] = 1;
                        else
                            A[i, j] = 0;
                    }
                }

                for (i = 0; i < jaco_num; i++)
                {
                    for (j = 0; j < jaco_num; j++)
                    {
                        A[i, j] = JAC0[i, j];
                    }
                }

                for (i = 0; i < jaco_num; i++)
                {
                    t = A[i, i];
                    for (j = i; j < 2 * jaco_num; j++)
                        A[i, j] = A[i, j] / t;
                    for (j = 0; j < jaco_num; j++)
                    {
                        if (i != j)
                        {
                            t = A[j, i];
                            for (k = 0; k < 2 * jaco_num; k++)
                                A[j, k] = A[j, k] - t * A[i, k];
                        }
                    }
                }
                Console.WriteLine("\n\n\n");
                for (i = 0; i < jaco_num; i++)
                {
                    for (j = 0; j < jaco_num; j++)
                    {
                        JACO_inverse[i, j] = A[i, j + jaco_num];
                        Console.Write("{0:F3}\t", JACO_inverse[i, j]);
                    }
                    Console.WriteLine();
                }

                for (i = 0; i < jaco_num; i++)
                {
                    dx[i] = 0;
                }
                for (i = 0; i < jaco_num; i++)
                {
                    for (j = 0; j < jaco_num; j++)
                    {
                        dx[i] += JACO_inverse[i, j] * dS[j];
                    }
                }
                Console.WriteLine("\n\n\n");
                for (i = 0; i < jaco_num; i++)
                {
                    Console.Write("{0:F7}\n", dx[i]);
                    Console.WriteLine();
                }
                for (i = 0; i < Bn - 1; i++)
                {
                    if (i < slack_no - 1)
                    {
                        Theta[i] += dx[i];
                    }
                    else
                    {
                        Theta[i + 1] += dx[i];
                    }
                }
                for (i = 0; i < load; i++)
                {
                    Vp[load_no[i] - 1] += dx[Bn - 1 + i];
                }
                int check = jaco_num;
                for (i = jaco_num; i > 0; i--)
                {
                    if (Math.Abs(dS[i]) < 0.000001)
                        check--;
                }
                if (check == 0)
                    break;
            }
        }
    }
}