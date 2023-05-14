using JohnService.BO;
using JohnService.DAL;
namespace JohnService.View
{
    public partial class Form1 : Form
    {
        public Form1()
        { 
            InitializeComponent();
            ServiceReader.ReadServiceOrder();
            txtServiceOrder.Lines = ServiceReader.ServiceRequest.Select(x => x.Key + ": " + x.Value.ToString()).ToArray();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            Connection.Connect("ISACNOTEBOOK", "JohnServicesDB", "sa", "senai@123");
            Connection.AddValues();
        }
    }
}