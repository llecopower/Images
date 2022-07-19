using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//HOME ALEX MAKE LINQ exercise

//To connect with SQL SERVER DB
using System.Data.SqlClient;

//DEAL WITH INPUT / OUPUT
using System.IO;

namespace Images
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //LOAD BUTTON
            OpenFileDialog OD = new OpenFileDialog();
            OD.FileName = "";
            OD.Filter = "Supported Images|*.jpg;*.jpeg;*.png";
            if (OD.ShowDialog() == DialogResult.OK)
                pictureBox1.Load(OD.FileName);
        }
        string connectionString = "Data Source=LAPTOP_ALEX;Initial Catalog=ImageDB;Integrated Security=True";
        private void button2_Click(object sender, EventArgs e)
        {
            //SAVE BUTTON
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand command = con.CreateCommand();
            var image = new ImageConverter().ConvertTo(pictureBox1.Image, typeof(Byte[]));
            command.Parameters.AddWithValue("@image", image);
            command.CommandText = "INSERT INTO ImageTable (image) VALUES (@image)";

            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Image was added");
            else
                MessageBox.Show("Image was NOT added");

            con.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //FETCH BUTTON

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand command = con.CreateCommand();
            con.Open();
            command.Parameters.AddWithValue("@id", textBox1.Text);
            command.CommandText = "SELECT * FROM ImageTable WHERE id = @id";
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                MemoryStream stream = new MemoryStream(reader.GetSqlBytes(1).Buffer);
                pictureBox2.Image = Image.FromStream(stream);
            }

            con.Close();


        }
    }
}
