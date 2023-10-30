using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase_chess.Client.Rendering.Windows
{
    public class Window
    {
        public readonly char[][] borders = new char[][] {
            new char[8] {
                ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '
            },
            new char[8] {
                '\u2554', '\u2557', '\u255a', '\u255d', '\u2550', '\u2550', '\u2551', '\u2551'
            },
            new char[8] {
                '\u250c', '\u2510', '\u2514', '\u2518', '\u2500', '\u2550', '\u2502', '\u2502'
            },
            new char[8] {
                '\u250f', '\u2513', '\u2517', '\u251b', '\u2501', '\u2501', '\u2503', '\u2503'
            },
            new char[8] {
                '\u256d', '\u256e', '\u2570', '\u256f', '\u2500', '\u2500', '\u2502', '\u2502'
            },
            new char[8] {
                ' ', ' ', ' ', ' ', '\u2500', '\u2500', '\u2502', '\u2502'
            },
            new char[8] {
                '\u253c', '\u253c', '\u253c', '\u253c', '\u2500', '\u2500', '\u2502', '\u2502'
            },
            new char[8] {
                '\u253c', '\u253c', '\u253c', '\u253c', '\u252c', '\u2534', '\u2524', '\u251c'
            },
            new char[8] {
                '\u250c', '\u2510', '\u2514', '\u2518', '\u254c', '\u254c', '\u2506', '\u2506'
            },
        };

        public char[] borderList => borders[(int)border];

        public delegate void UpdateHandler(Window window);
        public static event UpdateHandler Update;

        public int width;
        public int height;
        public BorderStyle border;
        public HorizontalAlignment horizontalAlign;
        public VerticalAlignment verticalAlign;

        public Window[,] windows;
        public List<string> lines;

        public Window(int width, int height, HorizontalAlignment horizontalAlign = HorizontalAlignment.Left, VerticalAlignment verticalAlign = VerticalAlignment.Top, BorderStyle border = BorderStyle.None)
        {
            this.width = width;
            this.height = height;
            this.horizontalAlign = horizontalAlign;
            this.verticalAlign = verticalAlign;
            this.border = border;

            lines = new List<string>();

            windows = new Window[1, 1];
        }

        public bool addWindow(Window window, int x = 0, int y = 0)
        {
            // Extend rows or columns if needed
            if (windows.GetLength((int)Layout.X) <= x || windows.GetLength((int)Layout.Y) <= y)
            {
                var temp = windows;
                int row = Math.Max(x, windows.GetLength((int)Layout.X));
                int col = Math.Max(y, windows.GetLength((int)Layout.Y));
                windows = new Window[row + 1, col + 1];
                for (int i = 0; i < temp.GetLength((int)Layout.X); i++)
                {
                    for (int j = 0; j < temp.GetLength((int)Layout.Y); j++)
                    {
                        windows[i, j] = temp[i, j];
                    }
                }
            }

            if (windows[x, y] is not null) return false;

            windows[x, y] = window;

            return true;
        }

        public bool removeWindow(Window window)
        {
            for (int x = 0; x < windows.GetLength((int)Layout.X); x++)
            {
                for (int y = 0; y < windows.GetLength((int)Layout.Y); y++)
                {
                    if (windows[x, y] == window) return removeWindow(x, y);
                }
            }

            return false;
        }

        public bool removeWindow(int x, int y = 0)
        {
            if (windows[x, y] is null) return false;

            windows[x, y] = null;

            truncateDimensions();

            return true;
        }

        public void truncateDimensions()
        {
            List<int>[] truncateList = { new List<int>(), new List<int>() };
            bool[] truncate = { true, true };

            // Find rows to truncate
            for (int i = windows.GetLength((int)Layout.X) - 1; i >= 0; i--)
            {
                truncate[(int)Layout.X] = false;
                for (int j = windows.GetLength((int)Layout.Y) - 1; j >= 0; j--)
                {
                    if (windows[i, j] is not null) truncate[(int)Layout.X] = false;
                }
                if (truncate[(int)Layout.X]) truncateList[(int)Layout.X].Add(i);
            }

            // Truncate rows
            foreach (var row in truncateList[(int)Layout.X])
            {
                var temp = windows;
                windows = new Window[temp.GetLength((int)Layout.X) - 2, temp.GetLength((int)Layout.Y) - 1];
                for (int i = 0; i < temp.GetLength((int)Layout.X); i++)
                {
                    if (i == row) continue;
                    for (int j = 0; j < temp.GetLength((int)Layout.Y); j++)
                    {
                        int current = i >= row ? i : i + 1;
                        windows[current, j] = temp[current, j];
                    }
                }
                truncateList[(int)Layout.X].Select((x, i) => truncateList[(int)Layout.X][i] = --x);
            }

            // Find columns to truncate
            for (int k = windows.GetLength((int)Layout.Y) - 1; k >= 0; k--)
            {
                truncate[(int)Layout.Y] = false;
                for (int l = windows.GetLength((int)Layout.X) - 1; l >= 0; l--)
                {
                    if (windows[l, k] is not null) truncate[(int)Layout.Y] = false;
                }
                if (truncate[(int)Layout.Y]) truncateList[(int)Layout.Y].Add(k);
            }

            // Truncate columns
            foreach (var col in truncateList[(int)Layout.Y])
            {
                var temp = windows;
                windows = new Window[temp.GetLength((int)Layout.X) - 1, temp.GetLength((int)Layout.Y) - 2];
                for (int k = 0; k < temp.GetLength((int)Layout.Y); k++)
                {
                    if (k == col) continue;
                    for (int l = 0; l < temp.GetLength((int)Layout.X); l++)
                    {
                        int current = k >= col ? k : k + 1;
                        windows[l, current] = temp[l, current];
                    }
                }
                truncateList[(int)Layout.X].Select((y, i) => truncateList[(int)Layout.Y][i] = --y);
            }
        }

        public virtual void addLine(string line)
        {
            lines.Add(line);
            if (lines.Count > height) lines.RemoveAt(0);
        }

        public virtual string getLine(int index)
        {
            if (index >= lines.Count || index < 0) return "";
            return lines[index];
        }

        public virtual int getLineCount()
        {
            return lines.Count;
        }

        public virtual string renderLine(int index)
        {
            string line = "";

            int tempIndex = index;
            for (int y = 0; y < windows.GetLength((int)Layout.Y); y++)
            {
                int height = maxWindowHeight(y);
                if (height <= tempIndex)
                {
                    tempIndex -= height;
                    continue;
                }
                for (int x = 0; x < windows.GetLength((int)Layout.X); x++)
                {
                    int width = maxWindowWidth(x);
                    Window window = windows[x, y];
                    string windowLine = "";
                    if (window is not null)
                    {
                        tempIndex = alignVertical(tempIndex, window.height, maxChildrenHeight());
                        if (window.height > tempIndex) windowLine = window.renderLine(tempIndex);
                    }
                    windowLine = windowLine.FormatPadRight(width, ' ');
                    windowLine = windowLine.FormatSubstring(0, width);
                    line += windowLine;
                }
                break;
            }

            int offsetIndex = alignVertical(index, getLineCount(), height);
            if (border != BorderStyle.None && verticalAlign == VerticalAlignment.Top) offsetIndex--;
            line += getLine(offsetIndex);

            line = alignHorizontal(line, width);
            if (border != BorderStyle.None) line = drawBorder(line, index);
            return line;
        }

        public int alignVertical(int index, int height, int maxHeight)
        {
            switch (verticalAlign)
            {
                case VerticalAlignment.Bottom:
                    index -= maxHeight - height;
                    break;
                case VerticalAlignment.Middle:
                    index -= (maxHeight - height) / 2;
                    break;
            }

            return index;
        }

        public string alignHorizontal(string line, int width)
        {
            switch (horizontalAlign)
            {
                case HorizontalAlignment.Left:
                    line = line.FormatPadRight(width, ' ');
                    break;
                case HorizontalAlignment.Right:
                    if (width < line.FormatLength()) line = line.FormatSubstring(line.FormatLength() - width, width);
                    line = line.FormatPadLeft(width, ' ');
                    break;
                case HorizontalAlignment.Center:
                    int length = width;
                    if (line.FormatLength() < width) length = (width - line.FormatLength()) / 2 + line.FormatLength() - 1;
                    line = line.FormatPadLeft(length, ' ');
                    line = line.FormatPadRight(width, ' ');
                    break;
            }

            line = line.FormatSubstring(0, width);
            return line;
        }

        public string drawBorder(string line, int index)
        {
            switch (horizontalAlign)
            {
                case HorizontalAlignment.Center:
                case HorizontalAlignment.Left:
                    line = line.FormatSubstring(0, width - 2);
                    break;
                case HorizontalAlignment.Right:
                    line = line.FormatSubstring(2, width - 2);
                    break;
            }

            if (index == 0)
            {
                line = borderList[(int)Border.TopLeft].ToString().FormatPadRight(width - 1, borderList[(int)Border.Top]) + borderList[(int)Border.TopRight];
            }
            else if (index == height - 1)
            {
                line = borderList[(int)Border.BottomLeft].ToString().FormatPadRight(width - 1, borderList[(int)Border.Bottom]) + borderList[(int)Border.BottomRight];
            }
            else if (index >= 0 && index < height)
            {
                line = borderList[(int)Border.Left] + line + borderList[(int)Border.Right];
            }

            return line;
        }

        public virtual List<string> render()
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < height; i++)
            {
                lines.Add(renderLine(i));
            }
            return lines;
        }

        public int maxWindowHeight(int row)
        {
            int height = 0;
            for (int x = 0; x < windows.GetLength((int)Layout.X); x++)
            {
                Window window = windows[x, row];
                if (window is null) continue;
                height = Math.Max(height, window.height);
            }
            return height;
        }

        public int maxWindowWidth(int col)
        {
            int width = 0;
            for (int y = 0; y < windows.GetLength((int)Layout.Y); y++)
            {
                Window window = windows[col, y];
                if (window is null) continue;
                width = Math.Max(width, window.width);
            }
            return width;
        }

        public int maxChildrenHeight()
        {
            int height = lines.Count;
            for (int y = 0; y < windows.GetLength((int)Layout.Y); y++)
            {
                height = Math.Max(height, maxWindowHeight(y));
            }
            return height;
        }

        public void NeedsUpdate()
        {
            Update?.Invoke(this);
        }
    }
}
