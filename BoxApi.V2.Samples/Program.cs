﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BoxApi.V2;
using BoxApi.V2.Authentication.Legacy;
using BoxApi.V2.Model;
using BoxApi.V2.Model.Enum;
using Microsoft.Win32;

namespace BoxApi.V2.Samples
{
    public class Program
    {
        //
        // NOTE - The api_key and auth_token need to be changed from the ones below
        //
        private static void Main(string[] args)
        {
            ExecuteAPIMethods("YOUR CLIENT ID", "YOUR CLIENT SECRET", "YOUR ACCESS TOKEN", "YOUR REFRESH TOKEN");
        }

        private static void ExecuteAPIMethods(string clientId, string clientSecret, string accessToken, string refreshToken)
        {
            // Instantiate a BoxManager with your api key and a user's auth token
            var boxManager = new BoxManager(clientId, clientSecret, accessToken, refreshToken);

            // Get all contents of the root folder
            // Specify that we only want the Name, Etag, Size, CreatedAt properties returned for the folder contents.
            var rootFolder = boxManager.GetFolder(Folder.Root, new[]{Field.Name, Field.Etag, Field.Size, Field.CreatedAt });

            // Find a file
            var file = rootFolder.Files.Single(f => f.Name.Equals("my file.txt"));

            // Change the file's name and description
            file.Name = "the new name.txt";
            file.Description = "the new description";

            // Update the file
            // A new file object is always returned with an updated ETag.
            // Specify that we only want the Name, Etag, and Size of the file returned.
            file = boxManager.Update(file);

            // Create a new subfolder
            var subfolder = boxManager.CreateFolder(Folder.Root, "my subfolder");

            // Move the file to the subfolder
            file = boxManager.Move(file, subfolder);

            // Write some content to the file
            file = boxManager.Write(file, new byte[] { 1, 2, 3 });

            // Read the contents to a stream
            using (var stream = new MemoryStream())
            {
                boxManager.Read(file, stream);
            }

            // Delete the file
            boxManager.Delete(file);
        }
    }
}