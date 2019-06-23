using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Test1
{
	public partial class Form1 : Form
	{
		private string		glob_text;
		private bool		is_selected;
		private ComboBox[]	comboBoxes;
		private TextBox[]	textBoxes;

		public Form1()
		{
			InitializeComponent();

			openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
			saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
			foreach (TextBox textB in this.Controls.OfType<TextBox>())
			{
				textB.Click += new EventHandler(textB_Click);
			}
			this.is_selected = false;
			this.glob_text = null;
			this.textBoxes = new TextBox[4];
			textBoxes[0] = textBox2;
			textBoxes[1] = textBox3;
			textBoxes[2] = textBox4;
			textBoxes[3] = textBox5;
			this.comboBoxes = new ComboBox[4];
			comboBoxes[0] = comboBox1;
			comboBoxes[1] = comboBox2;
			comboBoxes[2] = comboBox3;
			comboBoxes[3] = comboBox4;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
				return;
			string filename = openFileDialog1.FileName;
			this.glob_text = System.IO.File.ReadAllText(filename, Encoding.GetEncoding(1251));
			textBox1.Text = this.glob_text;
			this.is_selected = false;
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (this.is_selected == false)
				this.glob_text = this.textBox1.Text;
		}

		private void textB_Click(object sender, EventArgs e)
		{
			this.textBox1.Text = this.glob_text;
			this.is_selected = false;
		}

		private String find_string(String[] raw_str, ComboBox[] comboBoxes, TextBox[] textBoxes)
		{
			String	res;
			bool	is_find;
			int		k;
			Boolean or_flag;
			Boolean and_falg;
			int		i;

			res = null;
			is_find = false;
			k = 0;
			while (k < raw_str.Length)
			{
				and_falg = true;
				or_flag = false;
				i = 0;
				while (i < 4)
				{
					if ((comboBoxes[i].SelectedIndex != -1) && (textBoxes[i].TextLength > 0))
					{
						if (comboBoxes[i].SelectedIndex == 0)
							and_falg = and_falg && raw_str[k].Contains(textBoxes[i].Text);
						else
							or_flag = or_flag || raw_str[k].Contains(textBoxes[i].Text);
					}
					i++;
				}
				is_find = and_falg || or_flag;
				if (is_find)
					res = res + raw_str[k] + "\r\n";
				k++;
			}
			return (res);
		}

		private Boolean check_sets(ComboBox[] comboBoxes, TextBox[] textBoxes)
		{
			int		i;
			Boolean	res;

			res = false;
			i = 0;
			while (i < 4)
			{
				if ((comboBoxes[i].SelectedIndex != -1) && (textBoxes[i].TextLength > 0))
					res = res || true;
				i++;
			}
			return (res);
		}

		void	whole_blovk()
		{
			int i;

			this.button1.Enabled = false;
			this.button2.Enabled = false;
			this.textBox1.Enabled = false;
			i = 0;
			while (i < 4)
			{
				this.textBoxes[i].Enabled = false;
				this.comboBoxes[i].Enabled = false;
				i++;
			}
		}

		void whole_unblovk()
		{
			int i;

			this.button1.Enabled = true;
			this.button2.Enabled = true;
			this.textBox1.Enabled = true;
			i = 0;
			while (i < 4)
			{
				this.textBoxes[i].Enabled = true;
				this.comboBoxes[i].Enabled = true;
				i++;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			int			i;
			String[]	some_str;

			this.is_selected = true;
			whole_blovk();
			if (this.glob_text != null)
			{

				if (check_sets(this.comboBoxes, this.textBoxes))
				{
					some_str = this.glob_text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
					this.textBox1.Text = find_string(some_str, this.comboBoxes, this.textBoxes);
				}
				else
					MessageBox.Show("Настройте хотя бы один из фильтров!");
			}
			else
				MessageBox.Show("Выберите файл!");
			whole_unblovk();
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.textBox1.AllowDrop = true;
			this.textBox1.DragEnter += new DragEventHandler(textBox1_DragEnter);
			textBox1.DragDrop += new DragEventHandler(textBox1_DragDrop);
		}

		private void textBox1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
				e.Effect = DragDropEffects.All;
			else
				e.Effect = DragDropEffects.None;
		}

		private void textBox1_DragDrop(object sender, DragEventArgs e)
		{
			string[]	strings;
			string		filename;
			string[]	parse;

			strings = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			filename = strings[0];
			parse = filename.Split('.');
			if (parse[parse.Length - 1] == "txt")
			{
				this.glob_text = System.IO.File.ReadAllText(filename, Encoding.GetEncoding(1251));
				textBox1.Text = this.glob_text;
				this.is_selected = false;
			}
			else
				MessageBox.Show("Неверный тип файла!");
		}
	}
}
