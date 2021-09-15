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
using System;
using System.Diagnostics;  // for Process
using System.IO;           // for Stream
using System.Drawing;      // for Bitmap

namespace BUILDLet.Imaging.Jbig
{
    /// <summary>
    /// JBIG1 画像ファイルを .net で扱うためのクラスを実装します。
    /// </summary>
    public static class JbigImage
    {
        /// <summary>
        /// デフォルトのバッファサイズ
        /// </summary>
        public static int DefaultBufferSize { get; } = 5 * 1000 * 1000;


        /// <summary>
        /// 最大のバッファサイズ
        /// </summary>
        public static int MaxBufferSize { get; } = 1000 * 1000 * 1000;


        /// <summary>
        /// JBIG 画像ファイルを読み込んで <see cref="System.Drawing.Bitmap"/> オブジェクトに変換します。
        /// </summary>
        /// <param name="filepath">
        /// 読み込む JBIG 画像ファイルのパス
        /// </param>
        /// <returns>
        /// JBIG 画像ファイルから変換された <see cref="Bitmap"/> オブジェクト
        /// </returns>
        /// <remarks>
        /// 次のコマンドと同等の処理を行います。
        /// <code>
        /// jbigtopnm.exe input-file | ppmtobmp.exe > output-file
        /// </code>
        /// </remarks>
        public static Bitmap ToBitmap(string filepath) => ToBitmap(filepath, JbigImage.DefaultBufferSize);


        /// <summary>
        /// バイト配列を JBIG 画像として読み込んで <see cref="System.Drawing.Bitmap"/> オブジェクトに変換します。
        /// </summary>
        /// <param name="bytes">
        /// JBIG 画像として読み込むバイト配列
        /// </param>
        /// <returns>
        /// バイト配列から変換された <see cref="Bitmap"/> オブジェクト
        /// </returns>
        /// <remarks>
        /// 次のコマンドと同等の処理を行います。
        /// <code>
        /// standard-input-stream > jbigtopnm.exe - | ppmtobmp.exe > output-file
        /// </code>
        /// </remarks>
        public static Bitmap ToBitmap(byte[] bytes) => ToBitmap(bytes, JbigImage.DefaultBufferSize);


        /// <summary>
        /// <inheritdoc cref="JbigImage.ToBitmap(string)"/>
        /// </summary>
        /// <param name="filepath">
        /// <inheritdoc cref="JbigImage.ToBitmap(string)"/>
        /// </param>
        /// <param name="bufferSize">
        /// jbigtopnm.exe および ppmtobmp.exe の標準出力をリダイレクトする際に使用するバッファのサイズ
        /// </param>
        /// <returns>
        /// <inheritdoc cref="JbigImage.ToBitmap(string)"/>
        /// </returns>
        /// <remarks>
        /// <inheritdoc cref="JbigImage.ToBitmap(string)"/>
        /// </remarks>
        public static Bitmap ToBitmap(string filepath, int bufferSize)
        {
            // Validation (File Exsistence)
            if (!File.Exists(filepath))
            {
                // ERROR
                throw new FileNotFoundException("File is not found.", filepath);
            }

            // RETURN
            return JbigImage.ConvertToBitmap(filepath, bufferSize);
        }


        /// <summary>
        /// <inheritdoc cref="ToBitmap(byte[])"/>
        /// </summary>
        /// <param name="bytes">
        /// <inheritdoc cref="ToBitmap(byte[])"/>
        /// </param>
        /// <param name="bufferSize">
        /// <inheritdoc cref="ToBitmap(string, int)"/>
        /// </param>
        /// <returns>
        /// <inheritdoc cref="ToBitmap(byte[])"/>
        /// </returns>
        /// <remarks>
        /// <inheritdoc cref="ToBitmap(byte[])"/>
        /// </remarks>
        public static Bitmap ToBitmap(byte[] bytes, int bufferSize) => JbigImage.ConvertToBitmap("-", bufferSize, bytes);


        private static Bitmap ConvertToBitmap(string inputFileName, int bufferSize, byte[] standardInput = null)
        {
            // Flag of Redirecting Standard Input Stream of "jbigtopnm.exe"
            var redirectStandardInput = inputFileName.CompareTo("-") == 0;

            // Validation (Standard Input Stream Redirect)
            if (redirectStandardInput && (standardInput == null))
            {
                // ERROR
                throw new ArgumentException($"{nameof(standardInput)} is null, even though {nameof(inputFileName)} is \"-\".", nameof(standardInput));
            }

            // Validation (Buffer Size)
            if (bufferSize > JbigImage.MaxBufferSize)
            {
                // ERROR
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            // jbigtopnm.exe & ppmtobmp.exe
            var jbigtopnm_filename = "jbigtopnm.exe";
            var ppmtobmp_filename = "ppmtobmp.exe";

            // New Buffer(s)
            var buffer = new byte[bufferSize];


            // for jbigtopnm.exe
            using (Process jbigtopnm = new())
            {
                // Set StartInfo to jbigtopnm.exe
                jbigtopnm.StartInfo = new ProcessStartInfo
                {
                    FileName = jbigtopnm_filename,
                    Arguments = inputFileName,
                    RedirectStandardInput = redirectStandardInput,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                // START jbigtopnm.exe
                jbigtopnm.Start();


                // Redirecting Standard Input Stream of "jbigtopnm.exe"
                if (redirectStandardInput)
                {
                    // Write to Standard Input of ppmtobmp.exe
                    using (BinaryWriter writer = new(jbigtopnm.StandardInput.BaseStream))
                    {
                        writer.Write(standardInput);
                    }
                }


                // Read from Standard Output of jbigtopnm.exe
                using (BinaryReader reader = new(jbigtopnm.StandardOutput.BaseStream))
                {
                    buffer = reader.ReadBytes(bufferSize);
                }


                // Wait for EXIT jbigtopnm.exe
                jbigtopnm.WaitForExit();

                // Check Exit Code
                if (jbigtopnm.ExitCode != 0)
                {
                    // ERROR
                    throw new InvalidDataException($"Non Zero Exit code ({jbigtopnm.ExitCode}) is returned from {jbigtopnm_filename}.");
                }
            }
            // for jbgtopnm.exe


            // for ppmtobmp.exe
            using (Process ppmtobmp = new())
            {
                // Set StartInfo to jbigtopnm.exe
                ppmtobmp.StartInfo = new ProcessStartInfo
                {
                    FileName = ppmtobmp_filename,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                };

                // START ppmtobmp.exe
                ppmtobmp.Start();


                // Write to Standard Input of ppmtobmp.exe
                using (BinaryWriter writer = new(ppmtobmp.StandardInput.BaseStream))
                {
                    writer.Write(buffer);
                }


                // Read from Standard Output of ppmtobmp.exe
                using (BinaryReader reader = new(ppmtobmp.StandardOutput.BaseStream))
                {
                    buffer = reader.ReadBytes(bufferSize);
                }


                // Wait for EXIT ppmtobmp.exe 
                ppmtobmp.WaitForExit();

                // Check Exit Code
                if (ppmtobmp.ExitCode != 0)
                {
                    // ERROR
                    throw new InvalidDataException($"Non Zero Exit code ({ppmtobmp.ExitCode}) is returned from {ppmtobmp_filename}.");
                }
            }
            // for ppmtobmp.exe


            // Return Bitmap
            return new Bitmap(new MemoryStream(buffer));
        }
    }
}
