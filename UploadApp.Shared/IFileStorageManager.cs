﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadApp.Shared
{
    public interface IFileStorageManager
    {
        /// <summary>
        /// File(Blob) Upload
        /// </summary>
        /// <returns>New FileName</returns>
        Task<string> UploadAsync(byte[] bytes, string fileName, string folderPath, bool overwrite);
        Task<string> UploadAsync(Stream stream, string fileName, string folderPath, bool overwrite);

        /// <summary>
        /// File(Blob) Download
        /// </summary>
        /// <returns>File(Blob)</returns>
        Task<byte[]> DownloadAsync(string fileName, string folderPath);

        /// <summary>
        /// File(Blob) Delete
        /// </summary>
        /// <returns>true or false</returns>
        Task<bool> DeleteAsync(string fileName, string folderPath);

        /// <summary>
        /// Get Sub Folder with string
        /// </summary>
        string GetFolderPath(string ownerType, string ownerId, string fileType);

        /// <summary>
        /// Get Sub Folder with long
        /// </summary>
        string GetFolderPath(string ownerType, long ownerId, string fileType);

        /// <summary>
        /// Get Sub Folder with int
        /// </summary>
        string GetFolderPath(string ownerType, int ownerId, string fileType);
    }
}
