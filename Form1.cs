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
        int maxBrakeTemp = 1400;


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

            // Chart for 4 tire temps
            CreateChartAreas(chart5, 4);
            CreateChartAreas(chart6, 4);



            // Chart for 4 brake temps



            //ser2.ChartType = SeriesChartType.Line;
            StyleChart(chart1, 300, 150,"Speed", 0, 350, 1, new int[] { 255,32,26,71}, new int[] { 255,0,255,255});
            StyleChart(chart2, 300, 150, "RPM", 0, 17000, 1, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart3, 300, 150, "Brake", 0, 100, 1, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart4, 300, 150, "Throttle", 0, 100, 1, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart5, 240, 220, "Tire Temperature", 0, 140, 4, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            StyleChart(chart6, 240, 220, "Brake Temperature", 0, 1400, 4, new int[] { 255, 32, 26, 71 }, new int[] { 255, 0, 255, 255 });
            //chart1.DataSource = SpeedData;
            //chart1.DataBind();
            // 
            charts.Add(chart1);
            charts.Add(chart2);
            charts.Add(chart3);
            charts.Add(chart4);
            charts.Add(chart5);
            charts.Add(chart6);
            charts.Add(chart6);


            // Finishing Style chart
            chart5.Series[0].Name = "Tire Surf Temps Front Left";
            chart5.Series[1].Name = "Tire Surf Temps Front Right";
            chart5.Series[2].Name = "Tire Surf Temps Back Left";
            chart5.Series[3].Name = "Tire Surf Temps Back Right";

            chart6.Series[0].Name = "Brake Temps Front Left";
            chart6.Series[1].Name = "Brake Temps Front Right";
            chart6.Series[2].Name = "Brake Temps Back Left";
            chart6.Series[3].Name = "Brake Temps Back Right";
        }

        // Create Chart Area for TireTemps Chart
        private void CreateChartAreas(Chart chart, int numOfChartAreas)
        {
            List<ChartArea> chartAreaList = new List<ChartArea>();
            chart.ChartAreas.Clear();
            
            for (int i = 0; i < numOfChartAreas; i++)
            {
                ChartArea area = new ChartArea();
                area.AxisX.Enabled = AxisEnabled.False;
                //area.AxisX.Interval = 50;
                //area.AxisX.Maximum = 400;
                chartAreaList.Add(area);
            }


            foreach (var item in chartAreaList)
            {
                
                chart.ChartAreas.Add(item);
            }
        }

        private void StyleChart(Chart chart, int width, int height, string title, int min, int max, int numSeries, int[] backColor, int[] borderColor)
        {

            // Basic style
            chart.BackColor = Color.FromArgb(backColor[0], backColor[1], backColor[2], backColor[3]);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineColor = Color.Black;
            chart.Width = width;
            chart.Height = height;

            // Legend (Not needed now)
            foreach (var item in chart.ChartAreas)
            {
                
                // Chart area
                item.Area3DStyle.Enable3D = false;
                item.Area3DStyle.WallWidth = 0;
                item.BackColor = Color.FromArgb(100, Color.Black);

                // Axis
                item.AxisX.LineColor = Color.Red;
                item.AxisX.MajorGrid.Enabled = true;
                item.AxisX.MinorGrid.Enabled = false;
                item.AxisX.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
                item.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8F);
                //item.AxisX.Maximum = 400;
                
                item.AxisY.LineColor = Color.Red;
                item.AxisY.MajorGrid.Enabled = true;
                item.AxisY.MinorGrid.Enabled = false;
                item.AxisY.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
                item.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 8F);
                
                item.AxisY.Minimum = min;
                item.AxisY.Maximum = max;
            }

            // Title
            chart.Titles.Add(new Title(title));
            chart.Legends.Clear();
            chart.Series.Clear();
            for (int i = 0; i < numSeries; i++)
            {
                Series series = new Series();
                chart.Series.Add(series);
                chart.Series[i].ChartArea = chart.ChartAreas[i].Name;
            }
            foreach (var item in chart.Series)
            {
                item.BorderColor = Color.FromArgb(borderColor[0], borderColor[1], borderColor[2], borderColor[3]);
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartListening();
        }

        private void MoveGraph(Chart chart)
        {
            foreach (var serie in chart.Series)
            {
                double min = chart.ChartAreas[chart.ChartAreas.IndexOf(serie.ChartArea)].AxisX.Minimum;
                double max = chart.ChartAreas[chart.ChartAreas.IndexOf(serie.ChartArea)].AxisX.Maximum;
                if (serie.Points.Count() >= 400 && (max - min ) >= 400)
                {
                    chart.ChartAreas[chart.ChartAreas.IndexOf(serie.ChartArea)].AxisX.Minimum = chart.ChartAreas[chart.ChartAreas.IndexOf(serie.ChartArea)].AxisX.Minimum + 1;
                }
            }
        }

        private void StartListening()
        {
            ar_ = client.BeginReceive(Receive, new object());
            if (pHeader.GetPacketFormat() == 2018)
            {

                // Updating tires surface temperature
                tireSurfTempFL.BeginInvoke((Action)delegate { tireSurfTempFL.Text = "Surf Temps: " + pCarTelemetryData.GetTiresSurfaceTemperature1().ToString() + " °C"; });
                tireSurfTempFR.BeginInvoke((Action)delegate { tireSurfTempFR.Text = "Surf Temps: " + pCarTelemetryData.GetTiresSurfaceTemperature2().ToString() + " °C"; });
                tireSurfTempBL.BeginInvoke((Action)delegate { tireSurfTempBL.Text = "Surf Temps: " + pCarTelemetryData.GetTiresSurfaceTemperature3().ToString() + " °C"; });
                tireSurfTempBR.BeginInvoke((Action)delegate { tireSurfTempBR.Text = "Surf Temps: " + pCarTelemetryData.GetTiresSurfaceTemperature4().ToString() + " °C"; });

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
                    chart1.Series[0].Points[chart1.Series[0].Points.Count() - 1].BorderColor = Color.FromArgb(255, (int)(pCarTelemetryData.GetSpeed() / 1.2f), 255 - (int)(pCarTelemetryData.GetSpeed() / 1.2f), 0);

                    lblRPM.Text = pCarTelemetryData.GetEngineRPM().ToString();
                    chart2.Series[0].Points.AddY(pCarTelemetryData.GetEngineRPM());
                    chart2.ChartAreas[0].AxisY.Minimum = pCarStatus.GetIdleRPM();
                    chart2.ChartAreas[0].AxisY.Maximum = pCarStatus.GetMaxRPM();

                    lblThrottle.Text = pCarTelemetryData.GetThrottle().ToString() + " %";
                    chart3.Series[0].Points.AddY(pCarTelemetryData.GetBrake());

                    lblBrake.Text = pCarTelemetryData.GetBrake().ToString() + " %";
                    chart4.Series[0].Points.AddY(pCarTelemetryData.GetThrottle());

                    //Tires chart
                    
                    chart5.Series[0].Points.AddY(pCarTelemetryData.GetTiresSurfaceTemperature1());

                    chart5.Series[1].Points.AddY(pCarTelemetryData.GetTiresSurfaceTemperature2());

                    chart5.Series[2].Points.AddY(pCarTelemetryData.GetTiresSurfaceTemperature3());

                    chart5.Series[3].Points.AddY(pCarTelemetryData.GetTiresSurfaceTemperature4());

                    
                    // There should be automatic increse in max temps -> Brakes and tires into array
                    // Brake chart
                    
                    chart6.Series[0].Points.AddY(pCarTelemetryData.GetBrakesTemperature1());

                    chart6.Series[1].Points.AddY(pCarTelemetryData.GetBrakesTemperature2());

                    chart6.Series[2].Points.AddY(pCarTelemetryData.GetBrakesTemperature3());

                    chart6.Series[3].Points.AddY(pCarTelemetryData.GetBrakesTemperature4());


                    pictureBoxBrakeFL.BackColor = GetColorTempBrake(pCarTelemetryData.GetBrakesTemperature1());
                    pictureBoxBrakeFR.BackColor = GetColorTempBrake(pCarTelemetryData.GetBrakesTemperature2());
                    pictureBoxBrakeBL.BackColor = GetColorTempBrake(pCarTelemetryData.GetBrakesTemperature3());
                    pictureBoxBrakeBR.BackColor = GetColorTempBrake(pCarTelemetryData.GetBrakesTemperature4());

                // Move graph so it stays indented and visible
                foreach (var item in charts)
                    {
                        MoveGraph(item);
                    }
                });
            }
        }

        private Color GetColorTempBrake(int temps)
        {
            int r = 0;
            int b = 0;
            int g = 0;
            /*
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
            }*/
            return Color.FromArgb(255,r, g, b);
        }
        private void Receive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 20777);
            byte[] bytes = client.EndReceive(ar, ref ip);
            
            GetDataIntoSeriazle(bytes);
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
