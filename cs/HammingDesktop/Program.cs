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
    private TextBox _memory_in_text, _memory_out_text, _data_old_text, _data_new_text, _f_old_text, _f_new_text, _syndrome_text, _data_out_text;
    private Button _data_in_button;


    public Form0()
    {
        this.Text = "Error Correction Simulator";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        int width = 250; int height = 40;

        /* INPUT */

        _data_in_label = new Label();
        _data_in_label.Text = "Data In";
        _data_in_label.Location = new Point(30, 40);
        _data_in_label.Size = new Size(width, height);
        this.Controls.Add(_data_in_label);

        _data_in_length_label = new Label();
        _data_in_length_label.Text = "Data Length";
        _data_in_length_label.Location = new Point(300, 40);
        _data_in_length_label.Size = new Size(width, height);
        this.Controls.Add(_data_in_length_label);

        _data_in_text = new TextBox();
        _data_in_text.Location = new Point(30, 80);
        _data_in_text.Size = new Size(width, height);
        this.Controls.Add(_data_in_text);

        _data_in_length_text = new TextBox();
        _data_in_length_text.Location = new Point(300, 80);
        _data_in_length_text.Size = new Size(width, height);
        this.Controls.Add(_data_in_length_text);

        _data_in_button = new Button();
        _data_in_button.Text = "Push";
        _data_in_button.Location = new Point(600, 80);
        _data_in_button.Size = new Size(width-100, height);
        _data_in_button.Click += ButtonEvent;
        this.Controls.Add(_data_in_button);

        /* OUTPUT */

        // MEMORY IN
        _memory_in_label = new Label();
        _memory_in_label.Text = "Memory In";
        _memory_in_label.Location = new Point(30, 120);
        _memory_in_label.Size = new Size(width, height);
        this.Controls.Add(_memory_in_label);

        _memory_in_text = new TextBox();
        _memory_in_text.Location = new Point(30, 160);
        _memory_in_text.Size = new Size(width, height);
        _memory_in_text.ReadOnly = true;
        this.Controls.Add(_memory_in_text);

        // MEMORY OUT
        _memory_out_label = new Label();
        _memory_out_label.Text = "Memory Out";
        _memory_out_label.Location = new Point(300, 120);
        _memory_out_label.Size = new Size(width, height);
        this.Controls.Add(_memory_out_label);

        _memory_out_text = new TextBox();
        _memory_out_text.Location = new Point(300, 160);
        _memory_out_text.Size = new Size(width, height);
        _memory_out_text.ReadOnly = true;
        this.Controls.Add(_memory_out_text);

        // DATA OLD
        _data_old_label = new Label();
        _data_old_label.Text = "Data Old";
        _data_old_label.Location = new Point(30, 200);
        _data_old_label.Size = new Size(width, height);
        this.Controls.Add(_data_old_label);

        _data_old_text = new TextBox();
        _data_old_text.Location = new Point(30, 240);
        _data_old_text.Size = new Size(width, height);
        _data_old_text.ReadOnly = true;
        this.Controls.Add(_data_old_text);

        // DATA NEW
        _data_new_label = new Label();
        _data_new_label.Text = "Data New";
        _data_new_label.Location = new Point(300, 200);
        _data_new_label.Size = new Size(width, height);
        this.Controls.Add(_data_new_label);

        _data_new_text = new TextBox();
        _data_new_text.Location = new Point(300, 240);
        _data_new_text.Size = new Size(width, height);
        _data_new_text.ReadOnly = true;
        this.Controls.Add(_data_new_text);

        // F OLD
        _f_old_label = new Label();
        _f_old_label.Text = "F Old";
        _f_old_label.Location = new Point(30, 280);
        _f_old_label.Size = new Size(width, height);
        this.Controls.Add(_f_old_label);

        _f_old_text = new TextBox();
        _f_old_text.Location = new Point(30, 320);
        _f_old_text.Size = new Size(width, height);
        _f_old_text.ReadOnly = true;
        this.Controls.Add(_f_old_text);

        // F NEW
        _f_new_label = new Label();
        _f_new_label.Text = "F New";
        _f_new_label.Location = new Point(300, 280);
        _f_new_label.Size = new Size(width, height);
        this.Controls.Add(_f_new_label);

        _f_new_text = new TextBox();
        _f_new_text.Location = new Point(300, 320);
        _f_new_text.Size = new Size(width, height);
        _f_new_text.ReadOnly = true;
        this.Controls.Add(_f_new_text);

        // SYNDROME
        _syndrome_label = new Label();
        _syndrome_label.Text = "Syndrome";
        _syndrome_label.Location = new Point(30, 360);
        _syndrome_label.Size = new Size(width, height);
        this.Controls.Add(_syndrome_label);
        
        _syndrome_text = new TextBox();
        _syndrome_text.Location = new Point(30, 400);
        _syndrome_text.Size = new Size(width, height);
        _syndrome_text.ReadOnly = true;
        this.Controls.Add(_syndrome_text);


        // DATA OUT
        _data_out_label = new Label();
        _data_out_label.Text = "Data Out";
        _data_out_label.Location = new Point(30, 440);
        _data_out_label.Size = new Size(width, height);
        this.Controls.Add(_data_out_label);

        _data_out_text = new TextBox();
        _data_out_text.Location = new Point(30, 480);
        _data_out_text.Size = new Size(width, height);
        _data_out_text.ReadOnly = true;
        this.Controls.Add(_data_out_text);

    }
    private string ToBinary(ulong value, int bitLength)
    {
        return Convert.ToString((long)value, 2).PadLeft(bitLength, '0');
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
            _memory_out_text.Text = ToBinary(c_memory_out, (int)c_total_length);
            _data_old_text.Text = ToBinary(c_data_old, (int)len);
            _data_new_text.Text = ToBinary(c_data_new, (int)len);
            _f_old_text.Text = ToBinary(c_f_old, (int)(c_total_length - len));
            _f_new_text.Text = ToBinary(c_f_new, (int)(c_total_length - len));
            _syndrome_text.Text = ToBinary(c_syndrome, (int)(c_total_length - len));

            _data_out_text.Text = ToBinary(data_out, (int)len);
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