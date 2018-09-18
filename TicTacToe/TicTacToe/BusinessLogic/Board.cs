using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.DataAccessLayer;

namespace TicTacToe.BusinessLogic
{
    public class Board
    {
        static char[] moves = new char[9] { 'A','A', 'A', 'A' , 'A', 'A' , 'A', 'A' ,'A'};
        //Database database = new Database();
        DatabaseCassandra database = new DatabaseCassandra();
        char firstUser = 'O';
        char SecondUser = 'X';
        static  int flag=0;
        static int deciderFlag = 0;
        public bool IsSquareAvailable(int index)
        {
            if (moves[index] == 'A')
            {
                return true;
            }
            return false;
        }
        public bool AddPlayerMove(int index)
        {
            if (flag == 0)
            {
                flag = 1;
                if (deciderFlag == 0)
                {
                    deciderFlag = 1;
                    database.PreviousUser = database.CurrentUser;
                    moves[index] = firstUser;
                }
                return true;
            }
            if (database.PreviousUser != database.CurrentUser)
            {
                if (deciderFlag == 0)
                {
                    deciderFlag = 1;
                    moves[index] = firstUser;
                }
                else
                {
                    deciderFlag = 0;
                    moves[index] = SecondUser;
                }
                database.PreviousUser = database.CurrentUser;
                return true;
            }
            return false;
        }
        char horizontalCheck()
        {
            if(moves[0] == moves[1] && moves[0] == moves[2])
            {
                return moves[0];
            }
            else if(moves[3]==moves[4] && moves[3] == moves[5])
            {
                return moves[3];
            }
            else if(moves[6]==moves[7] && moves[6] == moves[8])
            {
                return moves[6];
            }
            else
            {
                return 'A';
            }
        }
        char verticalCheck()
        {
            if (moves[0] == moves[3] && moves[0] == moves[6])
            {
                return moves[0];
            }
            else if (moves[1] == moves[4] && moves[1] == moves[7])
            {
                return moves[1];
            }
            else if (moves[2] == moves[5] && moves[2] == moves[8])
            {
                return moves[2];
            }
            else
            {
                return 'A';
            }
        }
        char diagonalCheck()
        {
            if (moves[0] == moves[4] && moves[0] == moves[8])
            {
                return moves[0];
            }
            else if (moves[2] == moves[4] && moves[2] == moves[6])
            {
                return moves[3];
            }
            else
            {
                return 'A';
            }
        }
        public string CheckStatus()
        {
            char resultHorizontal = horizontalCheck();
            if (resultHorizontal != 'A')
            {
                if(resultHorizontal == firstUser)
                {
                    return "First user won!!";
                }
                else
                {
                    return "Second user won!";
                }
            }
            char resultVertical = verticalCheck();
            if (resultVertical != 'A')
            {
                if (resultVertical == firstUser)
                {
                    return "First user won!!";
                }
                else
                {
                    return "Second user won!";
                }
            }
            char resultDiagonal = diagonalCheck();
            if (resultDiagonal != 'A')
            {
                if (resultDiagonal == firstUser)
                {
                    return "First user won!!";
                }
                else
                {
                    return "Second user won!";
                }
            }
            for (int i = 0; i < moves.Count(); i++)
            {
                if (moves[i] == 'A')
                {
                    return "In Progress!!";
                }
            }
            return "Draw!!";

        }
    }
}
