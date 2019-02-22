﻿#region (c)2008-2015 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2015 Hawkynt

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Diagnostics.Contracts;
using System.Drawing;

using Imager;

namespace Classes.ScriptActions {
  internal class SaveFileCommand : IScriptAction {
    #region Implementation of IScriptAction
    public bool ChangesSourceImage { get { return (false); } }

    public bool ChangesTargetImage { get { return (false); } }
    public bool ProvidesNewGdiSource { get { return (false); } }

    public bool Execute() {
      var result = CLI.SaveHelper(this._fileName, this.TargetImage.ToBitmap());
      if (result == CLIExitCode.NothingToSave)
        throw new NullReferenceException("Nothing to save");
      if (result == CLIExitCode.JpegNotSupportedOnThisPlatform)
        throw new InvalidOperationException("Jpeg not supported");

      return (result == CLIExitCode.OK);
    }

    public Bitmap GdiSource { get { return (null); } }

    public cImage SourceImage { get; set; }

    public cImage TargetImage { get; set; }
    #endregion

    private readonly string _fileName;

    public string FileName { get { return (this._fileName); } }

    public SaveFileCommand(string fileName) {
      Contract.Requires(!string.IsNullOrWhiteSpace(fileName));
      this._fileName = fileName;
    }
  }
}
