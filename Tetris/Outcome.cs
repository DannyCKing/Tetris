using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Tetris
{
    class Outcome : IComparable<Outcome>
    {
        int linesCleared;
        int numberOfHoles;
        int boardHeight;
        int landingHeight;
        int rowTransitions;
        int columnTransitions;
        int wellSums;

        double outcomeRating;

        public Outcome(int linesCleared, int numberOfHoles, int boardHeight, int landingHeight, int rowTransitions, int columnTransitions, int wellSums)
        {
            this.linesCleared = linesCleared;
            this.numberOfHoles = numberOfHoles;
            this.boardHeight = boardHeight;
            this.landingHeight = landingHeight;
            this.rowTransitions = rowTransitions;
            this.columnTransitions = columnTransitions;
            this.wellSums = wellSums;
            GetScore();
        }

        private void GetScore()
        {
            this.outcomeRating = 
                (this.landingHeight * -4.5) +
                (this.linesCleared * 3.418) +
                (this.rowTransitions * -3.2179) +
                (this.columnTransitions * -9.3487) +
                (this.numberOfHoles * -7.899) +
                (this.wellSums * -3.385597);
            if (this.boardHeight > 15)
            {
                outcomeRating -= 30;
            }
            else if (this.boardHeight > 18)
            {
                outcomeRating -= 1000;
            }
            /*
           * ElTetris.prototype.evaluateBoard = function(last_move, board) {
             return GetLandingHeight(last_move, board) * -4.500158825082766 +
                 last_move.rows_removed * 3.4181268101392694 +
                 GetRowTransitions(board, this.number_of_columns) * -3.2178882868487753 +
                 GetColumnTransitions(board, this.number_of_columns) * -9.348695305445199 +
                 GetNumberOfHoles(board, this.number_of_columns) * -7.899265427351652 +
                 GetWellSums(board, this.number_of_columns) * -3.3855972247263626;
           };
           */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Outcome other)
        {
            double x = this.outcomeRating - other.outcomeRating;
            if (x > 0)
            {
                return -1;
            }
            else if (x < 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

    }
}
