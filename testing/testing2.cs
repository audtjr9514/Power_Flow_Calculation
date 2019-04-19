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
            string filePath = "3.Dat";
            int n = 0, i = 0, col = 0;
            double l;
            double b = 0, t = 0, a = 0, g = 0, c = 0, d = 0, e = 0, f = 0;
            double[,] BUS = new double[n, 9];
            using (FileStream fsIn = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fsIn))
                {
                    for (n = 0; n < sr.Peek(); n++)
                    {
                        while (sr.Peek() > -1)
                        {
                            string input = sr.ReadLine();
                            if (input.Equals("999999"))
                            {
                                continue;

                            }
                            if (input.Equals(""))
                            {
                                Console.ReadLine();
                            }

                            string[] token = input.Split('\t');
                            if (token.Length == 9)
                            {
                                l = Convert.ToDouble(token[0].ToString());
                                b = Convert.ToDouble(token[1].ToString());
                                t = Convert.ToDouble(token[2].ToString());
                                a = Convert.ToDouble(token[3].ToString());
                                g = Convert.ToDouble(token[4].ToString());
                                c = Convert.ToDouble(token[5].ToString());
                                d = Convert.ToDouble(token[6].ToString());
                                e = Convert.ToDouble(token[7].ToString());
                                f = Convert.ToDouble(token[8].ToString());
                                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", l, b, t, a, g, c, d, e, f);
                            }

                            if (token.Length == 6)
                            {
                                l = Convert.ToDouble(token[0].ToString());
                                b = Convert.ToDouble(token[1].ToString());
                                t = Convert.ToDouble(token[2].ToString());
                                a = Convert.ToDouble(token[3].ToString());
                                g = Convert.ToDouble(token[4].ToString());
                                c = Convert.ToDouble(token[5].ToString());
                                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", l, b, t, a, g, c);
                            }
                        }
                    }
                }
            }
            for (n = 0; n < 3; n++)
            {
                for (i = 0; i < 9; i++)
                {
                    Console.Write("{0}\t", BUS[n, i]);
                }
                Console.WriteLine();
            }
        }
    }
}