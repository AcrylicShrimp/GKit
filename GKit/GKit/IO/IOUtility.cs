﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKit {
	public static class IOUtility {
		public static FileInfo AppFileInfo {
			get {
				return new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
			}
		}


		//Move and Copy
		/// <summary>
		/// 파일 혹은 폴더를 이동합니다. 이미 있는 이름일 경우 넘버링을 붙입니다.
		/// </summary>
		public static void MoveFile(FileInfo originInfo, DirectoryInfo destInfo) {
			string origin = originInfo.FullName;
			string destDirectory = destInfo.FullName;
			if (File.Exists(origin)) {
				FileInfo fileInfo = new FileInfo(Path.Combine(destDirectory, Path.GetFileName(origin)));
				string fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
				string extension = fileInfo.Extension;

				if (File.Exists(fileInfo.FullName)) {
					int num = 2;
					while (File.Exists(Path.Combine(destDirectory, fileName + num + extension))) {
						++num;
					}
					fileInfo = new FileInfo(Path.Combine(destDirectory, fileName + num + extension));
				}

				File.Move(origin, fileInfo.FullName);
			} else if (Directory.Exists(origin)) {
				DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(destDirectory, originInfo.Name));
				if (Directory.Exists(dirInfo.FullName)) {
					int num = 2;
					while (File.Exists(Path.Combine(destDirectory, originInfo.Name + num))) {
						++num;
					}
					dirInfo = new DirectoryInfo(Path.Combine(destDirectory, originInfo.Name + num));
				}

				Directory.Move(origin, dirInfo.FullName);
			}
		}
		public static void MoveAllFiles(DirectoryInfo originInfo, DirectoryInfo destInfo) {
			try {
				destInfo.Create();

				foreach (FileInfo fileInfo in originInfo.GetFiles()) {
					try {
						string newFilePath = Path.Combine(destInfo.FullName, fileInfo.Name);
						if (File.Exists(newFilePath)) {
							File.Delete(newFilePath);
						}
						fileInfo.MoveTo(newFilePath);
					} catch(Exception ex) {
						ex.ToString().Log();
					}
				}
				foreach (DirectoryInfo subDirectory in originInfo.GetDirectories()) {
					DirectoryInfo nextDestDir = destInfo.CreateSubdirectory(subDirectory.Name);
					MoveAllFiles(subDirectory, nextDestDir);
				}
			} catch(Exception ex) {
				ex.ToString().Log();
			}
		}

		public static void CopyFile(FileInfo originInfo, DirectoryInfo destInfo) {
			string origin = originInfo.FullName;
			string destDirectory = destInfo.FullName;
			if (File.Exists(origin)) {
				FileInfo fileInfo = new FileInfo(Path.Combine(destDirectory, Path.GetFileName(origin)));
				string fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
				string extension = fileInfo.Extension;

				if (File.Exists(fileInfo.FullName)) {
					int num = 2;
					while (File.Exists(Path.Combine(destDirectory, fileName + num + extension))) {
						++num;
					}
					fileInfo = new FileInfo(Path.Combine(destDirectory, fileName + num + extension));
				}

				File.Copy(origin, fileInfo.FullName);
			}
		}
		public static void CopyFile(FileInfo originInfo, FileInfo destInfo, bool overwrite = true) {
			destInfo.Directory.Create();
			File.Copy(originInfo.FullName, destInfo.FullName, overwrite);
		}
		public static void CopyAllFiles(DirectoryInfo originInfo, DirectoryInfo destInfo) {
			foreach (DirectoryInfo dir in originInfo.GetDirectories())
				CopyAllFiles(dir, destInfo.CreateSubdirectory(dir.Name));
			foreach (FileInfo file in originInfo.GetFiles())
				file.CopyTo(Path.Combine(destInfo.FullName, file.Name));
		}

		public static ImageFileFormat GetImageFormat(Stream stream) {
			var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
			var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
			var png = new byte[] { 137, 80, 78, 71 };    // PNG
			var tiff = new byte[] { 73, 73, 42 };         // TIFF
			var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
			var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
			var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

			var buffer = new byte[4];
			stream.Read(buffer, 0, buffer.Length);

			if (bmp.SequenceEqual(buffer.Take(bmp.Length)))
				return ImageFileFormat.bmp;

			if (gif.SequenceEqual(buffer.Take(gif.Length)))
				return ImageFileFormat.gif;

			if (png.SequenceEqual(buffer.Take(png.Length)))
				return ImageFileFormat.png;

			if (tiff.SequenceEqual(buffer.Take(tiff.Length)))
				return ImageFileFormat.tiff;

			if (tiff2.SequenceEqual(buffer.Take(tiff2.Length)))
				return ImageFileFormat.tiff;

			if (jpeg.SequenceEqual(buffer.Take(jpeg.Length)))
				return ImageFileFormat.jpg;

			if (jpeg2.SequenceEqual(buffer.Take(jpeg2.Length)))
				return ImageFileFormat.jpg;

			return ImageFileFormat.unknown;
		}

		public static void SaveText(this string text, string filePath) {
			FileInfo fileInfo = new FileInfo(filePath);
			if (!fileInfo.Directory.Exists) {
				fileInfo.Directory.Create();
			}
			using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
				using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8)) {
					writer.Write(text);
				}
			}
		}
		public static string LoadText(this string filePath) {
			using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
				using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8)) {
					return reader.ReadToEnd();
				}
			}
		}
	}
}