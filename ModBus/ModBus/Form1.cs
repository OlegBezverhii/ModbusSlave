using Modbus.Device;
using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace ModBus
{
    public partial class Form1 : Form
    {
        private SerialPort sp = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Для каждого ComboBoxa указываю значение по умолчанию, надо потом будет дать осмысленные названия
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 3;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Считываю название COM порта - пока сделан выбор, потом можно переделать в текстовый формат
            string comport = comboBox1.SelectedItem.ToString();
            //MessageBox.Show(comport);

            //Скорость передачи данных
            int velocity = Int32.Parse(comboBox2.SelectedItem.ToString());
            //MessageBox.Show(velocity.ToString());

            //Четность
            string Paritys = comboBox4.SelectedItem.ToString();
            //MessageBox.Show(Paritys);
            Parity OutParity = Parity.None;

            switch(Paritys)
            {
                case "Even":
                    OutParity = Parity.Even;
                    break;
                case "Mark":
                    OutParity = Parity.Mark;
                    break;
                case "None":
                    OutParity = Parity.None;
                    break;
                case "Odd":
                    OutParity = Parity.Odd;
                    break;
                case "Space":
                    OutParity = Parity.Space;
                    break;
            }

            //Битность (7,8)
            //int bytes = Int32.Parse(comboBox2.SelectedItem.ToString());
            int bytes = Convert.ToInt32(numericBits.Value);
            //MessageBox.Show(bytes.ToString());

            //Число стоповых битов
            string StopBit = comboBox5.SelectedItem.ToString();
            StopBits OutBits = StopBits.One;

            switch (StopBit)
            {
                case "One":
                    OutBits = StopBits.One;
                    break;
                case "OnePointFive":
                    OutBits = StopBits.OnePointFive;
                    break;
                case "Two":
                    OutBits = StopBits.Two;
                    break;
                case "None":
                    OutBits = StopBits.None;
                    break;
            }


            try
            {
                sp = new SerialPort(comport, velocity, OutParity, bytes, OutBits);
                sp.Open();
                MessageBox.Show("Порт открыт");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sp.Close();
            MessageBox.Show("Порт закрыт");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Считываю Адрес устройства, к которому мы подсоединяемся
                byte SlaveID = Convert.ToByte(numericAdress.Value);
                ushort startAdress = 0;
                ushort numOfPoints = 1;

                //int adress = Convert.ToInt32(numericAdress.Value);
                //MessageBox.Show(adress.ToString());

                //ModbusSerialSlave slave = ModbusSerialSlave.CreateRtu(SlaveID, sp);
                /*string[] strArr = textBox1.Text.Split(',');
                float[] floatArr = new float[strArr.Length];
                for (int i=0; i< strArr.Length; i++)
                {
                   floatArr[i] = float.Parse(strArr[i]);
                }

                ushort[] registers = mod*/


                ModbusSerialMaster master = ModbusSerialMaster.CreateRtu(sp);
                ushort[] holding_register = master.ReadHoldingRegisters(SlaveID, startAdress, numOfPoints);
                MessageBox.Show(holding_register.ToString());
                //Console.WriteLine(holding_register);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
