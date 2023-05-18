using System.Configuration;

namespace StoragePower{
    public class StoragePower1
    {
        protected string plan1 = "";
        protected string plan2 = "";
        protected string planName1 = "plan1";
        protected string planName2 = "plan2";
        public StoragePower1()
        {
            getPlansConfig();
        }


        //записать план 1 в хранилище
        public void setPlan1(string planId){
            setPlan(this.planName1, planId);
        }

        //записать план 2 в хранилище
        public void setPlan2(string planId){
            setPlan(this.planName2, planId);
        }

        //получить планы из хранилища
        public string[] getPlansConfig(){
            plan1 = ConfigurationManager.AppSettings[planName1] ?? "";
            plan2 = ConfigurationManager.AppSettings[planName2] ?? "";
            string[] plans = {plan1,plan2};
            return plans;
        }


        //записать планы в хранилище
        private string[] setPlan(string planName, string planId){
            // Получаем текущую конфигурацию
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // Обновляем или добавляем значения в секцию appSettings
            config.AppSettings.Settings[planName].Value = planId;
            // Сохраняем изменения в файл конфигурации
            config.Save(ConfigurationSaveMode.Modified);
            // Обновляем конфигурацию в памяти
            ConfigurationManager.RefreshSection("appSettings");

            return getPlansConfig();
        }
    }
}