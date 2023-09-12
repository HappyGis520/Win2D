namespace MapControl
{
    public partial class Form1 : Form
    {

        MapControlViewModel _MapControlVM = null;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            _MapControlVM = new MapControlViewModel(this.pictureBox);
        }
    }
}