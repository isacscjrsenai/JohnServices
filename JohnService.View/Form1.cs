using JohnService.BO;
namespace JohnService.View
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var service = new ServiceReader();
            string ServiceOrderText ="";
            foreach (var item in service.ServiceRequest)
            {
                ServiceOrderText += item.Key.ToString();
                ServiceOrderText += ServiceOrderText + item.Value.ToString();
                //ServiceOrderText += "\n";
            }
            txtServiceOrder.Text = ServiceOrderText;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}