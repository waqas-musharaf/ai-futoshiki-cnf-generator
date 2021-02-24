using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futoshiki_CNF
{
    class Futoshiki_CNF
    {
        static readonly int N = 4; // To change
        // static int N = ReadInput(); // Buggy
        static readonly int N2 = N * N;
        static readonly int VARS = N * N * N;
        static int clauses = 0;
        static List<String> DIMACS_CNF = new List<String>();

        static readonly int[] solution = { -1, -2, 3, -4, -5, -6, -7, 8, 9, -10, -11, -12, -13, 14, -15, -16, 17, -18, -19, -20, -21, -22, 23, -24, -25, 26, -27, -28, -29, -30, -31, 32, -33, -34, -35, 36, -37, 38, -39, -40, -41, -42, 43, -44, 45, -46, -47, -48, -49, 50, -51, -52, 53, -54, -55, -56, -57, -58, -59, 60, -61, -62, 63, -64 };
        static readonly int[] solutionNoIneq = { -1, 2, -3, -4, 5, -6, -7, -8, -9, -10, -11, 12, -13, -14, 15, -16, 17, -18, -19, -20, -21, 22, -23, -24, -25, -26, 27, -28, -29, -30, -31, 32, -33, -34, 35, -36, -37, -38, -39, 40, -41, 42, -43, -44, 45, -46, -47, -48, -49, -50, -51, 52, -53, -54, 55, -56, 57, -58, -59, -60, -61, 62, -63, -64 };


        static void Main(string[] args)
        {
            AddFacts();
            AtLeastOneDigitInCell();
            EachDigitAtMostOnceInRow();
            EachDigitAtMostOnceInColumn();
            EachDigitSatisfiesInequalities();
            Print_DIMACS_CNF_format();

            PrintSATSolutionBoard(solution);
            // PrintSATSolutionBoard(solutionNoIneq);

            Console.ReadLine();
        }

        private static int ReadInput()
        {
        start: Console.Write("Please enter a positive integer value for 'N': ");
            String temp = Console.ReadLine();
            if (int.TryParse(temp, out int val) && val > 0)
            {
                return val;
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
                goto start;
            }
        }

        private static int ToVariable(int digit, int row, int column)
        {
            return (N2 * (digit - 1) + N * (row - 1) + (column - 1) + 1);
        }

        private static void AddFacts()
        {
            // Facts
            DIMACS_CNF.Add("c Pre-assigned entries");
            // Update the number of facts according to the number of added DIMACS CNF clauses
            int facts = 1;
            DIMACS_CNF.Add(ToVariable(2, 1, 1) + " 0");
            // DIMACS_CNF.Add(ToVariable(Z, X, Y) + " 0");
            clauses += facts;
        }

        private static void AtLeastOneDigitInCell()
        {
            // Every cell contains at least one digit
            DIMACS_CNF.Add("c Every cell contains at least one digit:");
            String str;
            for (int row = 1; row <= N; row++)
            {
                for (int column = 1; column <= N; column++)
                {
                    str = "";
                    for (int digit = 1; digit <= N; digit++)
                    {
                        str += ToVariable(digit, row, column) + " ";
                    }
                    DIMACS_CNF.Add(str + "0");
                    clauses++;
                }
            }
        }

        private static void EachDigitAtMostOnceInRow()
        {
            // Each digit appears at most once in each row
            DIMACS_CNF.Add("c Each digit appears at most once in each row:");
            for (int digit = 1; digit <= N; digit++)
            {
                for (int row = 1; row < N; row++)
                {
                    for (int columnLow = 1; columnLow <= N - 1; columnLow++)
                    {
                        for (int columnHigh = columnLow + 1; columnHigh <= N; columnHigh++)
                        {
                            DIMACS_CNF.Add("-" + ToVariable(digit, row, columnLow) + " -" + ToVariable(digit, row, columnHigh) + " 0");
                            clauses++;
                        }
                    }
                }
            }
        }

        private static void EachDigitAtMostOnceInColumn()
        {
            // Each digit appears at most once in each column
            DIMACS_CNF.Add("c Each number appears at most once in each column:");
            for (int digit = 1; digit <= N; digit++)
            {
                for (int column = 1; column <= N; column++)
                {
                    for (int rowLow = 1; rowLow <= N - 1; rowLow++)
                    {
                        for (int rowHigh = rowLow + 1; rowHigh <= N; rowHigh++)
                        {
                            DIMACS_CNF.Add("-" + ToVariable(digit, rowLow, column) + " -" + ToVariable(digit, rowHigh, column) + " 0");
                            clauses++;
                        }
                    }
                }
            }
        }

        private static void EachDigitSatisfiesInequalities()
        {
            // Each digit satisfies the specified inequalities
            // TODO: Change all instances of '4' to 'N'.  

            DIMACS_CNF.Add("c Each digit satisfies inequalities:");

            var ineqList = new List<Inequality>
            {
                new Inequality(2, 2, 1, 2),
                new Inequality(3, 2, 2, 2),
                new Inequality(4, 2, 3, 2),
                new Inequality(3, 1, 4, 1),
                new Inequality(2, 4, 1, 4),
                new Inequality(1, 4, 2, 4) // UNSAT Inequality
            };
            
            foreach (var ineq in ineqList)
            {               
                for (var i = 1; i < 4; i++)
                {
                    for (var j = i; j <= 4; j++)
                    {
                        DIMACS_CNF.Add("-" + ToVariable(i, ineq.isGreaterRow, ineq.isGreaterColumn) + " -" + ToVariable(j, ineq.isLessRow, ineq.isLessColumn) + " 0");
                    }
                }
            }
        }
        
        private static void Print_DIMACS_CNF_format()
        {
            // Print DIMACS CNF format 
            Console.WriteLine("==========================================");
            Console.WriteLine("===== Beginning of DIMACS CNF format =====");
            Console.WriteLine("==========================================");
            Console.WriteLine("c digit range [1..." + N + "]");
            Console.WriteLine("c row range: [1..." + N + "]");
            Console.WriteLine("c column range: [1..." + N + "]");
            Console.WriteLine("c board[digit][row][column]: variable");
            for (int digit = 1; digit <= N; digit++)
            {
                for (int row = 1; row <= N; row++)
                {
                    for (int column = 1; column <= N; column++)
                    {
                        Console.WriteLine("c board[" + digit + "][" + row + "][" + column + "]: " + ToVariable(digit, row, column));
                    }
                }
            }
            Console.WriteLine("c #vars: " + VARS);
            Console.WriteLine("c #clauses: " + clauses);
            Console.WriteLine("p cnf " + VARS + " " + clauses);
            for (int i = 0; i < DIMACS_CNF.Count(); i++)
            {
                Console.WriteLine(DIMACS_CNF.ElementAt(i));
            }
            Console.WriteLine("====================================");
            Console.WriteLine("===== End of DIMACS CNF format =====");
            Console.WriteLine("====================================");
            Console.WriteLine("");
        }

        private static void PrintSATSolutionBoard(int[] variables)
        {
            int digit;
            int tmp;
            int row;
            int column;

            int[,] tmpBoard = new int[N,N];
            for (row = 0; row < N; row++)
            {
                for (column = 0; column < N; column++)
                {
                    tmpBoard[row,column] = -1;
                }
            }
            for (int i = 0; i < variables.Count(); i++)
            {
                if (variables[i] > 0)
                {
                    digit = (variables[i] - 1) / N2;
                    tmp = (variables[i] - 1) % N2;
                    row = tmp / N;
                    column = tmp % N;
                    tmpBoard[row,column] = digit;
                }
            }

            Console.WriteLine("=======================");
            Console.WriteLine("===== Given board =====");
            Console.WriteLine("=======================");
            for (row = 0; row < N; row++)
            {
                Console.Write("   ");
                for (column = 0; column < N; column++)
                {
                    Console.Write(((tmpBoard[row,column]) >= 0 ? (tmpBoard[row,column] + 1) + "" : "-") + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("=======================");
        }
    }
}
