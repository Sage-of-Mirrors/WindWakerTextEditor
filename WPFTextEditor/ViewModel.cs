using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;
using Microsoft.Win32;
using GameFormatReader.Common;
using GameFormatReader.GCWii.Binaries.GC;
using System.IO;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows;

namespace WPFTextEditor
{
    public class ViewModel : INotifyPropertyChanged
    {
        public BmgTextFile LoadedTextFile
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

        private BmgTextFile m_loadedTextFile;

        public BmcColorFile LoadedColorFile
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

        private BmcColorFile m_loadedColorFile;

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

        private Message m_selectedMessage;

        public string SearchParameter
        {
            get { return m_searchParameter; }

            set
            {
                if (value != m_searchParameter)
                {
                    m_searchParameter = value;

                    NotifyPropertyChanged("SearchParameter");

                    Search();
                }
            }
        }

        private string m_searchParameter;

        public bool IsSearchByText
        {
            get { return m_isSearchByText; }

            set
            {
                if (value != m_isSearchByText)
                {
                    m_isSearchByText = value;

                    NotifyPropertyChanged("IsSearchByText");
                }
            }
        }

        private bool m_isSearchByText;

        private int m_previousSearchIndex;

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

        private int m_listboxSelectedIndex;

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

        private bool m_isDataLoaded;

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

        private CollectionViewSource m_colViewSource;

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

        private string m_searchFilter;

        private void AddFilter()
        {
            ColViewSource.Filter -= new FilterEventHandler(Filter);
            ColViewSource.Filter += new FilterEventHandler(Filter);
        }

        private void Filter(object sender, FilterEventArgs e)
        {
            // see Notes on Filter Methods:
            var src = e.Item as Message;

            if (src == null)
                e.Accepted = false;

            else if (SearchFilter.Contains("itemid"))
            {
                string[] parsed = SearchFilter.Split(':');

                if (parsed.Count() >= 2)
                {
                    if (parsed[1] != "" && src.DisplayItemId != (ItemIDValue)Convert.ToByte(parsed[1]))
                        e.Accepted = false;
                }
            }

            else if (SearchFilter.Contains("msgid"))
            {
                string[] parsed = SearchFilter.Split(':');

                if (parsed.Count() >= 2)
                {
                    if (parsed[1] != "" && Convert.ToInt32(src.MessageId) != Convert.ToInt32(parsed[1]))
                        e.Accepted = false;
                }
            }

            else if (src.TextData != null && !src.TextData.Contains(SearchFilter))// here is FirstName a Property in my YourCollectionItem
                e.Accepted = false;
        }

        private int m_textBoxPos;

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

        #region NotifyPropertyChanged Stuff

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion

        private void Open()
        {
            OpenFileDialog m_openFile = new OpenFileDialog();

            m_openFile.FileName = "bmgres";
            m_openFile.DefaultExt = ".arc";
            m_openFile.Filter = "arc files (*.arc)|*.arc";

            if (m_openFile.ShowDialog() == true)
            {
                RARC m_arc = new RARC(m_openFile.FileName);

                for (int i = 0; i < m_arc.Nodes.Count(); i++)
                {
                    for (int j = 0; j < m_arc.Nodes[i].Entries.Count(); j++)
                    {
                        if (m_arc.Nodes[i].Entries[j].Name.Contains(".bmg"))
                        {
                            LoadedTextFile = new BmgTextFile(new EndianBinaryReader(m_arc.Nodes[i].Entries[j].Data, Endian.Big));

                            m_listboxSelectedIndex = 0;

                            IsSearchByText = true;

                            ColViewSource = new CollectionViewSource();

                            ColViewSource.Source = LoadedTextFile.MessageList;

                            IsDataLoaded = true;
                        }

                        if (m_arc.Nodes[i].Entries[j].Name.Contains(".bmc"))
                        {
                            LoadedColorFile = new BmcColorFile(new EndianBinaryReader(m_arc.Nodes[i].Entries[j].Data, Endian.Big));
                        }
                    }
                }
            }
        }

        private void Save()
        {

        }

        public void Search()
        {
            if (SearchParameter == null)
                return;

            if (IsSearchByText)
            {
                if (m_previousSearchIndex >= LoadedTextFile.MessageList.Count)
                {
                    m_previousSearchIndex = -1;
                }

                string textSearch = Convert.ToString(SearchParameter).ToLower();

                for (int i = m_previousSearchIndex + 1; i < LoadedTextFile.MessageList.Count; i++)
                {
                    string searchedStringToLower = LoadedTextFile.MessageList[i].TextData.ToLower();

                    if (searchedStringToLower.Contains(textSearch))
                    {
                        ListboxSelectedIndex = i;

                        m_previousSearchIndex = i;

                        return;
                    }
                }

                if (System.Windows.MessageBox.Show("The string you entered wasn't found. Repeat search from the beginning?", "String not found", 
                    System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                {
                    m_previousSearchIndex = 0;

                    Search();
                }

                return;
            }

            else
            {
                try
                {
                    short searchedId = Convert.ToInt16(SearchParameter);

                    for (int i = 0; i < LoadedTextFile.MessageList.Count; i++)
                    {
                        if (LoadedTextFile.MessageList[i].MessageId == searchedId)
                        {
                            ListboxSelectedIndex = i;

                            return;
                        }
                    }

                    if (System.Windows.MessageBox.Show("The ID you entered wasn't found. Repeat search from the beginning?", "ID not found",
                        System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                    {
                        m_previousSearchIndex = 0;

                        Search();
                    }

                    return;
                }

                catch
                {
                    System.Windows.MessageBox.Show("A message ID can only include text.");
                }

            }
        }

        public void About()
        {
            MessageBox.Show("This is a text editor for The Legend of Zelda: The Wind Waker.\n\nYou can search for specific messages using the search box,\nand you can search by Item ID or Message ID by\ntyping in itemid:<num> or msgid:<num>, respectively.", "About");
        }

        public ICommand OnRequestOpenFile
        {
            get { return new RelayCommand(x => Open(), x => true); }
        }

        public ICommand OnRequestSaveFile
        {
            get { return new RelayCommand(x => Save(), x => LoadedTextFile != null); }
        }

        public ICommand OnRequestSearchMessages
        {
            get { return new RelayCommand(x => Search(), x => LoadedTextFile != null); }
        }

        public ICommand OnRequestAboutInfo
        {
            get { return new RelayCommand(x => About(), x => true); }
        }

        public ICommand InsertCommand
        {
            get { return new RelayCommand(x => OnInsert(x.ToString()), x => m_selectedMessage != null); }
        }

        private void OnInsert(string code)
        {
            m_selectedMessage.TextData = m_selectedMessage.TextData.Insert(m_textBoxPos, string.Format("<{0}>", code));
        }
    }
}
