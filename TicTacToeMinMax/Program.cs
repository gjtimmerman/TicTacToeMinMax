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
                    break;
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
                    break;
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
        public (Piece, int) evaluateBoard(Piece mover)
        {
            Piece winner = Winner();
            if (winner == Piece.Empty)
            {
                if (numMoves == Rows * Rows)
                    return (winner, 0);
            }
            else
                return (winner, 0);
            int numMovesForBestEval = -1;
            Piece bestSoFar = (Piece)(-(int)mover);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    if (board[i, j] != Piece.Empty)
                        continue;
                    board[i, j] = mover;
                    numMoves++;
                    if (Winner() == mover)
                    {
                        board[i, j] = Piece.Empty;
                        numMoves--;
                        return (mover, 1);
                    }
                    (Piece evaluation, int numMovesForEval) = evaluateBoard((Piece)(-(int)mover));
                    board[i, j] = Piece.Empty;
                    numMoves--;
                    if (turn == mover)
                        numMovesForEval++;
                    if (numMovesForBestEval == -1)
                    {
                        numMovesForBestEval = numMovesForEval;
                        bestSoFar = evaluation;
                    }
                    if (evaluation == bestSoFar)
                    {
                        if (evaluation == mover && numMovesForEval < numMovesForBestEval)
                            numMovesForBestEval = numMovesForEval;
                        else if (evaluation == (Piece)(-(int)mover) && numMovesForEval > numMovesForBestEval)
                            numMovesForBestEval = numMovesForEval;
                    }
                    else if (mover == Piece.Cross &&  evaluation < bestSoFar || mover == Piece.Circle && evaluation > bestSoFar)
                    {
                        bestSoFar = evaluation;
                        numMovesForBestEval = numMovesForEval;
                    }
                }
            }
            return (bestSoFar, numMovesForBestEval);
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
                    Piece bestSoFar = (Piece)(-(int)myPiece);
                    int bestSoFarX = -1;
                    int bestSoFarY = -1;
                    int numMovesForBestEval = TicTacToeBoard.Rows * TicTacToeBoard.Rows + 1;
                    for (int i = 0; i < TicTacToeBoard.Rows; i++)
                    {
                        for (int j = 0; j < TicTacToeBoard.Rows; j++)
                        {
                            if (ticTacToeBoard.board[i, j] != Piece.Empty)
                                continue;
                            ticTacToeBoard.board[i, j] = myPiece;
                            ticTacToeBoard.numMoves++;
                            if (ticTacToeBoard.Winner() == myPiece)
                            {
                                bestSoFar = myPiece;
                                bestSoFarX = i;
                                bestSoFarY = j;
                                numMovesForBestEval = 1;
                                goto loopend;
                            }
                            (Piece evaluation, int numMovesForEval) = ticTacToeBoard.evaluateBoard((Piece)(-(int)myPiece));
                            numMovesForEval++;
                            if (bestSoFarX == -1)
                            {
                                bestSoFar = evaluation;
                                bestSoFarX = i;
                                bestSoFarY = j;
                                numMovesForBestEval = numMovesForEval;
                            }
                            if (evaluation == bestSoFar && evaluation == myPiece)
                            {
                                if (numMovesForEval < numMovesForBestEval)
                                {
                                    bestSoFarX = i;
                                    bestSoFarY = j;
                                    numMovesForBestEval = numMovesForEval;
                                }

                            }
                            else if (evaluation == bestSoFar && evaluation == (Piece)(-(int)myPiece))
                            {
                                if (numMovesForEval > numMovesForBestEval)
                                {
                                    bestSoFarX = i;
                                    bestSoFarY = j;
                                    numMovesForBestEval = numMovesForEval;
                                }
                            }
                            else if (myPiece == Piece.Cross && evaluation < bestSoFar || myPiece == Piece.Circle && evaluation > bestSoFar)
                            {
                                bestSoFar = evaluation;
                                bestSoFarX = i;
                                bestSoFarY = j;
                                numMovesForBestEval = numMovesForEval;
                            }
                            ticTacToeBoard.board[i,j] = Piece.Empty;
                            ticTacToeBoard.numMoves--;
                        }
                    }
                    loopend: 
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