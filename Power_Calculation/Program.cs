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
            Console.Write("데이터를 골라주세요 : ");
            string filePath = Console.ReadLine();

            int n = 0, s = 0, i = 0, k = 0, j = 0;
            double t = 0;
            /*int gen_num=0;
            int load_num=0;
            int jaco_num = 2 * (n - 1) - gen;
        */
            double[] x1 = new double[MAT_A_LOW]; // 행렬A을 한 줄 담기 위한 임시 배열
            double[] x2 = new double[MAT_B_LOW]; // 행렬B를 한 줄 담기 위한 임시 배열  
            double[,] BUSDATA = new double[MAT_A_COL, MAT_A_LOW];  // BUS 데이터 행렬
            double[,] LINEDATA = new double[MAT_B_COL, MAT_B_LOW];  // 선로 데이터 행렬
            double[,] YBUS_G = new double[MAT_B_COL, MAT_B_COL]; //선로 어드미턴스  실수
            double[,] YBUS_B = new double[MAT_B_COL, MAT_B_COL]; // 선로 어드미턴스 허수부
            double[,] start = new double[MAT_B_COL, 1]; //선로 시작점
            double[,] end = new double[MAT_B_COL, 1]; //선로 끝점
            double[,] R = new double[MAT_B_COL, 1]; //선로 저항
            double[,] X = new double[MAT_B_COL, 1];//선로 임피던스
            double[,] B = new double[MAT_B_COL, 1]; //선로 대지어드미턴스
            double[,] Y_real = new double[MAT_B_COL, 1]; //실수
            double[,] Y_imag = new double[MAT_B_COL, 1]; //복소수 j값
            double[,] P = new double[MAT_A_COL, 1]; //유효전력
            double[,] Q = new double[MAT_A_COL, 1]; // 무효전력
            double[,] Px = new double[MAT_A_COL, 1]; //유효전력에서 발전량-부하량(고정)
            double[,] Qx = new double[MAT_A_COL, 1]; // 무효전력에서 발전량-부하량(고정)
            double[,] Pg = new double[MAT_A_COL, 1]; // 유효전력 발전량
            double[,] Qg = new double[MAT_A_COL, 1]; // 무효전력 발전량
            double[,] Pl = new double[MAT_A_COL, 1]; // 유효전력 부하량
            double[,] Ql = new double[MAT_A_COL, 1]; // 무효전력 부하량
            double[,] dP = new double[MAT_A_COL, 1]; // △p값
            double[,] dQ = new double[MAT_A_COL, 1]; // △q값
            double[,] Vp = new double[MAT_A_COL, 1]; // V값
            double[,] Theta = new double[MAT_A_COL, 1]; // 위상각(rad)
            int[,] BUS = new int[MAT_A_COL, 1]; // 모선번호
            int[,] type = new int[MAT_A_COL, 1]; // 모선 타입
            int gen = 0; // 발전모선 개수
            int load = 0; // 부하모선 개수
            int slack_no = 0; // 슬랙모선 번호
            int[,] gen_no = new int[MAT_A_COL, 1];
            int[,] load_no = new int[MAT_A_COL, 1];
            double[,] dS = new double[MAT_A_COL, 1];
            double[,] J11 = new double[MAT_A_COL, MAT_A_COL];
            double[,] J21 = new double[MAT_A_COL, MAT_A_COL];
            double[,] J12 = new double[MAT_A_COL, MAT_A_COL];
            double[,] J22 = new double[MAT_A_COL, MAT_A_COL];
            double[,] JAC0 = new double[MAT_A_COL, MAT_A_COL];
            double[,] JACO_inverse = new double[MAT_A_COL, MAT_A_COL];
            double[,] A = new double[MAT_A_COL, MAT_A_COL];
            double[,] dx = new double[MAT_A_COL, 1];




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
                                BUSDATA[n, j] = x1[j];
                            n++;

                        }
                        else if (token.Length == MAT_B_LOW + 1) // 토큰의 갯수가 6개라면
                        {
                            for (j = 0; j < token.Length - 1; j++)
                                x2[j] = Convert.ToDouble(token[j].ToString());
                            for (j = 0; j < 6; j++)
                                LINEDATA[s, j] = x2[j];
                            s++;
                        }
                    }
                }
            }

            Console.WriteLine("\n버스\t타입\t전압\t위상\tPl\tQl\tPg\tQg\tShunt");
            for (i = 0; i < n; i++)
            {
                for (k = 0; k < MAT_A_LOW; k++)
                    Console.Write("{0}\t", BUSDATA[i, k]);
                Console.WriteLine();
            }
            Console.WriteLine("\nStart\tEnd\tR\tL\tB\t탭비");
            for (i = 0; i < s; i++)
            {
                for (k = 0; k < MAT_B_LOW; k++)
                    Console.Write("{0}\t", LINEDATA[i, k]);
                Console.WriteLine();
            }
            for (i = 0; i < s; i++)
            {
                start[i, 0] = LINEDATA[i, 0];
                end[i, 0] = LINEDATA[i, 1];
                R[i, 0] = LINEDATA[i, 2];
                X[i, 0] = LINEDATA[i, 3];
                B[i, 0] = LINEDATA[i, 4];
            }
            for (i = 0; i < n; i++)
            {
                BUS[i, 0] = Convert.ToInt16(BUSDATA[i, 0]);
                type[i, 0] = Convert.ToInt16(BUSDATA[i, 1]);
                Vp[i, 0] = BUSDATA[i, 2];
                Theta[i, 0] = BUSDATA[i, 3];
                Pl[i, 0] = BUSDATA[i, 4] / 100; // pu값으로 바꾸기 위한 /100
                Ql[i, 0] = BUSDATA[i, 5] / 100;
                Pg[i, 0] = BUSDATA[i, 6] / 100;
                Qg[i, 0] = BUSDATA[i, 7] / 100;
            }
            for (i = 0; i < s; i++)
            {
                Y_real[i, 0] = R[i, 0] / (R[i, 0] * R[i, 0] + X[i, 0] * X[i, 0]); //실수부 식
                Y_imag[i, 0] = -X[i, 0] / (R[i, 0] * R[i, 0] + X[i, 0] * X[i, 0]); //허수부 식
            }

            for (i = 0; i < n; i++)
            {
                for (k = 0; k < n; k++)
                {
                    YBUS_G[i, k] = 0;
                    YBUS_B[i, k] = 0;
                }
            }
            for (i = 0; i < n; i++)
            {
                for (k = 0; k < n; k++)
                {
                    if (i == k)  // 대각요소k
                    {
                        for (int N = 0; N < s; N++) // YBUS데이터 행렬 개수
                        {
                            if ((start[N, 0] == i + 1) || (end[N, 0] == i + 1))
                            {
                                YBUS_G[i, i] = YBUS_G[i, i] + Y_real[N, 0];
                                YBUS_B[i, i] = YBUS_B[i, i] + Y_imag[N, 0] + B[N, 0];
                            }
                        }
                    }
                    else   // 비대각 요소
                    {
                        for (int N = 0; N < s; N++)
                        {
                            if ((start[N, 0] == i + 1) && (end[N, 0] == k + 1))
                            {
                                YBUS_G[i, k] = YBUS_G[i, k] - Y_real[N, 0];
                                YBUS_B[i, k] = YBUS_B[i, k] - Y_imag[N, 0];
                                YBUS_G[k, i] = YBUS_G[i, k];
                                YBUS_B[k, i] = YBUS_B[i, k];
                            }
                        }
                    }
                }
            }
            Console.WriteLine("\nYBUS행렬");
            for (i = 0; i < n; i++)
            {
                for (k = 0; k < n; k++)
                {
                    if (YBUS_B[i, k] < 0)
                    {
                        Console.Write("{0:F4}  {1:F4}j\t", YBUS_G[i, k], YBUS_B[i, k]);
                    }
                    else
                    {
                        Console.Write("{0:F4} + {1:F4}j\t", YBUS_G[i, k], YBUS_B[i, k]);
                    }
                }
                Console.WriteLine();
                Console.WriteLine("\n");
            }
            for (i = 0; i < n; i++)
            {
                P[i, 0] = 0.0; //  P와 Q는 바뀌기 때문에 매번 초기화 한다.
                Q[i, 0] = 0.0;
            }
            for (i = 0; i < n; i++)//P,Q계산
            {
                for (j = 0; j < n; j++)
                {
                    P[i, 0] += Math.Abs(Vp[i, 0]) * Math.Abs(Vp[j, 0]) * ((YBUS_G[i, j] * Math.Cos(Theta[i, 0] - Theta[j, 0])) + (YBUS_B[i, j] * Math.Sin(Theta[i, 0] - Theta[j, 0])));
                    Q[i, 0] += Math.Abs(Vp[i, 0]) * Math.Abs(Vp[j, 0]) * ((YBUS_G[i, j] * Math.Sin(Theta[i, 0] - Theta[j, 0])) - (YBUS_B[i, j] * Math.Cos(Theta[i, 0] - Theta[j, 0])));
                }
            }
            for (i = 0; i < n; i++)
            {
                Console.Write("P{0} = {1}\nQ{2} = {3}\n", i + 1, P[i, 0], i + 1, Q[i, 0]);
                Console.WriteLine();
            }
            for (i = 0; i < n; i++)
            {
                Px[i, 0] = Pg[i, 0] - Pl[i, 0];
                Qx[i, 0] = Qg[i, 0] - Ql[i, 0];
            }
            for (i = 0; i < n; i++)//dP,dQ계산
            {
                dP[i, 0] = Px[i, 0] - P[i, 0];
                dQ[i, 0] = Qx[i, 0] - Q[i, 0];
            }
            for (i = 0; i < n; i++)
            {
                Console.Write("dP{0} = {1}\ndQ{2} = {3}\n", i + 1, dP[i, 0], i + 1, dQ[i, 0]);
                Console.WriteLine();
            }
            //자코비안 행렬 시작
            for (i = 0; i < n; i++)
            {
                if (type[i, 0] == 1)
                {
                    slack_no = BUS[i, 0];
                }
                else if (type[i, 0] == 2)
                {
                    load_no[load, 0] = BUS[i, 0];
                    load++;
                }
                else if (type[i, 0] == 3)
                {
                    gen_no[gen, 0] = BUS[i, 0];
                    gen++;
                }
            }
            for (i = 0; i < n - 1; i++)
            {
                if (i < slack_no - 1)
                {
                    dS[i, 0] = dP[i, 0];
                }
                else
                {
                    dS[i, 0] = dP[i + 1, 0];
                }
            }
            int jaco_num = (2 * (n - 1)) - gen;
            //double[,] JAC0 = new double[MAT_A_COL, MAT_A_LOW];
            for (i = n - 1; i < jaco_num; i++)
            {
                dS[i, 0] = dQ[load_no[i - (n - 1), 0] - 1, 0];
            }

            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        J11[i, j] = (-1 * Q[i, 0]) - ((Vp[i, 0] * Vp[i, 0]) * YBUS_B[i, j]);
                        J21[i, j] = P[i, 0] - ((Vp[i, 0] * Vp[i, 0]) * YBUS_G[i, j]);
                        J12[i, j] = (P[i, 0] / Vp[i, 0]) + (Vp[i, 0] * YBUS_G[i, j]);
                        J22[i, j] = (Q[i, 0] / Vp[i, 0]) - (Vp[i, 0] * YBUS_B[i, j]);
                    }
                    else
                    {
                        J11[i, j] = Vp[i, 0] * Vp[j, 0] * (YBUS_G[i, j] * Math.Sin(Theta[i, 0] - Theta[j, 0]) - YBUS_B[i, j] * Math.Cos(Theta[i, 0] - Theta[j, 0]));
                        J21[i, j] = -Vp[i, 0] * Vp[j, 0] * (YBUS_G[i, j] * Math.Cos(Theta[i, 0] - Theta[j, 0]) - YBUS_B[i, j] * Math.Sin(Theta[i, 0] - Theta[j, 0]));
                        J12[i, j] = Vp[i, 0] * ((YBUS_G[i, j] * Math.Cos(Theta[i, 0] - Theta[j, 0])) + (YBUS_B[i, j] * Math.Sin(Theta[i, 0] - Theta[j, 0])));
                        J22[i, j] = Vp[i, 0] * ((YBUS_G[i, j] * Math.Sin(Theta[i, 0] - Theta[j, 0])) - (YBUS_B[i, j] * Math.Cos(Theta[i, 0] - Theta[j, 0])));

                    }
                }
            }
            //J11
            for (i = 0; i < n - 1; i++)
            {
                for (j = 0; j < n - 1; j++)
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
            for (i = 0; i < n - 1; i++)
            {
                for (j = 0; j < load; j++)
                {
                    if (i < slack_no - 1)
                    {
                        JAC0[i, n - 1 + j] = J12[i, load_no[j, 0] - 1];
                    }
                    else
                    {
                        JAC0[i, n - 1 + j] = J12[i + 1, load_no[j, 0] - 1];
                    }
                }
            }
            //J21
            for (i = 0; i < load; i++)
            {
                for (j = 0; j < n - 1; j++)
                {
                    if (i < slack_no - 1)
                    {
                        JAC0[n - 1 + i, j] = J21[load_no[i, 0] - 1, j];
                    }
                    else
                    {
                        JAC0[n - 1 + i, j] = J21[load_no[i, 0] - 1, j + 1];
                    }
                }
            }
            //J22
            for (i = 0; i < load; i++)
            {
                for (j = 0; j < load; j++)
                {
                    JAC0[n - 1 + i, n - 1 + j] = J22[load_no[i, 0] - 1, load_no[j, 0] - 1];
                }
            }
            for (i = 0; i < jaco_num; i++)
            {
                for (j = 0; j < jaco_num; j++)
                {
                    Console.Write("{0:F3}\t", JAC0[i, j]);
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
                dx[i, 0] = 0;
            }
            for (i = 0; i < jaco_num; i++)
            {
                for (j = 0; j < 1; j++)
                {
                    for (k = 0; k < jaco_num; k++)
                    {
                        dx[i, j] += JACO_inverse[i, k] * dS[k, j];
                    }
                }

            }
            for (i = 0; i < jaco_num; i++)
            {
                Console.Write("{0}\n", dx[i, 0]);
                Console.WriteLine();
            }
        }
    }
}
