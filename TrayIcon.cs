namespace TrayIcon{
    public class TrayIcon1
    {
        public NotifyIcon trayIcon;

        public string[] iconUrls = {
            "img/ico0.ico",
            "img/ico1.ico",
            "img/ico2.ico",
        };
        
        public TrayIcon1(Form1 form)
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Сменить план питания";
            trayIcon.Icon = new System.Drawing.Icon(iconUrls[0]);
            trayIcon.Visible = true;
            form.trayIcon = trayIcon;

            //Добавляем обработчик для разворачивания формы при клике на значок трея
            trayIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    form.Show();
                    form.WindowState = FormWindowState.Normal;
                }
            };

            //При сворачивании окна свернуть в трей
            form.Resize += (sender, e) =>
            {
                if (form.WindowState == FormWindowState.Minimized)
                {
                    form.Hide();
                }
                else if (form.WindowState == FormWindowState.Normal)
                {
                    form.Show();
                }
            };

            //Добавляем обработчик для разворачивания формы при клике на значок трея
            trayIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    form.ShowPowerPlans();
                    form.Show();
                    form.WindowState = FormWindowState.Normal;
                }
            };
        }

        //Сменить иконку
        public void changeTrayIcon(string url_icon)
        {
            Icon newIcon = new Icon(url_icon);
            trayIcon.Icon = newIcon;
        }
    }
}