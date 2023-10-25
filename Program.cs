
using System.Globalization;
using System.Text;

namespace CApp.UHCProcess
{
    class Program
    {
        public static StringBuilder Data = new StringBuilder();
        public static string validExtension = @"*.csv";
        public static string mainPath = @"C:\\Projects\\CApp.UHCProcess\IN\";
        public static string mainPathOutFiles = @"C:\\Projects\\CApp.UHCProcess\OUT\";
        public static string fileMassachussetts = "SSA-MA_HSN_dtl_CErrors.csv";
        public static string fileNewYork = "SSA-NY_PSS_dtl.csv";
        public static string fileConsolidate = "SSA-Consolidate_dtl.csv";
        public static string resultDate = string.Empty;
        public static string minorDate = string.Empty;
        public static string majorDate = string.Empty;

        /// <summary>
        /// POLICY NBR, 7.67E+11 = 767000000000                 - It's done.
        /// EE FIRST NAME        = Remove commas                - It's done.
        /// PAT LAST NAME        = Remove commas                - It's done. 
        /// SVC DATE             = Format date MMDDAAAA         - It's done.
        /// PMT DATE             = Format date MMDDAAAA         - It's done.
        /// Add Column STATE_CD                                 - It's done.
        /// Add Column MA for Massachussetts file               - It's done.
        /// Add Column NY for New York file                     - It's done.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            formatMassachussettsFile();     //Script task one
            formatNYFile();                 //Scripts task two.
            validateDatesFinalFileName();
        }

        public static bool ValidateHeader(string Headerline)
        {
            if (Headerline.Contains("POLICY NBR"))
                return true;

            return false;
        }

