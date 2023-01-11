﻿using Catfish.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class AudioRecorderField : BaseField
    {
        public static string FileContainerTag = "files";
        public XmlModelList<FileReference> Files { get; set; }
        //public string[] AllowedExtensions 
        //{ 
        //    get => GetAttribute("extensions", new string[0]); 
        //    set => SetAttribute("extensions", value); 
        //}
        //public int MaxFileSize
        //{
        //    get => GetAttribute("maxFileSize", 0);
        //    set => SetAttribute("maxFileSize", value);
        //}

        public AudioRecorderField() { DisplayLabel = "Audio Recorder Field"; }
        public AudioRecorderField(XElement data) : base(data) { DisplayLabel = "Audio Recorder Field"; }
        public AudioRecorderField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Audio Recorder Field"; }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            XmlModel xml = new XmlModel(Data);
            Files = new XmlModelList<FileReference>(xml.GetElement(FileContainerTag, true), true, FileReference.TagName);
        }

        public override void UpdateValues(BaseField srcField)
        {
            AudioRecorderField src = srcField as AudioRecorderField;

            //Removing existing file references
            Files.Clear();

            //Inserting the new file references
            foreach (var file in src.Files)
            {
                Files.Add(new FileReference(new XElement(file.Data)));

                //If the file is in the temporary folder, move it to the attachment-files folder
                string tmpFile = Path.Combine(ConfigHelper.GetUploadTempFolder(false), file.FileName);
                if(File.Exists(tmpFile))
                {
                    string finalFile = Path.Combine(ConfigHelper.GetAttachmentsFolder(true), file.FileName);
                    File.Move(tmpFile, finalFile);
                }
            }
        }

        /// <summary>
        /// This method has no meaning for the Attachment class.
        /// </summary>
        /// <param name="value"></param>
        public override void SetValue(string value, string lang) { }

        /// <summary>
        /// This method has no meaning for the Attachment class.
        /// </summary>
        /// <param name="srcField"></param>
        public override void CopyValue(BaseField srcField, bool overwrite = false) { }

        public string FileNames { get; set; }

        public string Format
        {
            get => GetAttribute("format", "");
            set => SetAttribute("format", value);
        }


    }
}