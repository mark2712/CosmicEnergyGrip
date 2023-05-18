using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.Diagnostics;
using StoragePower;

namespace Power{
    public class Power1 : StoragePower1
    {
        public PowerPlan[] PowerPlans = new PowerPlan[]{};
        public Power1() : base()
        {
            PowerPlans = GetPowerPlans();

            if(PowerPlans.Length >= 2){
                if(plan1 == ""){
                    setPlan1(PowerPlans[0].Id);
                }
                if(plan2 == ""){
                    setPlan2(PowerPlans[1].Id);
                }
            }
        }


        //сменить план питания
        public void SetPowerPlan(string powerPlanName)
        {
            var process = new Process();
            ProcessStartInfo startInfo = cmdProcessInfo($"/c powercfg /s \"{powerPlanName}\"");
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }


        //получить планы питания как массив строк
        public string[] GetPowerPlansStr()
        {
            ProcessStartInfo startInfo = cmdProcessInfo("/C powercfg -l");

            using (Process? process = Process.Start(startInfo))
            {
                string output = process!.StandardOutput.ReadToEnd();
                string[] lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                // Пропускаем первые две строки, так как они содержат заголовки
                string[] powerPlans = new string[lines.Length - 2];
                Array.Copy(lines, 2, powerPlans, 0, powerPlans.Length);

                return powerPlans;
            }
        }


        //получить планы питания как массив объектов PowerPlan
        public PowerPlan[] GetPowerPlans()
        {
            string[] powerPlans = GetPowerPlansStr();
            PowerPlan[] PowerPlans = new PowerPlan[powerPlans.Length];

            for (int i = 0; i < powerPlans.Length; i++)
            {
                string line = powerPlans[i];
                Match match = Regex.Match(line, @"GUID схемы питания: (.+)  \((.+)\)(\s\*)?");

                if (match.Success)
                {
                    PowerPlan powerPlan = new PowerPlan(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Success);
                    PowerPlans[i] = powerPlan;
                }
            }

            this.PowerPlans = PowerPlans;
            return PowerPlans;
        }


        //вывести в консоль планы питания
        public void powerConsoleWrite(PowerPlan[] powerPlans)
        {
            foreach (PowerPlan plan in powerPlans)
            {
                Console.WriteLine($"ID: {plan.Id}");
                Console.WriteLine($"Name: {plan.Name}");
                Console.WriteLine($"Use: {plan.Use}");
                Console.WriteLine();
            }
        }


        //создать данные для запуска cmd
        private ProcessStartInfo cmdProcessInfo(string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage)
            };
            return startInfo;
        }
    }


    public class PowerPlan
    {
        public string Id {get;}
        public string Name {get;}
        public bool Use {get;}

        public PowerPlan(string id, string name, bool use)
        {
            Id = id ?? "";
            Name = name ?? "";
            Use = use;
        }
    }
}