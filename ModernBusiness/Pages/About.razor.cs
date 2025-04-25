namespace ModernBusiness.Pages
{
    public partial class About
    {
        private string title = "우리는 are a modern business committed to excellence and innovation.";
        private string subTitle = "Our mission is to provide top-notch services and products that meet the needs of our clients.";

        public string Title 
        {
            get => title;
            set => title = value;
        }
        public string SubTitle
        {
            get => subTitle;
            set => subTitle = value;
        }

        protected override void OnInitialized()
        {
            // Initialization logic can go here
            subTitle = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
