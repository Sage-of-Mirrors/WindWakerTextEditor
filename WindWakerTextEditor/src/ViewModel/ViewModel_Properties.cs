using System.Windows.Data;

namespace WindWakerTextEditor.View
{
    public partial class ViewModel
    {
        public BMG LoadedTextFile
        {
            get { return m_loadedTextFile; }
            set
            {
                if (value != m_loadedTextFile)
                {
                    m_loadedTextFile = value;

                    NotifyPropertyChanged("LoadedTextFile");
                }
            }
        }

        public string WindowTitle
        {
            get { return m_loadedFileName + " - Wind Waker Text Editor"; }
            set { NotifyPropertyChanged("WindowTitle"); }
        }

        public BMC LoadedColorFile
        {
            get { return m_loadedColorFile; }
            set
            {
                if (value != m_loadedColorFile)
                {
                    m_loadedColorFile = value;

                    NotifyPropertyChanged("LoadedColorFile");
                }
            }
        }

        public Message SelectedMessage
        {
            get { return m_selectedMessage; }
            set
            {
                if (value != m_selectedMessage)
                {
                    m_selectedMessage = value;

                    NotifyPropertyChanged("SelectedMessage");
                }
            }
        }

        public bool IsCompressed
        {
            get { return m_isCompressed; }
            set
            {
                if (value != m_isCompressed)
                {
                    m_isCompressed = value;
                    NotifyPropertyChanged("IsCompressed");
                }
            }
        }

        public int ListboxSelectedIndex
        {
            get { return m_listboxSelectedIndex; }

            set
            {
                if (value != m_listboxSelectedIndex)
                {
                    m_listboxSelectedIndex = value;

                    NotifyPropertyChanged("ListboxSelectedIndex");
                }
            }
        }

        public bool IsDataLoaded
        {
            get { return m_isDataLoaded; }

            set
            {
                if (value != m_isDataLoaded)
                {
                    m_isDataLoaded = value;

                    NotifyPropertyChanged("IsDataLoaded");
                }
            }
        }

        public CollectionViewSource ColViewSource
        {
            get { return m_colViewSource; }
            set
            {
                if (value != m_colViewSource)
                {
                    m_colViewSource = value;

                    NotifyPropertyChanged("ColViewSource");
                }
            }
        }

        public string SearchFilter
        {
            get
            { return m_searchFilter; }

            set
            {
                m_searchFilter = value;

                if (!string.IsNullOrEmpty(SearchFilter))
                    AddFilter();

                ColViewSource.View.Refresh();

                NotifyPropertyChanged("SearchFilter");
            }
        }

        public int TextBoxPos
        {
            get { return m_textBoxPos; }
            set
            {
                if (value != m_textBoxPos)
                {
                    m_textBoxPos = value;
                    NotifyPropertyChanged("TextBoxPos");
                }
            }
        }

        public System.Windows.Visibility ErrorLabelVisible
        {
            get { return m_errorLabelVisible; }
            set
            {
                if (m_errorLabelVisible != value)
                {
                    m_errorLabelVisible = value;
                    NotifyPropertyChanged("ErrorLabelVisible");
                }
            }
        }

        public string ErrorLabelContent
        {
            get { return m_errorLabelContent; }
            set
            {
                if (m_errorLabelContent != value)
                {
                    m_errorLabelContent = value;
                    NotifyPropertyChanged("ErrorLabelContent");
                }
            }
        }
    }
}
