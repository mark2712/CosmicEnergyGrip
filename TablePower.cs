namespace TablePower{
    public class TablePower1
    {
        public DataGridView table;
        public TablePower1(){
            table = new DataGridView();
            //table.Dock = DockStyle.Fill;
            table.Dock = DockStyle.None;
            table.AutoGenerateColumns = false;
            table.RowHeadersVisible = false;
            table.AllowUserToAddRows = false;
            table.DefaultCellStyle.Padding = new Padding(6);
            table.ReadOnly = true;
            table.AutoSize = true;

            //танцы с бубном чтобы настроить автовысоту, ширину, отступы по нормальному
            table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            table.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            table.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders); // Пересчет высоты заголовка строки
            //table.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False; //запрет переноса текста в заголовких таблицы
            table.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            table.BorderStyle = BorderStyle.None; //убрать рамку

            // Создание столбцов таблицы
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Название";

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "Id";
            idColumn.HeaderText = "id в windows";

            DataGridViewTextBoxColumn usedColumn = new DataGridViewTextBoxColumn();
            usedColumn.DataPropertyName = "Used";
            usedColumn.HeaderText = "Используется";

            DataGridViewImageColumn plan1Column = new DataGridViewImageColumn();
            plan1Column.DataPropertyName = "Plan1";
            plan1Column.HeaderText = "Основной план питания";
            plan1Column.ImageLayout = DataGridViewImageCellLayout.Zoom;
            plan1Column.ToolTipText = "Нажмите на ячейку чтобы поставить план питания как основной";

            DataGridViewImageColumn plan2Column = new DataGridViewImageColumn();
            plan2Column.DataPropertyName = "Plan2";
            plan2Column.HeaderText = "Доп. план питания";
            plan2Column.ImageLayout = DataGridViewImageCellLayout.Zoom;
            plan2Column.ToolTipText = "Нажмите на ячейку чтобы поставить план питания как дополнительный";

            //дополнительно отформатировать и изменить ячейки
            table.CellFormatting += (sender, e) =>
            {
                if (e != null && e.CellStyle != null && table.Columns[e.ColumnIndex] == usedColumn && e.RowIndex >= 0)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //выровнять текст по центру
                }

                //добавить корректные подсказки по наведению мыши
                if (e != null && e.CellStyle != null && e.RowIndex >= 0)
                {
                    if(table.Columns[e.ColumnIndex] == plan1Column)
                    {
                        table.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "Нажмите чтобы поставить этот план питания как основной план питания";
                    }
                    if(table.Columns[e.ColumnIndex] == plan2Column)
                    {
                        table.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "Нажмите чтобы поставить этот план питания как дополнительный план питания";
                    }
                }
            };

            //Добавление столбцов в таблицу
            table.Columns.Add(nameColumn);
            table.Columns.Add(idColumn);
            table.Columns.Add(usedColumn);
            table.Columns.Add(plan1Column);
            table.Columns.Add(plan2Column);
        }


        //создать строку в таблице
        public void AddRowToDataGridView(string name, string id, bool used, Image plan1, Image plan2)
        {
            //поставить галочку в ячейку с активным планом питания
            string usedText = "";
            if(used){ usedText = "✔"; }

            //создать ячейки
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(table, name, id, usedText, plan1, plan2);
            table.Rows.Add(row);
        }
    }
}