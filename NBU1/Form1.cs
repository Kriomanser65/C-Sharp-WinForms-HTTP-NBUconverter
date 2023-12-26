using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;

namespace NBU1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string selectedCurrency = comboBox1.SelectedItem?.ToString();
            double amount = 0;
            if (double.TryParse(textBox1.Text, out amount) && selectedCurrency != null)
            {
                double result = await ConvertCurrency(selectedCurrency, amount);
                label1.Text = $"{amount} UAH = {result} {selectedCurrency}";
            }
            else
            {
                MessageBox.Show("Please enter the correct amount and select the currency.");
            }
        }
        private async Task<double> ConvertCurrency(string targetCurrency, double amount)
        {
            string url = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<Root> currencies = JsonConvert.DeserializeObject<List<Root>>(jsonResponse);
                    foreach (var currency in currencies)
                    {
                        if (currency.cc == targetCurrency)
                        {
                            double rate = currency.rate;
                            return amount / rate;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Not Found. Code Status: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public class Root
        {
            public int r030 { get; set; }
            public string txt { get; set; }
            public double rate { get; set; }
            public string cc { get; set; }
            public string exchangedate { get; set; }
        }

    }
}
