
using System.Windows.Input;
using System.Windows;

namespace WindWakerTextEditor.View
{
    public partial class ViewModel
    {
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

        public ICommand OnRequestCloseCurrentFile
        {
            get { return new RelayCommand(x => CloseCurrentFile(), x => LoadedTextFile != null); }
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

        public void CloseCurrentFile()
        {
            m_colViewSource.Source = null;
            LoadedTextFile = null;
            LoadedColorFile = null;
            m_loadedDirRoot = null;
            m_selectedMessage = null;

            IsDataLoaded = false;
        }

        public void About()
        {
            MessageBox.Show("This is a text editor for The Legend of Zelda: The Wind Waker.\n\nYou can search for specific messages using the search box,\nand you can search by Item ID or Message ID by\ntyping in itemid:<num> or msgid:<num>, respectively.", "About");
        }

        private void OpenWebPage(string address)
        {
            System.Diagnostics.Process.Start(address);
        }

        private void OnInsert(string code)
        {
            m_selectedMessage.TextData = m_selectedMessage.TextData.Insert(m_textBoxPos, string.Format("\\<{0}>", code));
        }
    }
}
