using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using WindEditor;

namespace WindWakerTextEditor.View
{
    public class ColorEditor_ViewModel : INotifyPropertyChanged
    {
        private BMC m_colorFile;
        private WLinearColor m_selectedColor;

        public BMC ColorFile
        {
            get { return m_colorFile; }
            set
            {
                if (m_colorFile != value)
                {
                    m_colorFile = value;
                    NotifyPropertyChanged("ColorFile");
                }
            }
        }

        public WLinearColor SelectedColor
        {
            get { return m_selectedColor; }
            set
            {
                if (m_selectedColor != value)
                {
                    m_selectedColor = value;
                    NotifyPropertyChanged("SelectedColor");
                    Console.WriteLine("color");
                }
            }
        }

        public void SetFile(BMC file)
        {
            ColorFile = file;
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
