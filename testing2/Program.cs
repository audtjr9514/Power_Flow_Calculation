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
        static void Main(string[] args)
        {/*
            int i, j, k;
            double B;


            string line;
            double[,] matrix = new double[100, 100];
            double[,] A = new double[3, 6];
            double t;
            StreamReader file = new System.IO.StreamReader("5.Dat");//데이터 불러오기
            {
                int c = 0, r = 0;
                Console.WriteLine("\n버스\t타입\t전압\t위상\tP\tQ\tPg\tQg\tShunt");
                
                while ((line = file.ReadLine()) != null) 
                {
                    if (line.Equals(""))
                    {
                        continue;
                    }
                    if (line.Equals("999999"))
                    {
                        Console.WriteLine("\nStart\tEnd\tR\tL\tC\t탭비");
                        continue;
                    }
                    string[] token = line.Split('\t');
                    for (i = 0; i < token.Length; i++)
                    {
                        Console.Write("{0}\t", token[i]);
                    }
                    Console.WriteLine();
                }                                               
                
            }
        */
            string filePath = "C:\\Users\\myoun\\Desktop\\메추라기\\testing2\\3.DAT";
            int n = 0, i = 0, s = 0;
            double[] x1 = new double[9];
            double[] x2 = new double[6];
            double[,] BUS1 = new double[3, 9];
            double[,] BUS2 = new double[3, 6];
            using (FileStream fsIn = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fsIn))
                {
                    while (sr.Peek() > -1)
                    {
                        string input = sr.ReadLine();
                        if (input.Equals("999999"))
                            continue;
                        if (input.Equals(""))
                            continue;

                        string[] token = input.Split('\t');
                        if (token.Length == 9)
                        {
                            x1[0] = Convert.ToDouble(token[0].ToString());
                            x1[1] = Convert.ToDouble(token[1].ToString());
                            x1[2] = Convert.ToDouble(token[2].ToString());
                            x1[3] = Convert.ToDouble(token[3].ToString());
                            x1[4] = Convert.ToDouble(token[4].ToString());
                            x1[5] = Convert.ToDouble(token[5].ToString());
                            x1[6] = Convert.ToDouble(token[6].ToString());
                            x1[7] = Convert.ToDouble(token[7].ToString());
                            x1[8] = Convert.ToDouble(token[8].ToString());
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", x1[0], x1[1], x1[2], x1[3], x1[4], x1[5], x1[6], x1[7], x1[8]);
                            for (int j = 0; j < 9; j++)
                                BUS1[n, j] = x1[j];
                            n++;

                        }
                        else if (token.Length == 6)
                        {
                            x2[0] = Convert.ToDouble(token[0].ToString());
                            x2[1] = Convert.ToDouble(token[1].ToString());
                            x2[2] = Convert.ToDouble(token[2].ToString());
                            x2[3] = Convert.ToDouble(token[3].ToString());
                            x2[4] = Convert.ToDouble(token[4].ToString());
                            x2[5] = Convert.ToDouble(token[5].ToString());
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", x2[0], x2[1], x2[2], x2[3], x2[4], x2[5]);
                            for (int j = 0; j < 6; j++)
                                BUS2[s, j] = x2[j];
                            s++;
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            for (n = 0; n < 3; n++)
            {
                for (i = 0; i < 9; i++)
                    Console.Write("{0}\t", BUS1[n, i]);
                Console.WriteLine();
            }
            for (n = 0; n < 3; n++)
            {
                for (i = 0; i < 6; i++)
                    Console.Write("{0}\t", BUS2[n, i]);
                Console.WriteLine();
            }
        }
    }
}