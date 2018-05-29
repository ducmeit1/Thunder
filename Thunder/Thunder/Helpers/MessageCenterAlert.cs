namespace Thunder.Helpers
{
    public class MessageCenterAlert
    {
        public MessageCenterAlert()
        {
        }

        public MessageCenterAlert(string title, string message, string cancel = "OK")
        {
            Title = title;
            Message = message;
            Cancel = cancel;
        }

        public string Title
        {
            get;
            set;
        }

        public string Message
        {
            set;
            get;
        }

        public string Cancel
        {
            get;
            set;
        }
    }
}
