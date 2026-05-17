using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HammingDesktop;


public class Hamming
{
    [DllImport("hamming.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint run(uint data, byte dataLength,
                                    out ulong out_memory_in, out ulong out_memory_out,
                                    out uint out_data_old, out uint out_data_new,
                                    out uint out_f_old, out uint out_f_new,
                                    out uint out_syndrome, out uint out_total_length);
}

public partial class Form0 : Form
{
    private Label _data_in_label, _data_in_length_label;
    private Label _memory_in_label, _memory_out_label, _data_old_label, _data_new_label, _f_old_label, _f_new_label, _syndrome_label, _data_out_label;
    private TextBox _data_in_text, _data_in_length_text;
    
    private RichTextBox _memory_in_text, _memory_out_text, _data_old_text, _data_new_text, _f_old_text, _f_new_text, _syndrome_text, _data_out_text;
    
    private Button _data_in_button;

    private Color bgDark = Color.FromArgb(20, 20, 20);
    private Color fgGreen = Color.Green;
    private Color boxBg = Color.FromArgb(30, 30, 30);
    private Font terminalFont = new Font("Consolas", 12, FontStyle.Bold);

    public Form0()
    {
        this.Text = "Error Correction Simulator";
        this.Size = new Size(1280, 720);
        this.StartPosition = FormStartPosition.CenterScreen;

        this.BackColor = bgDark;
        this.ForeColor = fgGreen;

        int width = 450; int height = 40;
        int startX = 40; int startY = 30;
        int gapX = 480; int gapY = 70;

        /* INPUT */

        _data_in_label = CreateLabel("Data In", startX, startY);
        _data_in_text = CreateTextBox(startX, startY + 25, width);

        _data_in_length_label = CreateLabel("Data Length", startX + gapX, startY);
        _data_in_length_text = CreateTextBox(startX + gapX, startY + 25, width);

        _data_in_button = new Button();
        _data_in_button.Text = "EXECUTE PIPELINE";
        _data_in_button.Location = new Point(startX + gapX * 2, startY + 22);
        _data_in_button.Size = new Size(width/2, height);
        _data_in_button.FlatStyle = FlatStyle.Flat;
        _data_in_button.FlatAppearance.BorderColor = fgGreen;
        _data_in_button.FlatAppearance.BorderSize = 2;
        _data_in_button.BackColor = bgDark;
        _data_in_button.ForeColor = fgGreen;
        _data_in_button.Font = new Font("Consolas", 10, FontStyle.Bold);
        _data_in_button.Cursor = Cursors.Hand;
        _data_in_button.Click += ButtonEvent;
        this.Controls.Add(_data_in_button);

        /* OUTPUT */

        int outY = startY + gapY + 20;

        // MEMORY IN
        _memory_in_label = CreateLabel("Memory In (Clean)", startX, outY);
        _memory_in_text = CreateRichTextBox(startX, outY + 25, width);

        // DATA OLD
        _data_old_label = CreateLabel("Data Old", startX + gapX, outY);
        _data_old_text = CreateRichTextBox(startX + gapX, outY + 25, width);

        // MEMORY OUT
        outY += gapY;
        _memory_out_label = CreateLabel("Memory Out (Corrupted)", startX, outY);
        _memory_out_text = CreateRichTextBox(startX , outY + 25, width);

        // DATA NEW
        _data_new_label = CreateLabel("Data New", startX + gapX, outY);
        _data_new_text = CreateRichTextBox(startX + gapX, outY + 25, width);

        // F OLD
        outY += gapY;
        _f_old_label = CreateLabel("F Old (Parity)", startX, outY);
        _f_old_text = CreateRichTextBox(startX, outY + 25, width);

        _f_new_label = CreateLabel("F New (Parity)", startX + gapX, outY);
        _f_new_text = CreateRichTextBox(startX + gapX, outY + 25, width);

        // SYNDROME
        outY += gapY;
        _syndrome_label = CreateLabel("Syndrome (Error Pos)", startX, outY);
        _syndrome_text = CreateRichTextBox(startX, outY + 25, width);


        // DATA OUT
        _data_out_label = CreateLabel("Final Data Out", startX + gapX, outY);
        _data_out_text = CreateRichTextBox(startX + gapX, outY + 25, width);

    }
    private Label CreateLabel(string text, int x, int y)
    {
        Label lbl = new Label { Text = text, Location = new Point(x, y), Size = new Size(200, 20), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        this.Controls.Add(lbl);
        return lbl;
    }

    private TextBox CreateTextBox(int x, int y, int width)
    {
        TextBox tb = new TextBox { Location = new Point(x, y), Size = new Size(width, 30), BackColor = boxBg, ForeColor = Color.White, Font = terminalFont, BorderStyle = BorderStyle.FixedSingle };
        this.Controls.Add(tb);
        return tb;
    }

    private RichTextBox CreateRichTextBox(int x, int y, int width)
    {
        RichTextBox rtb = new RichTextBox { Location = new Point(x, y), Size = new Size(width, 35), BackColor = boxBg, ForeColor = fgGreen, Font = terminalFont, ReadOnly = true, BorderStyle = BorderStyle.FixedSingle, Multiline = false };
        this.Controls.Add(rtb);
        return rtb;
    }
    private string ToBinary(ulong value, int bitLength)
    {
        string rawBinary = Convert.ToString((long)value, 2).PadLeft(bitLength, '0');

        string formattedBinary = "";

        int counter = 0;

        for(int i = rawBinary.Length - 1; i>= 0; i--)
        {
            formattedBinary = rawBinary[i] + formattedBinary;
            counter++;

            if(counter % 4 == 0 && i > 0)
            {
                formattedBinary = " " + formattedBinary;
            }
        }
        return formattedBinary;
    }
    private void PrintWithErrorHighlight(RichTextBox box, string binaryText, uint syndrome, bool isSyndromeBox = false)
    {
        box.Text = binaryText;

        if(syndrome == 0)
        {
            box.SelectAll();
            box.SelectionColor = fgGreen;
            return;
        }

        if (isSyndromeBox)
        {
            box.SelectAll();
            box.SelectionColor = Color.Orange;
            return;
        }

        int spacesToSkip = ((int)syndrome - 1) /4;
        int errorIndex = binaryText.Length - ((int)syndrome + spacesToSkip);

        if(errorIndex >= 0 && errorIndex < binaryText.Length)
        {
            box.SelectAll();
            box.SelectionColor = fgGreen;

            box.Select(errorIndex, 1);
            box.SelectionColor = Color.White;
            box.SelectionBackColor = Color.Red;
        }
    }

    private void ButtonEvent(object? sender, EventArgs e)
    {
        string data_in_string = _data_in_text.Text; string data_in_length_string = _data_in_length_text.Text;

        if(uint.TryParse(data_in_string, out uint data) && byte.TryParse(data_in_length_string, out byte len))
        {
            ulong c_memory_in; ulong c_memory_out;
            uint c_data_old; uint c_data_new;
            uint c_f_old; uint c_f_new;
            uint c_syndrome; uint c_total_length;

            uint data_out = Hamming.run(data, len,
                            out c_memory_in, out c_memory_out,
                            out c_data_old, out c_data_new,
                            out c_f_old, out c_f_new,
                            out c_syndrome, out c_total_length);
            _memory_in_text.Text = ToBinary(c_memory_in, (int)c_total_length);
             _data_old_text.Text = ToBinary(c_data_old, (int)len);
            _data_new_text.Text = ToBinary(c_data_new, (int)len);
            _f_old_text.Text = ToBinary(c_f_old, (int)(c_total_length - len));
            _f_new_text.Text = ToBinary(c_f_new, (int)(c_total_length - len));

            _data_out_text.Text = ToBinary(data_out, (int)len);           

            PrintWithErrorHighlight(_memory_out_text, ToBinary(c_memory_out, (int)c_total_length), c_syndrome);
            PrintWithErrorHighlight(_syndrome_text, ToBinary(c_syndrome, (int)(c_total_length - len)), c_syndrome, true);

            _data_out_text.SelectAll();
            _data_out_text.SelectionColor = Color.Cyan;
        }
        else
        {
            MessageBox.Show("Please enter valid numeric values!", "Input Error");
        }
    }
}

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.Run(new Form0());
    }    
}