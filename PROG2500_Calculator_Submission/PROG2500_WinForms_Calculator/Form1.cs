using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace CalculatorApp
{
    public class Form1 : Form
    {
        private TextBox display;
        private string currentOperator = string.Empty;
        private double accumulator = 0.0;
        private bool nextClear = false;

        public Form1()//
        {
            this.Text = "Calculator";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.ClientSize = new Size(320, 420);
            InitializeUi();
        }

        private void InitializeUi()
        {
            display = new TextBox
            {
                ReadOnly = true,
                Text = "0",
                Font = new Font("Segoe UI", 20, FontStyle.Regular),
                TextAlign = HorizontalAlignment.Right,
                Location = new Point(10, 10),
                Size = new Size(300, 45)
            };
            this.Controls.Add(display);

            string[,] layout = new string[5,4]
            {
                {"C", "←", "+/-", "/"},
                {"7", "8", "9", "*"},
                {"4", "5", "6", "-"},
                {"1", "2", "3", "+"},
                {"0", ".", "=", "%"}
            };

            int startY = 70; int startX = 10; int bw = 70; int bh = 55; int gap = 5;

            for (int r = 0; r < layout.GetLength(0); r++)
            {
                for (int c = 0; c < layout.GetLength(1); c++)
                {
                    string text = layout[r, c];
                    Button btn = new Button
                    {
                        Text = text,
                        Font = new Font("Segoe UI", 12),
                        Size = new Size(bw, bh),
                        Location = new Point(startX + c * (bw + gap), startY + r * (bh + gap))
                    };
                    btn.Click += OnButtonClick;
                    this.Controls.Add(btn);
                }
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            string t = btn.Text;

            if (char.IsDigit(t, 0)) { Digit(t); return; }
            switch (t)
            {
                case ".": DecimalPoint(); break;
                case "+": case "-": case "*": case "/": Operator(t); break;
                case "=": EqualsOp(); break;
                case "C": ClearAll(); break;
                case "←": Backspace(); break;
                case "+/-": ToggleSign(); break;
                case "%": Percent(); break;
            }
        }

        private void Digit(string d)
        {
            if (nextClear || display.Text == "0")
            {
                display.Text = d;
                nextClear = false;
            }
            else
            {
                display.Text += d;
            }
        }

        private void DecimalPoint()
        {
            if (nextClear) { display.Text = "0"; nextClear = false; }
            if (!display.Text.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                display.Text += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }

        private void Operator(string op)
        {
            if (!string.IsNullOrEmpty(currentOperator))
                ComputePending();
            else
                accumulator = Parse(display.Text);

            currentOperator = op;
            nextClear = true;
        }

        private void EqualsOp()
        {
            ComputePending();
            currentOperator = string.Empty;
        }

        private void ComputePending()
        {
            double rhs = Parse(display.Text);
            double result = accumulator;
            try
            {
                switch (currentOperator)
                {
                    case "+": result = accumulator + rhs; break;
                    case "-": result = accumulator - rhs; break;
                    case "*": result = accumulator * rhs; break;
                    case "/": result = rhs == 0 ? double.NaN : accumulator / rhs; break;
                    default: result = rhs; break;
                }
            }
            finally
            {
                accumulator = result;
                display.Text = Format(result);
                nextClear = true;
            }
        }

        private void ClearAll()
        {
            accumulator = 0.0;
            currentOperator = string.Empty;
            display.Text = "0";
            nextClear = false;
        }

        private void Backspace()
        {
            if (!nextClear && display.Text.Length > 0)
            {
                display.Text = display.Text.Substring(0, display.Text.Length - 1);
                if (display.Text.Length == 0 || display.Text == "-") display.Text = "0";
            }
        }

        private void ToggleSign()
        {
            if (display.Text.StartsWith("-")) display.Text = display.Text.Substring(1);
            else if (display.Text != "0") display.Text = "-" + display.Text;
        }

        private void Percent()
        {
            double v = Parse(display.Text);
            v = v / 100.0;
            display.Text = Format(v);
        }

        private static double Parse(string s)
        {
            if (double.TryParse(s, out double v)) return v;
            return double.NaN;
        }

        private static string Format(double v)
        {
            if (double.IsNaN(v) || double.IsInfinity(v)) return "Error";
            return v.ToString("G15");
        }
    }
}
