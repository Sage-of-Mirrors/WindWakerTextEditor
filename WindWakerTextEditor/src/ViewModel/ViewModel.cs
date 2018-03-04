using System;
using System.Linq;
using System.Windows.Data;
using Microsoft.Win32;
using GameFormatReader.Common;
using System.IO;
using System.ComponentModel;
using WArchiveTools.FileSystem;

namespace WindWakerTextEditor.View
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        private BMG m_loadedTextFile;
        private BMC m_loadedColorFile;
        private Message m_selectedMessage;
        private CollectionViewSource m_colViewSource;

        private VirtualFilesystemDirectory m_loadedDirRoot;

        private string m_loadedFileName;
        private string m_searchFilter;
        private bool m_isDataLoaded;
        private bool m_isCompressed;
        private int m_listboxSelectedIndex;
        private int m_textBoxPos;

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

                    if (node.Type == NodeType.File)
                    {
                        VirtualFilesystemFile file = node as VirtualFilesystemFile;

                        if (file.Extension == ".bmg")
                        {
                            if (IsDataLoaded)
                                CloseCurrentFile();

                            reader = new EndianBinaryReader(file.Data, Endian.Big);

                            LoadedTextFile = new BMG(reader);

                            m_listboxSelectedIndex = 0;
                            m_loadedFileName = m_openFile.FileName;
                            WindowTitle = m_loadedFileName;

                            ColViewSource = new CollectionViewSource();
                            ColViewSource.Source = LoadedTextFile.MessageList;
                            SelectedMessage = LoadedTextFile.MessageList[0];
                            IsDataLoaded = true;

                            m_loadedDirRoot = sourceDir;
                        }

                        else if (file.Extension == ".bmc")
                        {
                            VirtualFilesystemFile bmcFile = node as VirtualFilesystemFile;

                            reader = new EndianBinaryReader(bmcFile.Data, Endian.Big);

                            LoadedColorFile = new BMC(reader);
                        }
                    }
                }
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
            /*
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
            */

            VirtualFilesystemDirectory rootDir = new VirtualFilesystemDirectory("archive");

            foreach (VirtualFilesystemNode node in m_loadedDirRoot.Children)
            {
                if (node is VirtualFilesystemFile)
                {
                    VirtualFilesystemFile oldFile = node as VirtualFilesystemFile;
                    MemoryStream newFileData = new MemoryStream();
                    EndianBinaryWriter newFileWriter = new EndianBinaryWriter(newFileData, Endian.Big);

                    if (oldFile.Extension == ".bmg")
                        LoadedTextFile.Export(newFileWriter);
                    else if (oldFile.Extension == ".bmc")
                        LoadedColorFile.Export(newFileWriter);
                    else
                    {
                        rootDir.Children.Add(oldFile);
                        continue;
                    }

                    VirtualFilesystemFile newFile = new VirtualFilesystemFile(oldFile.Name, oldFile.Extension, newFileData.ToArray());
                    rootDir.Children.Add(newFile);
                }
            }

            if (m_isCompressed)
                WArchiveTools.ArchiveUtilities.WriteArchive(path, rootDir, WArchiveTools.ArchiveCompression.Yaz0);
            else
                WArchiveTools.ArchiveUtilities.WriteArchive(path, rootDir);
        }

        private void AddMessage()
        {
            short largestID = GetHighestID();
            Message newMes = new Message(largestID, LoadedTextFile.MessageList.Count);
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
                        if (parsed[1] != "" && src.DisplayItemId != (ItemID)Convert.ToByte(parsed[1]))
                            e.Accepted = false;
                    }
                    catch (OverflowException ex)
                    {
                        SearchFilter = string.Format("itemid:{0}", 255);
                    }
                    catch (FormatException ex)
                    {
                        SearchFilter = string.Format("itemid:{0}", parsed[1].Remove(parsed[1].Length - 1));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Exception {0}!"), ex.ToString());
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
                    catch (OverflowException ex)
                    {
                        SearchFilter = string.Format("msgid:{0}", (int)(GetHighestID() - 1));
                    }
                    catch (FormatException ex)
                    {
                        SearchFilter = string.Format("msgid:{0}", parsed[1].Remove(parsed[1].Length - 1));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Exception {0}!"), ex.ToString());
                    }
                }
            }

            else if (SearchFilter.Contains("index"))
            {
                string[] parsed = SearchFilter.Split(':');

                if (parsed.Count() >= 2)
                {
                    try
                    {
                        if (parsed[1] != "" && Convert.ToInt32(src.Index) != Convert.ToInt32(parsed[1]))
                            e.Accepted = false;
                    }
                    // Something fucked up. Let's catch the exception
                    catch (OverflowException ex)
                    {
                        SearchFilter = string.Format("index:{0}", (int)(LoadedTextFile.MessageList.Count - 1));
                    }
                    catch (FormatException ex)
                    {
                        SearchFilter = string.Format("index:{0}", parsed[1].Remove(parsed[1].Length - 1));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Exception {0}!"), ex.ToString());
                    }
                }
            }

            else if (src.TextData != null && !src.TextData.Contains(SearchFilter))// here is FirstName a Property in my YourCollectionItem
                e.Accepted = false;
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
    }
}
