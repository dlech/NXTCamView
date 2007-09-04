//
//    Copyright 2007 Paul Tingey
//
//    This file is part of NXTCamView.
//
//    NXTCamView is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 3 of the License, or
//    (at your option) any later version.
//
//    Foobar is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
using System.Windows.Forms;

namespace NXTCamView.StripCommands
{
    public class SaveFileStripCommand : StripCommand
    {
        private bool _isSaveAs;
        private SaveFileDialog _saveFileDialog;

        public SaveFileStripCommand(SaveFileDialog saveFileDialog, bool isSaveAs)
        {
            _saveFileDialog = saveFileDialog;
            _isSaveAs = isSaveAs;
        }

        public override bool CanExecute()
        {
            CaptureForm form = MainForm.Instance.ActiveMdiChild as CaptureForm;
            return form != null;
        }

        public override bool Execute()
        {
            CaptureForm form = MainForm.Instance.ActiveMdiChild as CaptureForm;
            if( form == null ) return false;
            if( form.Filename == "" || _isSaveAs )
            {
                _saveFileDialog.FileName = form.Filename != "" ? form.Filename : form.Text;
                if (_saveFileDialog.ShowDialog() != DialogResult.OK) return false;
                form.Filename = _saveFileDialog.FileName;
            }
            form.SaveFile(form.Filename);
            return true;
        }

        public override bool HasExecuted()
        {
            return false;
        }
    }
}