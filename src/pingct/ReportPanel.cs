using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Ctyar.Pingct
{
    internal class ReportPanel
    {
        private readonly string _emptyLine;
        private readonly int _height;
        private readonly string _name;
        private readonly Queue<(string, int)> _values;
        private readonly int _width;
        private string _lastValue;
        private int _lastValueLength;

        public ReportPanel(string name)
        {
            _name = name;
            _values = new Queue<(string, int)>(_height);
            _lastValue = string.Empty;
            _lastValueLength = 0;

            Console.CursorVisible = false;

            _height = Console.WindowHeight - 3;

            var panel = new Panel(string.Empty);
            var panelEdge = 2; // Panel.EdgeWidth
            var paddings = 2 * (panel.Padding.Left + panel.Padding.Right + panelEdge + 1);
            _width = (Console.WindowWidth - paddings) / 2;

            _emptyLine = new string(' ', _width);
        }

        public void Add()
        {
            if (_values.Count < _height)
            {
                _values.Enqueue((_lastValue, _lastValueLength));
            }
            else
            {
                _values.Dequeue();
                _values.Enqueue((_lastValue, _lastValueLength));
            }

            _lastValue = string.Empty;
            _lastValueLength = 0;
        }

        public void Append(string value, int length)
        {
            _lastValue += value;
            _lastValueLength += length;
        }

        public IRenderable Render()
        {
            return new Panel(Print())
                .SetHeader(_name)
                .RoundedBorder();
        }

        public void Remove()
        {
            if (_values.Count > 0)
            {
                _values.Dequeue();
            }
        }

        private string Print()
        {
            var result = new StringBuilder();

            var i = 0;
            foreach (var (line, length) in _values)
            {
                var printLine = Pad(line, length);
                if (i == _height - 1)
                {
                    result.Append(printLine);
                }
                else
                {
                    result.AppendLine(printLine);
                }

                i++;
            }

            for (var j = 0; j < _height - _values.Count; j++)
            {
                if (j == _height - _values.Count - 1)
                {
                    result.Append(_emptyLine);
                }
                else
                {
                    result.AppendLine(_emptyLine);
                }
            }

            return result.ToString();
        }

        private string Pad(string value, int valueLength)
        {
            if (valueLength >= _width)
            {
                return value;
            }

            var charCount = _width - valueLength;

            return value + new string(' ', charCount);
        }
    }
}