﻿using System.Runtime.ExceptionServices;

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
        public Piece evaluateBoard(Piece mover)
        {
            if (numMoves == Rows * Rows)
                return Winner();
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
                        return mover;
                    }
                    Piece evaluation = evaluateBoard((Piece)(-(int)mover));
                    board[i, j] = Piece.Empty;
                    numMoves--;
                    if (evaluation == mover)
                    {
                        return evaluation;
                    }
                    if (evaluation == Piece.Empty && evaluation != bestSoFar)
                    {
                        bestSoFar = evaluation;
                    }
                }
            }
            return bestSoFar;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Piece myPiece = Piece.Empty;
            TicTacToeBoard ticTacToeBoard = new TicTacToeBoard();
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
            while(ticTacToeBoard.numMoves < 9)
            {
                if (ticTacToeBoard.turn == myPiece)
                {
                    Piece bestSoFar = (Piece)(-(int)myPiece);
                    int bestSoFarX = -1;
                    int bestSoFarY = -1;
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
                                Console.WriteLine("I win!!");
                                return;
                            }
                            Piece evaluation = ticTacToeBoard.evaluateBoard((Piece)(-(int)myPiece));
                            if (evaluation == Piece.Empty && evaluation != bestSoFar)
                            {
                                bestSoFar = evaluation;
                                bestSoFarX = i;
                                bestSoFarY = j;
                            }
                        }
                    }
                    if (bestSoFar == (Piece)(-(int)myPiece))
                        Console.WriteLine("I resign!");
                    else
                        Console.WriteLine($"My move: {bestSoFarX}, {bestSoFarY}");
                }
                else
                {
                    Console.WriteLine("Your turn:");
                    Console.WriteLine("Give me the x coordinate:");
                    string Xstr = Console.ReadLine();
                    Console.WriteLine("Give me the y coordinate:");
                    string Ystr = Console.ReadLine();
                }
            }
        }
    }
}