        public static void formatMassachussettsFile() 
        {
            string[] lines = File.ReadLines(mainPath + fileMassachussetts).ToArray();
            string headerLine = lines[0];

            if (ValidateHeader(headerLine))
            {
                int i = 0;
                using (var writer = new StreamWriter(File.OpenWrite(mainPathOutFiles + fileConsolidate)))
                using (var reader = new StreamReader(File.OpenRead(mainPath + fileMassachussetts)))
                {
                    var index = 0;
                    while (!reader.EndOfStream)
                    {
                        string line1 = string.Empty;
                        var line = reader.ReadLine().Replace(@"""", "");

                        if (index > 0)
                        {
                            var tmpLine = line.Split(",");
                            for (i = 0; i < tmpLine.Length; i++)
                            {
                                if(tmpLine[i].ToString() == string.Empty) i++;

                                if (tmpLine[i].Contains("+"))
                                {
                                    var r = Convert.ToString(double.Parse(tmpLine[i], CultureInfo.InvariantCulture));
                                    line1 = line1 + r + ',';
                                }
                                else if (i == 6 && tmpLine.Length == 19)
                                {
                                    if (tmpLine[i].Contains(" "))
                                    {
                                        line1 = line1 + tmpLine[i] + ',';
                                    }
                                    else
                                    {
                                        line1 = line1 + tmpLine[i] + ' ' + tmpLine[i + 1] + ',';
                                        i = i + 1;
                                    }
                                }
                                else if (i == 6 && tmpLine.Length == 18)
                                {
                                    line1 = line1 + tmpLine[i].Replace(",", " ") + ',';
                                }
                                else if (i == 13 && tmpLine.Length == 19)
                                {
                                    if (tmpLine[i].Length == 7)
                                    {
                                        resultDate = DateTime.ParseExact("0" + tmpLine[i], "MMddyyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                        line1 = line1 + resultDate + ',';
                                    }
                                    else
                                    {
                                        resultDate = DateTime.ParseExact(tmpLine[i], "MMddyyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                        line1 = line1 + resultDate + ',';
                                    }

                                    if (tmpLine[i + 1].Length == 7)
                                    {
                                        resultDate = DateTime.ParseExact("0" + tmpLine[i + 1], "MMddyyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                        line1 = line1 + resultDate + ',';
                                    }
                                    else
                                    {
                                        resultDate = DateTime.ParseExact(tmpLine[i + 1], "MMddyyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                        line1 = line1 + resultDate + ',';
                                    }
                                    i = i + 1;
                                }
                                else if (i == 12 && tmpLine.Length == 18)
                                {
                                    if (tmpLine[i].Length == 7)
                                    {
                                        resultDate = DateTime.ParseExact("0" + tmpLine[i], "MMddyyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                        line1 = line1 + resultDate + ',';
                                    }
                                    else
                                    {
                                        resultDate = DateTime.ParseExact(tmpLine[i], "MMddyyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                        line1 = line1 + resultDate + ',';
                                    }

                                    if (tmpLine[i + 1].Length == 7)
                                    {
                                        resultDate = DateTime.ParseExact("0" + tmpLine[i + 1], "MMddyyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                        line1 = line1 + resultDate + ',';
                                    }
                                    else
                                    {
                                        resultDate = DateTime.ParseExact(tmpLine[i + 1], "MMddyyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                        line1 = line1 + resultDate + ',';
                                    }
                                    i = i + 1;
                                }
                                else
                                {
                                    line1 = line1 + tmpLine[i] + ',';
                                }
                            }

                            if (i == tmpLine.Length && Path.GetFileName(mainPath + fileMassachussetts).Contains("MA")) { line1 = line1 + "MA"; }

                            line = line1;
                            writer.WriteLine(line);
                        }
                        else
                        {
                            line = line + ",STATE_CD";
                            writer.WriteLine(line);
                        }
                        index++;
                    }
                    //Console.WriteLine($"Proceso MA finalizado exitosamente con {index} lineas, por favor revise archivo consolidado.");
                }
            }
            else
            {
                //Console.WriteLine("Datos obligatorios no existen.");
            }
        }
        public static void formatNYFile()
        {
            string[] lines = File.ReadLines(mainPath + fileNewYork).ToArray();
            string headerLine = lines[0];
            int index = 0;

            if (ValidateHeader(headerLine))
            {
                int i = 0;
                var writer = new StreamWriter(mainPathOutFiles + fileConsolidate, true);
                using (var reader = new StreamReader(File.OpenRead(mainPath + fileNewYork)))
                {
                    while (!reader.EndOfStream)
                    {
                        string line1 = string.Empty;
                        string line = string.Empty;
                        line = reader.ReadLine().Replace(@"""", "");

                        if (index > 0)
                        {
                            var tmpLine = line.Split(",");
                            for (i = 0; i < tmpLine.Length; i++)
                            {
                                if (i != 13)
                                {
                                    if (tmpLine[i].Contains("+"))
                                    {
                                        var r = Convert.ToString(double.Parse(tmpLine[i], CultureInfo.InvariantCulture));
                                        line1 = line1 + r + ',';
                                    }
                                    else if (i == 6 && tmpLine.Length == 19)
                                    {
                                        if (tmpLine[i].Contains(" "))
                                        {
                                            line1 = line1 + tmpLine[i] + ',';
                                        }
                                        else
                                        {
                                            line1 = line1 + tmpLine[i] + ',';
                                        }
                                    }
                                    else if (i == 12 && tmpLine.Length == 19)
                                    {
                                        if (tmpLine[i].Length == 7)
                                        {
                                            string result = DateTime.ParseExact("0" + tmpLine[i], "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                            line1 = line1 + result + ',';
                                        }
                                        else
                                        {
                                            string result = DateTime.ParseExact(tmpLine[i], "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                            line1 = line1 + result + ',';
                                        }
                                    }
                                    else if (i == 14 && tmpLine.Length == 19)
                                    {
                                        if (tmpLine[i].Length == 7)
                                        {
                                            string result = DateTime.ParseExact("0" + tmpLine[i], "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                            line1 = line1 + result + ',';
                                        }
                                        else
                                        {
                                            string result = DateTime.ParseExact(tmpLine[i], "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MMddyyyy");
                                            line1 = line1 + result + ',';
                                        }
                                    }
                                    else
                                    {
                                        line1 = line1 + tmpLine[i] + ',';
                                    }
                                }
                            }

                            if (i == tmpLine.Length && Path.GetFileName(mainPath + fileNewYork).Contains("NY")) { line1 = line1 + "NY"; }
                            line = line1;
                            writer.WriteLine(line);
                        }
                        index++;
                    }
                }
                writer.Close();
                //Console.WriteLine($"Proceso NY finalizado exitosamente con: {index} lineas, por favor revise archivo consolidado.");
            }
            else
            {
                //Console.WriteLine("Datos obligatorios no existen.");
            }
        }


        /// <summary>
        /// Get minor date.         - It's done.
        /// Get major date.         - It's done.
        /// </summary>
        public static void validateDatesFinalFileName()
        {
            int index = 0;

            using (var reader = new StreamReader(File.OpenRead(mainPathOutFiles + fileConsolidate)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var tmpLine = line.Split(",");

                    if (index == 1)
                    {
                        minorDate = tmpLine[12];
                        majorDate = tmpLine[12];
                    }
                    if (index > 1) 
                    {
                        if (DateTime.ParseExact(tmpLine[12].ToString(), "MMddyyyy", CultureInfo.InvariantCulture)
                            < DateTime.ParseExact(minorDate, "MMddyyyy", CultureInfo.InvariantCulture))
                        {
                            minorDate = tmpLine[12].ToString();
                        }

                        if (DateTime.ParseExact(tmpLine[12].ToString(), "MMddyyyy", CultureInfo.InvariantCulture)
                            > DateTime.ParseExact(majorDate, "MMddyyyy", CultureInfo.InvariantCulture))
                        {
                            majorDate = tmpLine[12].ToString();
                        }
                    }
                    index++;
                }
            }
            //Console.WriteLine($"minorDate: {minorDate}");
            Console.WriteLine($"majorDate: {majorDate}");

            if (File.Exists(mainPathOutFiles + "MA_" + minorDate + "_" + majorDate + ".csv")) { File.Delete(mainPathOutFiles + "MA_" + minorDate + "_" + majorDate + ".csv"); }

            File.Move(mainPathOutFiles + fileConsolidate, mainPathOutFiles + "MA_" + minorDate +"_" + majorDate + ".csv");
        }
    }
}