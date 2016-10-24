using FilePersistence.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePersistence.View
{
   
    public class ConsoleMenu
    {
        #region FIELD

        private string _name;
        private int _WIDTH;
        private int _HEIGHT;

        #endregion

        #region PROPERTY

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int WIDTH
        {
            get { return _WIDTH; }
            set { _WIDTH = value; }
        }

        public int HEIGHT
        {
            get { return _HEIGHT; }
            set { _HEIGHT = value; }
        }

        #endregion

        #region CONSTRUCTOR

        public ConsoleMenu()
        {
            Console.CursorVisible = false;
            
        }

        public ConsoleMenu(int w, int h) : this()
        {
            _WIDTH = w;
            _HEIGHT = h;
            Console.WindowWidth = _WIDTH;
            Console.WindowHeight = _HEIGHT;
            Console.BufferWidth = _WIDTH;
            Console.BufferHeight = _HEIGHT;        
        }

        #endregion

        #region DRAW

        /// <summary>
        /// Draw Lines
        /// </summary>
        public void DrawLine(int x, int y, int l, bool horizontal, char c)
        {
            l--;

            if (horizontal) //Horizontal
                for (int i = 0; i < l; i++) WriteAt(x + i, y, c);
            else    //Vertical
                for (int i = 0; i < l; i++) WriteAt(x, y + i, c);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void DrawRectangle(int x, int y, int w, int h)
        {
            w--;
            h--;

            //Making the corners
            WriteAt(x, y, '╔');
            WriteAt(x + w, y, '╗');
            WriteAt(x, y + h, '╚');
            WriteAt(x + w, y + h, '╝');

            //Make the sides
            DrawLine(x + 1, y, w, true, '═');
            DrawLine(x + 1, y + h, w, true, '═');
            DrawLine(x, y + 1, h, false, '║');
            DrawLine(x + w, y + 1, h, false, '║');
        }

        /// <summary>
        /// Creates a grid
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y positions</param>
        /// <param name="rowNum">Number of Rows</param>
        /// <param name="colNum">Number of Columns</param>
        /// <param name="cellWidth">Cell's Width</param>
        /// <param name="cellHeight">Cell's Height</param>
        public void DrawGrid(int x, int y, int rowNum, int colNum, int cellWidth, int cellHeight)
        {
            //█, ═, ║, ╩, ╦, ╠, ╣, ╔, ╗, ╚, ╝, ╬

            rowNum = Math.Abs(rowNum);
            colNum = Math.Abs(colNum);
            cellWidth = Math.Abs(cellWidth);
            cellHeight = Math.Abs(cellHeight);

            int w = (colNum * cellWidth) + colNum + 1;
            int h = (rowNum * cellHeight) + rowNum + 1;

            //Draw frame
            DrawRectangle(x, y, w, h);

            //Draw Main Lines: ═ & ║
            for (int xI = x + cellWidth + 1; xI <= x + w - cellWidth - 1; xI += cellWidth + 1) DrawLine(xI, y + 1, h - 1, false, '║');
            for (int yI = y + cellHeight + 1; yI <= y + h - cellHeight - 1; yI += cellHeight + 1) DrawLine(x + 1, yI, w - 1, true, '═');

            //Draw Top & Bottom Juntions: ╦ & ╩
            for (int xI = x + cellWidth + 1; xI <= x + w - cellWidth - 1; xI += cellWidth + 1) WriteAt(xI, y, '╦');
            for (int xI = x + cellWidth + 1; xI <= x + w - cellWidth - 1; xI += cellWidth + 1) WriteAt(xI, y + h - 1, '╩');

            //Draw Left & Right Juntions: ╠ & ╣
            for (int yI = y + cellHeight + 1; yI <= y + h - cellHeight - 1; yI += cellHeight + 1) WriteAt(x, yI, '╠');
            for (int yI = y + cellHeight + 1; yI <= y + h - cellHeight - 1; yI += cellHeight + 1) WriteAt(x + w - 1, yI, '╣');

            //Draw Inner Juntions: ╬
            for (int yI = y + cellHeight + 1; yI <= y + h - cellHeight - 1; yI += cellHeight + 1)
                for (int xI = x + cellWidth + 1; xI <= x + w - cellWidth - 1; xI += cellWidth + 1)
                    WriteAt(xI, yI, '╬');
        }

        /// <summary>
        /// Draw a menu from a list
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="menu"></param>
        public void DrawMenu(int x, int y, int w, int h, List<string> menu)
        {
            Console.Clear();
            int i = 2;

            DrawRectangle(x, y, w, h);

            foreach (string s in menu)
            {
                WriteAt(x + 2, y + i, s);

                i += 2;
            }
        }

        /// <summary>
        /// Draws the menu at the center of the screen
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="menu"></param>
        public void DrawMenu(int w, int h, List<string> menu)
        {
            int x = (_WIDTH / 2) - (w / 2);
            int y = (_HEIGHT / 2) - (h / 2);

            DrawRectangle(x, y, w, h);
            DrawMenu(x, y, w, h, menu);
        }

        /// <summary>
        /// Draw a single ligned textbox
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="s"></param>
        public void DrawTextBox(int x, int y, int w, int h, string s)
        {
            DrawRectangle(x, y, w, h);
            WriteAt(x + (w / 10), y + h / 2, s);
        }

        /// <summary>
        /// Override of DrawTextBox(x,y,w,h,s)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="s"></param>
        public void DrawTextBox(int x, int y, string s)
        {
            DrawTextBox(x, y, s.Length + 11, 5, s);
        }

        /// <summary>
        /// Simpler DrawTextBox override
        /// </summary>
        /// <param name="s"></param>
        public void DrawTextBox(string s)
        {
            int w = s.Length + 12;
            int h = 5;
            int x = _WIDTH / 2 - w / 2;
            int y = _HEIGHT / 2 - h / 2;

            DrawTextBox(x, y, w, h, s);
        }

        /// <summary>
        /// DrawTextBox override that can clear the screen
        /// </summary>
        /// <param name="s"></param>
        /// <param name="v"></param>
        public void DrawTextBox(string s, bool v)
        {
            if (v) Console.Clear();

            DrawTextBox(s);
        }

        /// <summary>
        /// Draw a specialized text box
        /// </summary>
        public void DrawPromptBox(string s)
        {
            Console.Clear();

            int w = s.Length + 21;
            int h = 7;
            int x = _WIDTH / 2 - w / 2;
            int y = _HEIGHT / 2 - h / 2;

            DrawRectangle(x, y, w, h);
            WriteAt(x + w / 8, y + 2, s);

            Console.SetCursorPosition(x + w / 8, y + 4);
            Console.CursorVisible = true;
        }

        #endregion

        #region OTHER

        /// <summary>
        /// Change Front and Back color of console
        /// </summary>
        /// <param name="fColor"></param>
        /// <param name="bColor"></param>
        public void ChangeColors(ConsoleColor fColor, ConsoleColor bColor)
        {
            Console.ForegroundColor = fColor;
            Console.BackgroundColor = bColor;
        }

        /// <summary>
        /// Write Single Character
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="c"></param>
        public void WriteAt(int x, int y, char c)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(c);
        }

        /// <summary>
        /// Write Full sentence
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="s"></param>
        public void WriteAt(int x, int y, string s)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(@s);
        }

        /// <summary>
        /// Prompt a ConsoleKey
        /// </summary>
        /// <returns></returns>
        public ConsoleKey PromptKey()
        {
            return Console.ReadKey(true).Key;
        }

        #endregion
    }
}
