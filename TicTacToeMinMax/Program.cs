using System.Runtime.ExceptionServices;

namespace TicTacToeMinMax
{
    enum Piece
    {
        Cross = -1,
        Empty = 0,
        Circle = 1
    }

    class TicTacToeBoard
    {
        public const int Rows = 3;
        public int numMoves;
        public Piece[,] board = new Piece[Rows, Rows];
        public Piece turn = Piece.Cross;
        public int numberOfBoardEvaluations;
        public void Print()
        {
            void PrintHeader()
            {
                Console.Write("  ");
                for (int j = 0; j < Rows; j++)
                    Console.Write($" {j} ");
                Console.WriteLine();
            }
            void PrintRow(int i)
            {
                Console.Write($"{(char)(i + 'a')} ");
                for (int j = 0; j < Rows; j++)
                {
                    Console.Write(board[i, j] == Piece.Cross ? " x " : (board[i, j] == Piece.Circle ? " O " : "   "));
                }
                Console.WriteLine();

            }
            PrintHeader();
            for (int i = 0; i < Rows; i++)
            {
                PrintRow(i);
            }
            Console.WriteLine();
        }
        public Piece Winner()
        {
            for (int i = 0; i < Rows; i++)
            {
                Piece first = board[i,0];
                if (first == Piece.Empty)
                    continue;
                bool hasWinner = true;
                for (int j = 1; j < Rows; j++)
                {
                    if (board[i, j] != first)
                    {
                        hasWinner = false;
                        break;
                    }
                }
                if (hasWinner)
                    return first;
            }
            for (int i = 0;i < Rows; i++)
            {
                Piece first = board[0, i];
                if (first == Piece.Empty)
                    continue;
                bool hasWinner = true;
                for (int j = 1;j < Rows; j++)
                {
                    if (board[j,i] != first)
                    {
                        hasWinner = false;
                        break;
                    }
                }
                if (hasWinner)
                    return first;
            }
            Piece firstDiag = board[0,0];
            bool hasDiagWinner = true;
            for (int i = 1; i < Rows; i++)
            {
                if (firstDiag == Piece.Empty || firstDiag != board[i,i])
                {
                    hasDiagWinner = false;
                    break;
                }
            }
            if (hasDiagWinner)
                return firstDiag;
            firstDiag = board[0, Rows-1];
            hasDiagWinner = true;
            for (int i = 1;i < Rows;i++)
            {
                if (firstDiag == Piece.Empty || firstDiag != board[i,Rows - i - 1])
                {
                    hasDiagWinner = false;
                    break;
                }
            }
            if (hasDiagWinner)
                return firstDiag;
            else
                return Piece.Empty;
            
        }
        public (Piece, int, int, int) evaluateBoard(Piece mover, Piece alfa, Piece beta)
        {
            Piece winner = Winner();
            if (winner == Piece.Empty)
            {
                if (numMoves == Rows * Rows)
                {
                    numberOfBoardEvaluations++;
                    return (winner, 0, 0, 0);
                }
            }
            else
            {
                numberOfBoardEvaluations++;
                return (winner, 0, 0 ,0);
            }
            int numMovesForBestEval = -1;
            int bestSoFarX = -1;
            int bestSoFarY = -1;
            Piece bestSoFar = (Piece)(-(int)mover);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    if (board[i, j] != Piece.Empty)
                        continue;
                    board[i, j] = mover;
                    numMoves++;
                    (Piece evaluation, int numMovesForEval, int bestSoFarXCalculated, int bestSoFarYCalculated) = evaluateBoard((Piece)(-(int)mover), alfa, beta);
                    board[i, j] = Piece.Empty;
                    numMoves--;
                    if (turn == mover)
                        numMovesForEval++;
                    if (numMovesForBestEval == -1)
                    {
                        numMovesForBestEval = numMovesForEval;
                        bestSoFar = evaluation;
                        bestSoFarX = i;
                        bestSoFarY = j;
                    }
                    if (evaluation == bestSoFar)
                    {
                        if (evaluation == mover && numMovesForEval < numMovesForBestEval)
                        {
                            numMovesForBestEval = numMovesForEval;
                            bestSoFarX = i;
                            bestSoFarY = j;
                        }
                        else if (evaluation == (Piece)(-(int)mover) && numMovesForEval > numMovesForBestEval)
                        {
                            numMovesForBestEval = numMovesForEval;
                            bestSoFarX = i;
                            bestSoFarY = j;
                        }
                    }
                    else if (mover == Piece.Cross &&  evaluation < bestSoFar || mover == Piece.Circle && evaluation > bestSoFar)
                    {
                        bestSoFar = evaluation;
                        bestSoFarX = i;
                        bestSoFarY = j;
                        numMovesForBestEval = numMovesForEval;
                    }
                    if (mover == Piece.Cross && bestSoFar < alfa || mover == Piece.Circle && bestSoFar > beta)
                        goto loopend;
                    if (mover == Piece.Cross)
                        beta = bestSoFar < beta ? bestSoFar : beta;
                    else
                        alfa = bestSoFar > alfa ? bestSoFar : alfa;
                }
            }
            loopend:
            numberOfBoardEvaluations++;
            return (bestSoFar, numMovesForBestEval, bestSoFarX, bestSoFarY);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Piece myPiece = Piece.Empty;
            TicTacToeBoard ticTacToeBoard = new TicTacToeBoard();
            ticTacToeBoard.Print();
            Console.WriteLine("Hello, Player!");
            Console.WriteLine("Do you want to make the first move (Y/N)?");
            String ?answer = Console.ReadLine();
            if (answer == null || answer.Length == 0)
                return;
            if (answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                myPiece = Piece.Circle;
            }
            else
            {
                myPiece = Piece.Cross;
            }
            while (ticTacToeBoard.numMoves < TicTacToeBoard.Rows * TicTacToeBoard.Rows && ticTacToeBoard.Winner() == Piece.Empty)
            {
                if (ticTacToeBoard.turn == myPiece)
                {
                    ticTacToeBoard.numberOfBoardEvaluations = 0;
                    (Piece bestSoFar, int numMovesForBestEval, int bestSoFarX, int bestSoFarY) = ticTacToeBoard.evaluateBoard(myPiece, Piece.Cross, Piece.Circle);
                    if (bestSoFar == (Piece)(-(int)myPiece))
                    {
                        Console.WriteLine("I resign!");
                        break;
                    }

                    else
                    {
                        if (bestSoFar == myPiece)
                            Console.WriteLine($"I claim victory in {numMovesForBestEval} moves.");
                        Console.WriteLine($"My move: {(char)(bestSoFarX + 'a')}, {bestSoFarY}");
                        ticTacToeBoard.board[bestSoFarX, bestSoFarY] = myPiece;
                        ticTacToeBoard.numMoves++;
                        ticTacToeBoard.turn = (Piece)(-(int)ticTacToeBoard.turn);
                    }
                    if (ticTacToeBoard.Winner() == myPiece)
                    {
                        Console.WriteLine("I win.");
                    }
                    Console.WriteLine($"Number of evaluations: {ticTacToeBoard.numberOfBoardEvaluations}");
                }
                else
                {
                    Console.WriteLine("Your turn:");
                    Console.WriteLine("Give me the x coordinate (a, b, c):");
                    string ?Xstr = Console.ReadLine();
                    Console.WriteLine("Give me the y coordinate (0, 1, 2):");
                    string ?Ystr = Console.ReadLine();
                    int xMove = Xstr?.Trim()[0] - 'a' ?? -1;
                    int yMove = int.Parse(Ystr ?? "");
                    while (ticTacToeBoard.board[xMove,yMove] != Piece.Empty)
                    {
                        Console.WriteLine("This space on the board is already occupied");
                        Console.WriteLine("Please enter a new move");
                        Console.WriteLine("Give me the x coordinate (a, b, c):");
                        Xstr = Console.ReadLine();
                        Console.WriteLine("Give me the y coordinate (0, 1, 2):");
                        Ystr = Console.ReadLine();
                        xMove = Xstr?.Trim()[0] - 'a' ?? -1;
                        yMove = int.Parse(Ystr ?? "");

                    }
                    ticTacToeBoard.board[xMove,yMove] = (Piece)(-(int)myPiece);
                    ticTacToeBoard.numMoves++;
                    ticTacToeBoard.turn = (Piece)(-(int)ticTacToeBoard.turn);
                    if (ticTacToeBoard.Winner() == (Piece)(-(int)myPiece))
                    {
                        Console.WriteLine("You won, congratulations!");
                        break;
                    }

                }
                ticTacToeBoard.Print();
            }
            if (ticTacToeBoard.Winner() == Piece.Empty)
            {
                Console.WriteLine("It is a draw!");
            }
        }
    }
}