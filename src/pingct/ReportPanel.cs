using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

namespace Ctyar.Pingct
{
    internal class ReportPanel : FrameView
    {
        private int _capacity;
        private Queue<List<(MessageType, string)>> _values;
        private List<(MessageType, string)> _lastLine;
        // Top border (1) + bottom border (1) + status bar (1)
        private const int Margin = 3;
        private static readonly Terminal.Gui.Attribute Red = new(Color.BrightRed);
        private static readonly Terminal.Gui.Attribute Green = new(Color.BrightGreen);
        private static readonly Terminal.Gui.Attribute Yellow = new(Color.BrightYellow);
        private static readonly ColorScheme RedScheme = new()
        {
            Disabled = Red,
            Focus = Red,
            HotFocus = Red,
            HotNormal = Red,
            Normal = Red,
        };
        private static readonly ColorScheme GreenScheme = new()
        {
            Disabled = Green,
            Focus = Green,
            HotFocus = Green,
            HotNormal = Green,
            Normal = Green,
        };
        private static readonly ColorScheme YellowScheme = new()
        {
            Disabled = Yellow,
            Focus = Yellow,
            HotFocus = Yellow,
            HotNormal = Yellow,
            Normal = Yellow,
        };

        public ReportPanel(string name) : base(name)
        {
            _values = new();
            _lastLine = new();

            _capacity = Driver.Rows - Margin;

            Application.Resized += ResizeHandler;
        }

        public void Add()
        {
            if (_values.Count == _capacity)
            {
                _values.Dequeue();
            }

            _values.Enqueue(_lastLine);

            _lastLine = new();

            Render();
        }

        public void Append(string value, MessageType messageType)
        {
            _lastLine.Add((messageType, value));
        }

        public void Remove()
        {
            if (_values.Count > 0)
            {
                _values.Dequeue();
            }

            Render();
        }

        private void Render()
        {
            RemoveAll();

            var lines = _values.ToList();
            for (var lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                Add(GetLine(lines[lineIndex], lineIndex));
            }

            Application.Refresh();
        }

        private void ResizeHandler(Application.ResizedEventArgs resizedEventArgs)
        {
            _capacity = resizedEventArgs.Rows - Margin;
            var resizedQueue = new Queue<List<(MessageType, string)>>(_capacity);
            var copyCount = Math.Min(_capacity, _values.Count);

            foreach (var item in _values.Take(copyCount))
            {
                resizedQueue.Enqueue(item);
            }

            _values = resizedQueue;
        }

        private static View GetLine(List<(MessageType, string)> line, int lineIndex)
        {
            var container = new View
            {
                X = 1,
                Y = lineIndex,
                Width = Dim.Fill(),
                Height = 1,
            };

            Label? previous = null;

            foreach (var item in line)
            {
                var label = GetLabel(item.Item1, item.Item2);
                label.X = previous is null ? 0 : Pos.Right(previous);

                container.Add(label);

                previous = label;
            }

            return container;
        }

        private static Label GetLabel(MessageType messageType, string value)
        {
            var colorScheme = messageType switch
            {
                MessageType.Failure => RedScheme,
                MessageType.Warning => YellowScheme,
                MessageType.Success => GreenScheme,
                _ => null,
            };

            return new Label
            {
                Width = value.Length,
                Height = 1,
                Text = value,
                ColorScheme = colorScheme,
            };
        }
    }
}