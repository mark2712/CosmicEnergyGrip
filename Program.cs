using System.Text;

public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        //Регистрация провайдера кодировок для корректной работы кодировок чтобы нормально вывести текст из консоли cmd
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Form1 form = new Form1();
        
        Icon appIcon = new Icon("img/ico0.ico");
        form.Icon = appIcon;
        
        Application.Run(form);
    }
}