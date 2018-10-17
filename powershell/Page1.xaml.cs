using System.Windows;
using System.Windows.Controls;
using System.Management.Automation;
using System.Management.Automation.Runspaces;


namespace powershell
{
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string command = @in.Text;
            @out.Text += command + "\r\n";
            RunspaceConfiguration rspacecfg = RunspaceConfiguration.Create();
            Runspace rspace = RunspaceFactory.CreateRunspace(rspacecfg);
            rspace.Open();
            Pipeline pipeline = rspace.CreatePipeline();
            pipeline.Commands.AddScript(command);
            pipeline.InvokeAsync();
            while (pipeline.PipelineStateInfo.State == PipelineState.Running || pipeline.PipelineStateInfo.State == PipelineState.Stopping)
            {
                System.Threading.Thread.Sleep(50);
            }
            foreach (object item in pipeline.Output.ReadToEnd())
            {
                if (item != null)
                    @out.Text += item.ToString() + "\r\n";
            }
            foreach (object item in pipeline.Error.ReadToEnd())
            {
                if (item != null)
                    @out.Text += item.ToString() + "\r\n";
            }
        }
    }
}
