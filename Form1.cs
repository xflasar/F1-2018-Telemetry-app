using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace UDP
{
    public partial class Form1 : Form
    {
        PacketHeader pHeader = new PacketHeader();
        CarTelemetryData pCarTelemetryData = new CarTelemetryData();
        CarStatus pCarStatus = new CarStatus();
        List<double> SpeedData = new List<double>();
        double[] SpData;
        Series ser2;

        List<Chart> charts = new List<Chart>();

        // Temp for data
        int lastSpeedNumber = 0;
        int numSpeed = 0;



        UdpClient client = new UdpClient(20777);
        IAsyncResult ar_ = null;
        string msgLea = "";
        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(255, 32, 30, 45);
            SpeedData.Add(0);
            SpData = SpeedData.ToArray();
            timer1.Start();
            ser2 = chart1.Series[0];
            ser2.Name = "Speed";
            //ser2.ChartType = SeriesChartType.Line;
            StyleChart(chart1, "Speed", 0, 350, new int[] { 255,32,26,71}, new int[] { 255,0,255,255});
            StyleChart(chart2, "RPM", 0, 17000, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart3, "Brake", 0, 100, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart4, "Throttle", 0, 100, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart5, "Tire Temperature Front Left", 0, 100, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart6, "Tire Temperature Front Right", 0, 100, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart7, "Tire Temperature Back Left", 0, 100, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart8, "Tire Temperature Back Right", 0, 100, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart9, "Brake Temperature Front Left", 0, 100, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart10, "Brake Temperature Front Right", 0, 100, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart11, "Brake Temperature Back Left", 0, 100, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart12, "Brake Temperature Back Right", 0, 100, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            //chart1.DataSource = SpeedData;
            //chart1.DataBind();
            // 
            charts.Add(chart1);
            charts.Add(chart2);
            charts.Add(chart3);
            charts.Add(chart4);
            charts.Add(chart5);
            charts.Add(chart6);
            charts.Add(chart7);
            charts.Add(chart8);
            charts.Add(chart9);
            charts.Add(chart10);
            charts.Add(chart11);
            charts.Add(chart12);

        }

        private void StyleChart(Chart chart, string title, int min, int max, int[] backColor, int[] borderColor)
        {

            // Basic style
            chart.BackColor = Color.FromArgb(backColor[0], backColor[1], backColor[2], backColor[3]);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineColor = Color.Black;
            chart.Width = 300;
            chart.Height = 150;

            // Legend (Not needed now)

            // Chart area
            chart.ChartAreas[0].Area3DStyle.Enable3D = false;
            chart.ChartAreas[0].Area3DStyle.WallWidth = 0;
            chart.ChartAreas[0].BackColor = Color.FromArgb(100, Color.Black);

            // Axis
            chart.ChartAreas[0].AxisX.LineColor = Color.Red;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
            chart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8F);

            chart.ChartAreas[0].AxisY.LineColor = Color.Red;
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chart.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
            chart.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 8F);

            chart.ClientSize = new Size(chart.Size.Width - 10, chart.Size.Height - 10);
            
            if(chart.Size.Width <= 100 || chart.Size.Height <= 100)
            {
                chart.ChartAreas[0].Position.Width = 15;
                chart.ChartAreas[0].Position.Height = 15;
            }


            //
            
            // Title
            chart.Titles.Add(new Title(title));
            chart.Legends.Clear();
            chart.ChartAreas[0].AxisY.Minimum = min;
            chart.ChartAreas[0].AxisY.Maximum = max;
            
            chart.Series[0].BorderColor = Color.FromArgb(borderColor[0], borderColor[1], borderColor[2], borderColor[3]);
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartListening();
        }

        private void MoveGraph(Chart chart)
        {
            if(chart.Series[0].Points.Count() > 500)
            {
                chart.ChartAreas[0].AxisX.Minimum = chart.ChartAreas[0].AxisX.Minimum + 1;
            }
        }

        private void StartListening()
        {
            ar_ = client.BeginReceive(Receive, new object());
            
            // Updating tires surface temperature
            tireSurfTempFL.BeginInvoke((Action)delegate { tireSurfTempFL.Text = "Surf Temps: " + pCarTelemetryData.GetTiresSurfaceTemperature1().ToString() + " °C"; });
            tireSurfTempFR.BeginInvoke((Action)delegate { tireSurfTempFR.Text = "Surf Temps: " + pCarTelemetryData.GetTiresSurfaceTemperature2().ToString() + " °C"; });
            tireSurfTempBL.BeginInvoke((Action)delegate { tireSurfTempBL.Text = "Surf Temps: " + pCarTelemetryData.GetTiresSurfaceTemperature3().ToString() + " °C"; });
            tireSurfTempBR.BeginInvoke((Action)delegate { tireSurfTempBR.Text = "Surf Tempse: " + pCarTelemetryData.GetTiresSurfaceTemperature4().ToString() + " °C"; });

            // Updating tires inner temperature
            tireInnerTempFL.BeginInvoke((Action)delegate { tireInnerTempFL.Text = "Inner Temps: " + pCarTelemetryData.GetTiresInnerTemperature1().ToString() + " °C"; });
            tireInnerTempFR.BeginInvoke((Action)delegate { tireInnerTempFR.Text = "Inner Temps: " + pCarTelemetryData.GetTiresInnerTemperature2().ToString() + " °C"; });
            tireInnerTempBL.BeginInvoke((Action)delegate { tireInnerTempBL.Text = "Inner Temps: " + pCarTelemetryData.GetTiresInnerTemperature3().ToString() + " °C"; });
            tireInnerTempBR.BeginInvoke((Action)delegate { tireInnerTempBR.Text = "Inner Temps: " + pCarTelemetryData.GetTiresInnerTemperature4().ToString() + " °C"; });

            // Updating tires pressure
            tirePressureFL.BeginInvoke((Action)delegate { tirePressureFL.Text = "Pressure: " + pCarTelemetryData.GetTiresPressure1().ToString() + " PSI"; });
            tirePressureFR.BeginInvoke((Action)delegate { tirePressureFR.Text = "Pressure: " + pCarTelemetryData.GetTiresPressure2().ToString() + " PSI"; });
            tirePressureBL.BeginInvoke((Action)delegate { tirePressureBL.Text = "Pressure: " + pCarTelemetryData.GetTiresPressure3().ToString() + " PSI"; });
            tirePressureBR.BeginInvoke((Action)delegate { tirePressureBR.Text = "Pressure: " + pCarTelemetryData.GetTiresPressure4().ToString() + " PSI"; });

            // Updating brake temperature
            brakeTempFL.BeginInvoke((Action)delegate { brakeTempFL.Text = "Brake Temps: " + pCarTelemetryData.GetBrakesTemperature1().ToString() + " °C"; });
            brakeTempFR.BeginInvoke((Action)delegate { brakeTempFR.Text = "Brake Temps: " + pCarTelemetryData.GetBrakesTemperature2().ToString() + " °C"; });
            brakeTempBL.BeginInvoke((Action)delegate { brakeTempBL.Text = "Brake Temps: " + pCarTelemetryData.GetBrakesTemperature3().ToString() + " °C"; });
            brakeTempBR.BeginInvoke((Action)delegate { brakeTempBR.Text = "Brake Temps: " + pCarTelemetryData.GetBrakesTemperature4().ToString() + " °C"; });



            
            chart1.BeginInvoke((Action)delegate
            {
                lblSpeed.Text = pCarTelemetryData.GetSpeed().ToString();
                chart1.Series[0].Points.AddY(pCarTelemetryData.GetSpeed());
                lblRPM.Text = pCarTelemetryData.GetEngineRPM().ToString();
                chart2.Series[0].Points.AddY(pCarTelemetryData.GetEngineRPM());
                lblThrottle.Text = pCarTelemetryData.GetThrottle().ToString() + " %";
                chart3.Series[0].Points.AddY(pCarTelemetryData.GetBrake());
                lblBrake.Text = pCarTelemetryData.GetBrake().ToString() + " %";
                chart4.Series[0].Points.AddY(pCarTelemetryData.GetThrottle());

                chart1.Series[0].Points[chart1.Series[0].Points.Count() - 1].BorderColor = Color.FromArgb(255, (int)(pCarTelemetryData.GetSpeed() / 1.2f), 255 - (int)(pCarTelemetryData.GetSpeed() / 1.2f), 0);

                pictureBoxBrakeFL.BackColor = GetColorTempBrake(pCarTelemetryData.GetBrakesTemperature1());
                pictureBoxBrakeFR.BackColor = Color.FromArgb(255, pCarTelemetryData.GetBrakesTemperature2() / 5, 255 - (pCarTelemetryData.GetBrakesTemperature1() / 5), 0);
                pictureBoxBrakeBL.BackColor = Color.FromArgb(255, pCarTelemetryData.GetBrakesTemperature3() / 5, 255 - (pCarTelemetryData.GetBrakesTemperature1() / 5), 0);
                pictureBoxBrakeBR.BackColor = Color.FromArgb(255, pCarTelemetryData.GetBrakesTemperature4() / 5, 255 - (pCarTelemetryData.GetBrakesTemperature1() / 5), 0);

                // Move graph so it stays indented and visible
                foreach (var item in charts)
                {
                    MoveGraph(item);
                }
            });
        }

        private Color GetColorTempBrake(int temps)
        {
            int r = 0;
            int b = 0;
            int g = 0;
            int colorInt = 0;
            if (0 <= temps && temps <= 433) {
                b = (int)(255 - temps/1.7f);
                g = (int)(temps /1.7f);
            }
            else if (433 < temps && temps <= 866)
            {
                g = (int)(255 - temps/3.4f);
                r = (int)(temps/3.4f);
            }
            else if (866 < temps)
            {
                g =(int) (255 - temps/5.1f);
                r = (int)(temps/5.1f);
            }
            return Color.FromArgb(255,r, g, b);
        }
        private void Receive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 20777);
            byte[] bytes = client.EndReceive(ar, ref ip);
            //Console.WriteLine(bytes.Length);
            /*if(bytes.Length == 1085)
            {
                Console.WriteLine("Format: " + BitConverter.ToUInt16(bytes, 0)); // 2 Byte
                Console.WriteLine("Version: " + BitConverter.ToString(bytes, 2, 1)); // 1 Byte
                Console.WriteLine("PacketID: " + BitConverter.ToString(bytes, 3, 1)); // 1 Byte
                Console.WriteLine("sessionUID: " + BitConverter.ToUInt64(bytes, 4)); // 8 Byte
                Console.WriteLine("sessionTime: " + BitConverter.ToSingle(bytes, 12)); // 4 Byte
                Console.WriteLine("frameIdentifier: " + BitConverter.ToInt32(bytes, 16)); // 4 Byte
                Console.WriteLine("PlayerCarIndex: " + BitConverter.ToString(bytes, 20,1)); // 1 Byte
            }*/
            GetDataIntoSeriazle(bytes);
            string message = Encoding.ASCII.GetString(bytes);
            int offset = 0;
            int remaining = bytes.Length;
            byte[] byteArray = Encoding.UTF8.GetBytes(message);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            while (remaining > 0)
            {
                int read = stream.Read(bytes, offset, remaining);
                if(read <= 0)
                {
                    
                }
                //Console.WriteLine(read);
                remaining -= read;
                offset += read;
            }
            //Console.WriteLine("From {0} received: {1} ", ip.Address.ToString(), message);
            msgLea = message;
            //richTextBox1.Text += message;
            StartListening();
        }

        private void GetDataIntoSeriazle(byte[] data)
        {
            //PacketHeader
            pHeader.SetPacketFormat(BitConverter.ToUInt16(data, 0)); // 2B
            pHeader.SetPacketVersion(int.Parse(BitConverter.ToString(data, 2, 1))); // 1B
            pHeader.SetPacketID(int.Parse(BitConverter.ToString(data, 3, 1))); // 1B
            pHeader.SetSessionUID(BitConverter.ToUInt64(data, 4)); // 8B
            pHeader.SetSessionTime(BitConverter.ToSingle(data, 12)); // 4B
            pHeader.SetFrameIdentifier(BitConverter.ToInt32(data, 16)); // 4B
            pHeader.SetPlayerCarIndex(data[20]); // 1B


            switch (data.Length)
            {
                case 1085: // CarTelemetry Packet
                    
                    
                    pCarTelemetryData.SetSpeed(System.BitConverter.ToUInt16(data, 21 + (pHeader.GetPlayerCarIndex() * 53)));                      // Speed of car in kilometres per hour
                    //SpeedData.Add(System.BitConverter.ToUInt16(data, 21 + (pHeader.GetPlayerCarIndex() * 53)));
                    //numSpeed = System.BitConverter.ToUInt16(data, 21 + (pHeader.GetPlayerCarIndex() * 53));
                    

                    pCarTelemetryData.SetThrottle(data[23 + (pHeader.GetPlayerCarIndex() * 53)]);
                    pCarTelemetryData.SetSteer(data[24 + (pHeader.GetPlayerCarIndex() * 53)]);
                    pCarTelemetryData.SetBrake(data[25 + (pHeader.GetPlayerCarIndex() * 53)]);
                    pCarTelemetryData.SetClutch(data[26 + (pHeader.GetPlayerCarIndex() * 53)]);
                    pCarTelemetryData.SetGear(data[27 + (pHeader.GetPlayerCarIndex() * 53)]);
                    pCarTelemetryData.SetEngineRPM(System.BitConverter.ToUInt16(data, 28 + (pHeader.GetPlayerCarIndex() * 53)));

                    pCarTelemetryData.SetDRS(data[30 + (pHeader.GetPlayerCarIndex() * 53)]);
                    pCarTelemetryData.SetRevLightsPerc(data[31 + (pHeader.GetPlayerCarIndex() * 53)]);
                        // 4 data
                    pCarTelemetryData.SetBrakesTemperature4(System.BitConverter.ToUInt16(data, 32 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetBrakesTemperature3(System.BitConverter.ToUInt16(data, 34 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetBrakesTemperature2(System.BitConverter.ToUInt16(data, 36 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetBrakesTemperature1(System.BitConverter.ToUInt16(data, 38 + (pHeader.GetPlayerCarIndex() * 53)));
                        // 4 data
                    pCarTelemetryData.SetTiresSurfaceTemperature4(System.BitConverter.ToUInt16(data, 40 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetTiresSurfaceTemperature3(System.BitConverter.ToUInt16(data, 42 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetTiresSurfaceTemperature2(System.BitConverter.ToUInt16(data, 44 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetTiresSurfaceTemperature1(System.BitConverter.ToUInt16(data, 46 + (pHeader.GetPlayerCarIndex() * 53)));
                        // 4 data
                    pCarTelemetryData.SetTiresInnerTemperature4(System.BitConverter.ToUInt16(data, 48 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetTiresInnerTemperature3(System.BitConverter.ToUInt16(data, 50 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetTiresInnerTemperature2(System.BitConverter.ToUInt16(data, 52 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetTiresInnerTemperature1(System.BitConverter.ToUInt16(data, 54 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetEngineTemperature(System.BitConverter.ToUInt16(data, 56 + (pHeader.GetPlayerCarIndex() * 53)));
                        // 4 data
                    pCarTelemetryData.SetTiresPressure4(System.BitConverter.ToUInt16(data, 58 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetTiresPressure3(System.BitConverter.ToUInt16(data, 60 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetTiresPressure2(System.BitConverter.ToUInt16(data, 62 + (pHeader.GetPlayerCarIndex() * 53)));
                    pCarTelemetryData.SetTiresPressure1(System.BitConverter.ToUInt16(data, 64 + (pHeader.GetPlayerCarIndex() * 53)));

                    SpData = SpeedData.ToArray();
                    break;
                case 1082:
                    /*
                    for (int i = 0; i < 20; i++)
                    {
                        if (!System.BitConverter.ToBoolean(data, 23 +(i*53)))
                        {
                            Console.WriteLine("racenumber is : " + System.BitConverter.ToString(data, 26 + (i*53)));
                        }
                    }*/
                    break;
                case 1061: // CarStatus Packet
                    pCarStatus.SetTractionControl(data[21 + (pHeader.GetPlayerCarIndex() * 52)]);

                    pCarStatus.SetAntiLockBrakes(data[22 + (pHeader.GetPlayerCarIndex() * 52)]);

                    pCarStatus.SetFuelMix(data[23 + (pHeader.GetPlayerCarIndex() * 52)]);

                    // 24 FOR BRAKEBIASfront
                    // 25 for pitlimiterstatus

                    pCarStatus.SetFuelInTank(System.BitConverter.ToSingle(data, 26 + (pHeader.GetPlayerCarIndex() * 52)));

                    pCarStatus.SetFuelCapacity(System.BitConverter.ToSingle(data, 30 + (pHeader.GetPlayerCarIndex() * 52)));

                    pCarStatus.SetMaxRPM(System.BitConverter.ToUInt16(data, 34 + (pHeader.GetPlayerCarIndex() * 52)));

                    pCarStatus.SetIdleRPM(System.BitConverter.ToUInt16(data, 36 + (pHeader.GetPlayerCarIndex() * 52)));

                    pCarStatus.SetMaxGears(data[38 + (pHeader.GetPlayerCarIndex() * 52)]);

                    pCarStatus.SetDRSAllowed(data[39 + (pHeader.GetPlayerCarIndex() * 52)]);

                    pCarStatus.SetTireWear1(data[40 + (pHeader.GetPlayerCarIndex() * 52)]);
                    pCarStatus.SetTireWear2(data[41 + (pHeader.GetPlayerCarIndex() * 52)]);
                    pCarStatus.SetTireWear3(data[42 + (pHeader.GetPlayerCarIndex() * 52)]);
                    pCarStatus.SetTireWear4(data[43 + (pHeader.GetPlayerCarIndex() * 52)]);

                    pCarStatus.SetTireCompound(data[44 + (pHeader.GetPlayerCarIndex() * 52)]);

                    pCarStatus.SetTireDamage1(data[45 + (pHeader.GetPlayerCarIndex() * 52)]);
                    pCarStatus.SetTireDamage2(data[46 + (pHeader.GetPlayerCarIndex() * 52)]);
                    pCarStatus.SetTireDamage3(data[47 + (pHeader.GetPlayerCarIndex() * 52)]);
                    pCarStatus.SetTireDamage4(data[48 + (pHeader.GetPlayerCarIndex() * 52)]);

                    // 49 frontleftwingdamage 
                    // 50 front rightwingdamage
                    // 51 rearwingdamage
                    // 52 enginedamage
                    // 53 gearboxdamage
                    // 54 exhaustDamage
                    // 55 vehicleFiaFlags

                    pCarStatus.SetERSStoredEnergy(System.BitConverter.ToSingle(data, 56 + (pHeader.GetPlayerCarIndex() * 52)));

                    pCarStatus.SetERSDeployMode(data[60 + (pHeader.GetPlayerCarIndex() * 52)]);

                    pCarStatus.SetERSHarvestedEnergyThisLapKinetic(System.BitConverter.ToSingle(data, 61 + (pHeader.GetPlayerCarIndex() * 52)));
                    pCarStatus.SetERSHarvestedEnergyThisLapHeat(System.BitConverter.ToSingle(data, 65 + (pHeader.GetPlayerCarIndex() * 52)));
                    pCarStatus.SetERSDeployedThisLap(System.BitConverter.ToSingle(data, 69 + (pHeader.GetPlayerCarIndex() * 52)));

                    pCarStatus.PrintOut();
                    break;
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 500;
            //formsPlot1.Reset();
            //formsPlot1.Render();
        }
    }
}
