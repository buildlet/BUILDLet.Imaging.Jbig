/***************************************************************************************************
The MIT License (MIT)

Copyright 2021 Daiki Sakamoto

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, 
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or 
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
***************************************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BUILDLet.Imaging.Jbig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;  // for Bitmap
using System.IO;
using System.Security.Cryptography;  // for MD5

namespace BUILDLet.Imaging.Jbig.Tests
{
    [TestClass]
    public class JbigImageTests
    {
        [DataTestMethod]
        [DataRow("ccitt1", "B8-F8-38-66-1C-3C-92-67-F5-FA-C9-8E-FF-C9-A3-5B")]
        [DataRow("ccitt2", "80-BA-8E-61-CC-91-93-84-D2-8A-3A-77-28-7D-68-C2")]
        [DataRow("ccitt3", "9E-8B-99-77-7B-59-24-F3-D6-16-9A-7C-98-77-17-30")]
        [DataRow("ccitt4", "62-FC-7C-90-30-06-90-24-F6-C0-13-DF-11-F9-EB-E7")]
        [DataRow("ccitt5", "DE-03-C2-1E-77-E0-C1-B8-16-1F-25-B8-8E-55-8E-7B")]
        [DataRow("ccitt6", "A6-B4-A0-B9-C5-B4-AE-43-8E-86-2E-92-39-A9-4F-EE")]
        [DataRow("ccitt7", "03-49-E3-DA-8C-F6-F1-91-0C-43-35-E7-B9-D4-9D-40")]
        [DataRow("ccitt8", "EE-02-3A-4F-38-43-20-59-FF-1F-A8-C6-E6-4C-1A-CF")]
        public void ToBitmapTest(string filename, string filehash)
        {
            // ARRANGE: Remove File
            File.Delete($"{filename}.bmp");

            // ACT
            var bitmap = JbigImage.ToBitmap($@"TestFiles\{filename}.jbg");
            
            // Save as Bitmap File
            bitmap.Save($"{filename}.bmp");

            // ASSERT
            Assert.AreEqual(filehash, BitConverter.ToString(MD5.Create().ComputeHash(File.OpenRead($"{filename}.bmp"))));
        }


        [DataTestMethod]
        [DataRow("ccitt1", "B8-F8-38-66-1C-3C-92-67-F5-FA-C9-8E-FF-C9-A3-5B")]
        [DataRow("ccitt2", "80-BA-8E-61-CC-91-93-84-D2-8A-3A-77-28-7D-68-C2")]
        [DataRow("ccitt3", "9E-8B-99-77-7B-59-24-F3-D6-16-9A-7C-98-77-17-30")]
        [DataRow("ccitt4", "62-FC-7C-90-30-06-90-24-F6-C0-13-DF-11-F9-EB-E7")]
        [DataRow("ccitt5", "DE-03-C2-1E-77-E0-C1-B8-16-1F-25-B8-8E-55-8E-7B")]
        [DataRow("ccitt6", "A6-B4-A0-B9-C5-B4-AE-43-8E-86-2E-92-39-A9-4F-EE")]
        [DataRow("ccitt7", "03-49-E3-DA-8C-F6-F1-91-0C-43-35-E7-B9-D4-9D-40")]
        [DataRow("ccitt8", "EE-02-3A-4F-38-43-20-59-FF-1F-A8-C6-E6-4C-1A-CF")]
        public void ToBitmapTest2(string filename, string filehash)
        {
            // ARRANGE

            // Remove File
            File.Delete($"{filename}a.bmp");

            // Read Bytes from file
            var bytes = File.ReadAllBytes($@"TestFiles\{filename}.jbg");

            // ACT
            var bitmap = JbigImage.ToBitmap(bytes);

            // Save as Bitmap File
            bitmap.Save($"{filename}a.bmp");

            // ASSERT
            Assert.AreEqual(filehash, BitConverter.ToString(MD5.Create().ComputeHash(File.OpenRead($"{filename}.bmp"))));
        }
    }
}