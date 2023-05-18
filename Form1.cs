using Power;
using TrayIcon;
using ContextMenu;
using TablePower;
using HotkeyManager;

public class Form1 : Form
{
    public HotkeyManager1 hotkeyManager;
    public Power1 power;
    public TrayIcon1 trayIcon1;
    public NotifyIcon trayIcon;
    public ContextMenu1 contextMenu1;
    public ContextMenuStrip contextMenu;
    public DataGridView? table;

    public Form1(){
        //горячие клавиши Ctrl+Shift+E для смены плана питания
        hotkeyManager = new HotkeyManager1();
        hotkeyManager.HotkeyManagerStart(this);
        hotkeyManager.HotkeyPressed += HotkeyManager_HotkeyPressed;

        //создать класс отвечающий за планы питания и операции с ними
        power = new Power1();
        if(power.PowerPlans == null || power.PowerPlans.Length < 2){
            MessageBox.Show("У вас меньше 2 планов электропитания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Создаем NotifyIcon (иконка в трее)
        trayIcon1 = new TrayIcon1(this);
        trayIcon = trayIcon1.trayIcon ?? throw new ArgumentNullException(nameof(trayIcon));

        //Создаем контекстное меню для иконки в трее
        contextMenu1 = new ContextMenu1();
        contextMenu = contextMenu1.create();
        trayIcon.ContextMenuStrip = contextMenu;// Привязываем контекстное меню к иконке трея
        createContextMenu(trayIcon); //создать пункты контекстного меню

        //создать содержимое формы
        FormLayout();

        //Добавляем обработчик для сворачивания формы при закрытии окна (осторожно, станет НЕВОЗМОЖНО ЗАКРЫТЬ ПРИЛОЖЕНИЕ через крестик!)
        this.FormClosing += (sender, e) =>
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        };
        
        //событие при закрытии окна
        this.FormClosing += Form1_FormClosing;
    }

    //создать пункты контекстного меню
    public void createContextMenu(NotifyIcon trayIcon){
        contextMenu1.createItem("Закрыть приложение", (sender, e) => {
            hotkeyManager.FormClosing();
            trayIcon!.Dispose();
            Application.Exit();
        });
        
        PowerPlan[] parsedPlans = power.GetPowerPlans();

        //в контекстом меню создать пункты-планы питания
        foreach (PowerPlan plan in parsedPlans)
        {
            contextMenu1.createItem(plan.Name, (sender, e) => {
                power.SetPowerPlan(plan.Id);
                ShowPowerPlans();
                changeTrayIcon();
            });
        }
    }


    //Сменить иконку в трее
    public void changeTrayIcon()
    {
        int number = 0;
        string[] plansConfig =  power.getPlansConfig();
        PowerPlan[] powerPlans = power.GetPowerPlans();
        foreach (PowerPlan plan in powerPlans)
        {
            if(plan.Use)
            {
                if(plansConfig[0] == plan.Id)
                {
                    number = 1;
                }
                if(plansConfig[1] == plan.Id)
                {
                    number = 2;
                }
            }
        }

        trayIcon1!.changeTrayIcon(trayIcon1.iconUrls[number]);
        //power.powerConsoleWrite(powerPlans);
    }


    //создать содержимое формы
    public void FormLayout()
    {
        this.Name = "Form1";
        this.Text = "Cosmic Eenergy Grip - Контроль планов электропитания";
        this.Size = new System.Drawing.Size(900, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Resize += Form1_Resize;

        DataGridView table = this.ShowPowerPlans();

        changeTrayIcon();

        //изменить размер окна
        Size tableSize = table.PreferredSize; //Получить размер таблицы
        this.ClientSize = new Size(tableSize.Width, tableSize.Height); //Установить новый размер формы
    }


    //Вывести список планов электропитания
    public DataGridView ShowPowerPlans()
    {
        this.AutoScroll = true;
        this.BackColor = Color.White;
        
        this.Controls.Remove(table); //удаляем таблицу если она уже есть
        TablePower1 tablePower1 = new TablePower1();
        table = tablePower1.table;
        this.Controls.Add(table); // Добавление таблицу на форму
        table.BackgroundColor = this.BackColor; //цвет фона как у формы

        string[] plansConfig =  power.getPlansConfig();

        PowerPlan[] powerPlans = power.GetPowerPlans();
        foreach (PowerPlan plan in powerPlans)
        {
            string planIco1 = trayIcon1.iconUrls[0];
            string planIco2 = trayIcon1.iconUrls[0];
            if(plan.Id == plansConfig[0]){ planIco1 = trayIcon1.iconUrls[1]; }
            if(plan.Id == plansConfig[1]){ planIco2 = trayIcon1.iconUrls[2]; }
            Image plan1Image = Image.FromFile(planIco1);
            Image plan2Image = Image.FromFile(planIco2);
            tablePower1.AddRowToDataGridView(plan.Name, plan.Id, plan.Use, plan1Image, plan2Image);
        }

        table.CellClick += (sender, e) =>
        {
            // Проверяем, что был кликнут столбец plan1 или plan2
            bool plan1OrPlan2 = e.ColumnIndex == table.Columns[3].Index || e.ColumnIndex == table.Columns[4].Index;
            if (e.RowIndex >= 0)
            {
                // Получаем значение ячейки id
                string idPowerValue = table.Rows[e.RowIndex].Cells[1].Value.ToString() ?? "";
                if(plan1OrPlan2){
                    if(e.ColumnIndex == 3){ power.setPlan1(idPowerValue); }
                    if(e.ColumnIndex == 4){ power.setPlan2(idPowerValue); }
                    ShowPowerPlans();
                    changeTrayIcon();
                }
                if(e.ColumnIndex == 2){ 
                    power.SetPowerPlan(idPowerValue);
                    changeTrayIcon();
                    ShowPowerPlans();
                }
            }
        };

        return table;
    }


    //событие нажатии клавиши
    private void HotkeyManager_HotkeyPressed(object? sender, EventArgs e)
    {
        Console.WriteLine("e");
        PowerPlan[] powerPlans = power.GetPowerPlans();
        PowerPlan nowUsePlan;
        foreach (PowerPlan plan in powerPlans)
        {
            if(plan.Use){
                nowUsePlan = plan;
                string[] plansConfig =  power.getPlansConfig();
                if(nowUsePlan.Id == plansConfig[0])
                {
                    power.SetPowerPlan(plansConfig[1]);
                }
                else
                {
                    power.SetPowerPlan(plansConfig[0]);
                }
                ShowPowerPlans();
                changeTrayIcon();
            }
        }
    }

    //необходимо чтобы сработало нажатие клавиш
    protected override void WndProc(ref Message m)
    {
        hotkeyManager.ProcessHotkey(m);
        base.WndProc(ref m);
    }


    private void Form1_Resize(object? sender, EventArgs e)
    {
        int left = (this.ClientSize.Width - table!.Width) / 2;
        int top = (this.ClientSize.Height - table!.Height) / 2;
        // Установить отступы для таблицы
        table!.Left = left < 0 ? 0 : left;
        table!.Top = top < 0 ? 0 : top;;
    }


    //тут можно сделать что-либо при закрытии приложения
    public void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        //trayIcon!.Dispose();
    }
}