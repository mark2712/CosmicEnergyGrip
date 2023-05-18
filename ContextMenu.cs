namespace ContextMenu{
    public class ContextMenu1
    {
        public ContextMenuStrip? contextMenu;
        public ContextMenuStrip create()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            this.contextMenu = contextMenu;
            return contextMenu;
        }

        //создать пункт в меню
        public void createItem(string text, EventHandler func)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = text;
            menuItem.Click += func;
            contextMenu!.Items.Add(menuItem);
        }
    }
}