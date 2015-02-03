using System;

namespace Genbox.Presentation.Windows.Models
{
    public class HistoryRecord : Model
    {
        private string _text;
        private DateTime _timestamp;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set
            {
                _timestamp = value;
                OnPropertyChanged();
            }
        }
    }
}