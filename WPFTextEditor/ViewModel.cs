using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;
using Microsoft.Win32;
using GameFormatReader.Common;
using System.IO;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows;
using WArchiveTools.Archives;
using WArchiveTools.FileSystem;

namespace WPFTextEditor
{
    /// <summary>
    /// Null-to-Bool converter. If an object is null, Convert returns false.
    /// </summary>
    public class NullToFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType,
          object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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

        public string WindowTitle
        {
            get { return m_loadedFileName + " - GC Zelda Text Editor"; }
            set { NotifyPropertyChanged("WindowTitle"); }
        }

        private string m_loadedFileName;

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
                    try
                    {
                        if (parsed[1] != "" && src.DisplayItemId != (ItemIDValue)Convert.ToByte(parsed[1]))
                            e.Accepted = false;
                    }
                    catch (Exception ex)
                    {
                        // Overflow. Snap the search filter to 255.
                        if (ex.GetType() == typeof(OverflowException))
                        {
                            SearchFilter = string.Format("itemid:{0}", 255);
                        }
                        // Format exception - letters and symbols instead of numbers. We'll just... delete... them...?
                        else if (ex.GetType() == typeof(FormatException))
                        {
                            SearchFilter = string.Format("itemid:{0}", parsed[1].Remove(parsed[1].Length - 1));
                        }
                    }
                }
            }

            else if (SearchFilter.Contains("msgid"))
            {
                string[] parsed = SearchFilter.Split(':');

                if (parsed.Count() >= 2)
                {
                    // Oh boy, parsing
                    try
                    {
                        if (parsed[1] != "" && Convert.ToInt32(src.MessageId) != Convert.ToInt32(parsed[1]))
                            e.Accepted = false;
                    }
                    // Something fucked up. Let's catch the exception
                    catch(Exception ex)
                    {
                        // Overflow. So we'll snap the search filter to the highest ID there is.
                        if (ex.GetType() == typeof(OverflowException))
                        {
                            SearchFilter = string.Format("msgid:{0}", (int)(GetHighestID() - 1));
                        }
                        // Format exception - letters and symbols instead of numbers. We'll just... delete... them...?
                        else if (ex.GetType() == typeof(FormatException))
                        {
                            SearchFilter = string.Format("msgid:{0}", parsed[1].Remove(parsed[1].Length - 1));
                        }
                    }
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
                VirtualFilesystemDirectory sourceDir = WArchiveTools.ArchiveUtilities.LoadArchive(m_openFile.FileName);

                foreach (VirtualFilesystemNode node in sourceDir.Children)
                {
                    EndianBinaryReader reader;

                    if (node.Name == "zel_00")
                    {
                        VirtualFilesystemFile bmgFile = node as VirtualFilesystemFile;

                        reader = new EndianBinaryReader(bmgFile.File.GetData(), Endian.Big);

                        LoadedTextFile = new BmgTextFile(reader);

                        m_listboxSelectedIndex = 0;

                        IsSearchByText = true;

                        m_loadedFileName = m_openFile.FileName;

                        WindowTitle = m_loadedFileName;

                        ColViewSource = new CollectionViewSource();

                        ColViewSource.Source = LoadedTextFile.MessageList;

                        SelectedMessage = LoadedTextFile.MessageList[0];

                        IsDataLoaded = true;
                    }

                    else if (node.Name == "color")
                    {
                        VirtualFilesystemFile bmcFile = node as VirtualFilesystemFile;

                        reader = new EndianBinaryReader(bmcFile.File.GetData(), Endian.Big);

                        LoadedColorFile = new BmcColorFile(reader);
                    }
                }

                /*
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

                            m_loadedFileName = m_openFile.FileName;

                            ColViewSource = new CollectionViewSource();

                            ColViewSource.Source = LoadedTextFile.MessageList;

                            SelectedMessage = LoadedTextFile.MessageList[0];

                            IsDataLoaded = true;
                        }

                        if (m_arc.Nodes[i].Entries[j].Name.Contains(".bmc"))
                        {
                            LoadedColorFile = new BmcColorFile(new EndianBinaryReader(m_arc.Nodes[i].Entries[j].Data, Endian.Big));
                        }
                    }
                }*/
            }
        }

        private void SaveAs()
        {
            SaveFileDialog save = new SaveFileDialog();

            save.DefaultExt = ".arc";
            save.Filter = "Archive (*.arc)|*.arc";

            if (save.ShowDialog() == true)
            {
                PathExport(save.FileName);

                m_loadedFileName = save.FileName;
                WindowTitle = save.FileName;
            }
        }

        private void PathExport(string path)
        {
            VirtualFilesystemDirectory rootDir = new VirtualFilesystemDirectory("archive");

            // Write text
            MemoryStream bmgFile = new MemoryStream();
            EndianBinaryWriter bmgWriter = new EndianBinaryWriter(bmgFile, Endian.Big);
            LoadedTextFile.Export(bmgWriter);
            VirtualFilesystemFile bmg = new VirtualFilesystemFile("zel_00", ".bmg", new VirtualFileContents(bmgFile.ToArray()));
            rootDir.Children.Add(bmg);

            // Write color
            MemoryStream bmcFile = new MemoryStream();
            EndianBinaryWriter bmcWriter = new EndianBinaryWriter(bmcFile, Endian.Big);
            LoadedColorFile.Export(bmcWriter);
            VirtualFilesystemFile bmc = new VirtualFilesystemFile("color", ".bmc", new VirtualFileContents(bmcFile.ToArray()));
            rootDir.Children.Add(bmc);

            WArchiveTools.ArchiveUtilities.WriteArchive(path, rootDir);
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
            get { return new RelayCommand(x => PathExport(m_loadedFileName), x => LoadedTextFile != null); }
        }

        public ICommand OnRequestSaveFileAs
        {
            get { return new RelayCommand(x => SaveAs(), x => LoadedTextFile != null); }
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
            get { return new RelayCommand(x => OnInsert((string)x), x => m_selectedMessage != null); }
        }

        public ICommand OnRequestAddMessage
        {
            get { return new RelayCommand(x => AddMessage(), x => LoadedTextFile != null); }
        }

        public ICommand OnRequestRemoveMessage
        {
            get { return new RelayCommand(x => RemoveMessage(), x => LoadedTextFile != null); }
        }

        public ICommand OnRequestOpenWebPage
        {
            get { return new RelayCommand(x => OpenWebPage((string)x), x => true); }
        }

        private void AddMessage()
        {
            short largestID = GetHighestID();
            Message newMes = new Message(largestID);
            LoadedTextFile.MessageList.Add(newMes);

            SelectedMessage = newMes;
            ColViewSource.View.Refresh();
        }

        private void RemoveMessage()
        {
            int currentIndex = LoadedTextFile.MessageList.IndexOf(SelectedMessage); // Get index of selected message
            LoadedTextFile.MessageList.Remove(SelectedMessage); // Remove selected message

            if (LoadedTextFile.MessageList.Count == 0)
                AddMessage(); // We will always have at least one message in the list
            else if (currentIndex == 0)
                SelectedMessage = LoadedTextFile.MessageList[0]; // Have new first message selected if we deleted the original
            else
                SelectedMessage = LoadedTextFile.MessageList[currentIndex - 1]; // Have message before the deleted one selected
        }

        private short GetHighestID()
        {
            short highest = 0;

            foreach (Message mes in LoadedTextFile.MessageList)
            {
                if (highest < mes.MessageId + 1)
                    highest = (short)(mes.MessageId + 1);
            }

            return highest;
        }

        private void OnInsert(string code)
        {
            m_selectedMessage.TextData = m_selectedMessage.TextData.Insert(m_textBoxPos, string.Format("<{0}>", code));
        }

        private void OpenWebPage(string address)
        {
            System.Diagnostics.Process.Start(address);
        }
    }
}
