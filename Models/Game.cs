using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sokoban.Models
{
    public class Game
    {
        char[][] _cActiveGameBoard;
        int _iCurrentLevel = 0;
        int _iMoves = 0;             // current moves in this attempt to solve level.
        int _iFinishedInMoves = 0;   // zero if never finished this level. 

        public Game()
        {
            RestartLevel();
        }

        public int FinishedInMoves
        {
            get { return _iFinishedInMoves; }
            set { _iFinishedInMoves = value; }
        }

        public int Moves
        {
            get { return _iMoves; }
            set { _iMoves = value; }
        }

        public string[] SupportedActions
        {
            get { return new string[] { "r", "n", "w", "e", "s" }; }
        }

        private SokoPoint GetAttemptedMoveLocation(string sGameAction, int iX, int iY)
        {
            int iDX = 0, iDY = 0;

            if (sGameAction == "n")
                iDY = -1;
            else if (sGameAction == "w")
                iDX = -1;
            else if (sGameAction == "e")
                iDX = 1;
            else if (sGameAction == "s")
                iDY = 1;

            int iTX = iX + iDX;
            int iTY = iY + iDY;

            if (iTX < 0 || iTY < 0 || iTX >= _cActiveGameBoard[0].Length - 1 || iTY >= _cActiveGameBoard.Length - 1)
                return null;
            else
                return new SokoPoint(iTX, iTY);
        }

        public string[] GetBlankTemplateByLevel(int iLevel)
        {
            return Templates.GetSokoTemplates()[iLevel];
        }

        private char GetCharFromSokoPoint(SokoPoint sp)
        {
            return _cActiveGameBoard[sp.Y][sp.X];
        }

        private char[][] GetCharArrayTemplate(string[] sTemplate)
        {
            //we're using jagged arrays and trusting our enforcement of regular rectangles. if not rect, will be a mess.
            List<char[]> lstChars = new List<char[]>();
            foreach (string s in sTemplate)
            {
                lstChars.Add(s.ToCharArray());
            }

            return lstChars.ToArray();
        }

        private SokoPoint GetFirstSymbolFromGameLayout(char cSymbol, char[][] cGameBoard)
        {
            for (int i = 0; i < cGameBoard.Length; i++)
            {
                for (int j = 0; j < cGameBoard[i].Length; j++)
                {
                    if (cGameBoard[i][j] == cSymbol)
                        return new SokoPoint(j, i);     // j is "x" and i is "y".  don't be confused.
                }
            }

            //if we found nothing, return null
            return null;
        }

        public string GetHTMLDisplay() 
        {
            System.Text.StringBuilder sbHTML = new System.Text.StringBuilder();

            for (int i = 0; i < _cActiveGameBoard.Length; i++)
            {
                string sRaw = new string(_cActiveGameBoard[i]);
                string sLineHTML = sRaw.Replace(" ", "&nbsp;");
                sLineHTML = System.Text.RegularExpressions.Regex.Replace(sLineHTML, @"(@|\+)", "<span class=\"worker\">$1</span>");
                sbHTML.Append(sLineHTML);
                sbHTML.Append("<br />");
                sbHTML.Append(Environment.NewLine);     // nicer if we want to view source.
            }

            return sbHTML.ToString();
        }

        public Game NextLevel()
        {
            //if some crazy person wins all the levels, their reward is to start over.  :)
            if (_iCurrentLevel < Templates.GetSokoTemplates().Count - 1)
                _iCurrentLevel++;
            else
                _iCurrentLevel = 0;

            RestartLevel();

            return this;
        }

        public Game ProcessGameAction(string sGameAction)
        {
            if (sGameAction == "r")
            {
                RestartLevel();
                return this;
            }

            SokoPoint wsp = GetFirstSymbolFromGameLayout('@', _cActiveGameBoard) ?? GetFirstSymbolFromGameLayout('+', _cActiveGameBoard);
            if (wsp == null)
                throw new InvalidOperationException("Game board is missing worker, cannot continue game.");

            //get the char we're trying to move into, and also whatever char is one further in same direction.
            SokoPoint spAttempt = GetAttemptedMoveLocation(sGameAction, wsp.X, wsp.Y);

            if (spAttempt != null)
            {
                //figure out if we need to replace our worker with a space or a storage slot.
                char cExitedSpace;
                if (GetCharFromSokoPoint(wsp) == '@')
                    cExitedSpace = ' ';
                else
                    cExitedSpace = '.';

                char cAttempt = GetCharFromSokoPoint(spAttempt);

                if (cAttempt == ' ')
                {
                    _cActiveGameBoard[spAttempt.Y][spAttempt.X] = '@';
                    _cActiveGameBoard[wsp.Y][wsp.X] = cExitedSpace;
                    _iMoves++;
                }
                else if (cAttempt == '.')
                {
                    _cActiveGameBoard[spAttempt.Y][spAttempt.X] = '+';
                    _cActiveGameBoard[wsp.Y][wsp.X] = cExitedSpace;
                    _iMoves++;
                }
                else if (cAttempt == 'o' || cAttempt == '*')
                {
                    //here's the interesting one. can we move the crate? see what's in the space just past the crate.
                    SokoPoint spCrateAttempt = GetAttemptedMoveLocation(sGameAction, spAttempt.X, spAttempt.Y);

                    if (spCrateAttempt != null)
                    {
                        char cCrateAttempt = GetCharFromSokoPoint(spCrateAttempt);

                        //a crate can *only* move into a blank square or a storage slot. if so, move crate and worker. if no, do nothing.
                        if (cCrateAttempt == ' ' || cCrateAttempt == '.')
                        {
                            char cCrateExitedSpace;
                            if (cAttempt == 'o')
                                cCrateExitedSpace = '@';
                            else
                                cCrateExitedSpace = '+';

                            if (cCrateAttempt == ' ')
                                _cActiveGameBoard[spCrateAttempt.Y][spCrateAttempt.X] = 'o';
                            else
                                _cActiveGameBoard[spCrateAttempt.Y][spCrateAttempt.X] = '*';

                            _cActiveGameBoard[spAttempt.Y][spAttempt.X] = cCrateExitedSpace;
                            _cActiveGameBoard[wsp.Y][wsp.X] = cExitedSpace;
                            _iMoves++;
                        }
                    }
                }
            }

            //we've made all the changes, let's see if we've got any crates left.
            SokoPoint spC = GetFirstSymbolFromGameLayout('o', _cActiveGameBoard);
            if (spC == null)
                _iFinishedInMoves = _iMoves;

            return this;
        }

        public void RestartLevel()
        {
            _iMoves = 0;
            _iFinishedInMoves = 0;
            _cActiveGameBoard = GetCharArrayTemplate(GetBlankTemplateByLevel(_iCurrentLevel));

            //make sure we have a worker.
            if (GetFirstSymbolFromGameLayout('@', _cActiveGameBoard) == null)
                throw new InvalidOperationException("Game board is missing worker, cannot start game.");
        }
    }

    //because the System.Windows Point class isn't available here by default.
    public class SokoPoint
    {
        int _iX = 0;
        int _iY = 0;

        public SokoPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X
        {
            get { return _iX; }
            set { _iX = value; }
        }

        public int Y
        {
            get { return _iY; }
            set { _iY = value; }
        }
    }


}