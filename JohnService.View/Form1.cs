using JohnService.BO;
namespace JohnService.View
{
    public partial class Form1 : Form
    {
        public Form1()
        { //9994-0814
            InitializeComponent();
            ServiceReader.ReadServiceOrder();
            txtServiceOrder.Lines = ServiceReader.ServiceRequest.Select(x => x.Key + ": " + x.Value.ToString()).ToArray();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}