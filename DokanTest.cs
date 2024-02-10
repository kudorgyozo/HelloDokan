using DokanNet;
using DokanNet.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using FileAccess = DokanNet.FileAccess;
using static DokanNet.FormatProviders;
using System.Runtime.InteropServices;

namespace HelloDokan {
    public class DokanTest : IDokanOperations {

        private const FileAccess DataAccess = FileAccess.ReadData | FileAccess.WriteData | FileAccess.AppendData |
                                      FileAccess.Execute |
                                      FileAccess.GenericExecute | FileAccess.GenericWrite |
                                      FileAccess.GenericRead;

        private const FileAccess DataWriteAccess = FileAccess.WriteData | FileAccess.AppendData |
                                                   FileAccess.Delete |
                                                   FileAccess.GenericWrite;

        private readonly ILogger logger;

        public DokanTest(ILogger logger) {
            this.logger = logger;
        }

        protected NtStatus Trace(string method, string fileName, IDokanFileInfo info, NtStatus result,
            params object[] parameters) {
#if TRACE
            var extraParameters = parameters != null && parameters.Length > 0
                ? ", " + string.Join(", ", parameters.Select(x => string.Format(DefaultFormatProvider, "{0}", x)))
                : string.Empty;

            logger.Debug(DokanFormat($"{method}('{fileName}', {info}{extraParameters}) -> {result}"));
#endif

            return result;
        }

        private NtStatus Trace(string method, string fileName, IDokanFileInfo info,
            FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes,
            NtStatus result) {
#if TRACE
            logger.Debug(
                DokanFormat(
                    $"{method}('{fileName}', {info}, [{access}], [{share}], [{mode}], [{options}], [{attributes}]) -> {result}"));
#endif

            return result;
        }



        public void Cleanup(string fileName, IDokanFileInfo info) {
            //throw new NotImplementedException();
        }

        public void CloseFile(string fileName, IDokanFileInfo info) {
            //throw new NotImplementedException();
        }

        public NtStatus CreateFile(string fileName, FileAccess access, FileShare share, FileMode mode,
            FileOptions options, FileAttributes attributes, IDokanFileInfo info) {
            var result = DokanResult.Success;
            //var filePath = GetPath(fileName);

            if (info.IsDirectory) {
                try {
                    switch (mode) {
                        case FileMode.Open:
                            return DokanResult.NotADirectory;

                        //if (!Directory.Exists(filePath)) {
                        //    try {
                        //        if (!File.GetAttributes(filePath).HasFlag(FileAttributes.Directory))
                        //            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                        //                attributes, DokanResult.NotADirectory);
                        //    } catch (Exception) {
                        //        return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                        //            attributes, DokanResult.FileNotFound);
                        //    }
                        //    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                        //        attributes, DokanResult.PathNotFound);
                        //}

                        //new DirectoryInfo(filePath).EnumerateFileSystemInfos().Any();
                        //// you can't list the directory
                        //break;

                        case FileMode.CreateNew:
                            return DokanResult.FileExists;

                            //if (Directory.Exists(filePath))
                            //    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                            //        attributes, DokanResult.FileExists);

                            //try {
                            //    File.GetAttributes(filePath).HasFlag(FileAttributes.Directory);
                            //    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                            //        attributes, DokanResult.AlreadyExists);
                            //} catch (IOException) {
                            //}

                            //Directory.CreateDirectory(GetPath(fileName));
                            //break;
                    }
                } catch (UnauthorizedAccessException) {
                    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                        DokanResult.AccessDenied);
                }
            } else {
                //var pathExists = true;
                //var pathIsDirectory = false;

                //var readWriteAttributes = (access & DataAccess) == 0;
                //var readAccess = (access & DataWriteAccess) == 0;

                //pathExists = (fileName == "hello.txt");
                //pathIsDirectory = fileName == @"\\";


                //switch (mode) {
                //    case FileMode.Open:

                //        if (pathExists) {
                //            // check if driver only wants to read attributes, security info, or open directory
                //            if (readWriteAttributes || pathIsDirectory) {
                //                if (pathIsDirectory && (access & FileAccess.Delete) == FileAccess.Delete
                //                    && (access & FileAccess.Synchronize) != FileAccess.Synchronize)
                //                    //It is a DeleteFile request on a directory
                //                    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                //                        attributes, DokanResult.AccessDenied);

                //                info.IsDirectory = pathIsDirectory;
                //                info.Context = new object();
                //                // must set it to something if you return DokanError.Success

                //                return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                //                    attributes, DokanResult.Success);
                //            }
                //        } else {
                //            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                //                DokanResult.FileNotFound);
                //        }
                //        break;

                //    case FileMode.CreateNew:
                //        if (pathExists)
                //            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                //                DokanResult.FileExists);
                //        break;

                //    case FileMode.Truncate:
                //        if (!pathExists)
                //            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                //                DokanResult.FileNotFound);
                //        break;
                //}

                //try {
                //    System.IO.FileAccess streamAccess = readAccess ? System.IO.FileAccess.Read : System.IO.FileAccess.ReadWrite;

                //    if (mode == System.IO.FileMode.CreateNew && readAccess) streamAccess = System.IO.FileAccess.ReadWrite;

                //    info.Context = new FileStream(filePath, mode,
                //        streamAccess, share, 4096, options);

                //    if (pathExists && (mode == FileMode.OpenOrCreate
                //                       || mode == FileMode.Create))
                //        result = DokanResult.AlreadyExists;

                //    bool fileCreated = mode == FileMode.CreateNew || mode == FileMode.Create || (!pathExists && mode == FileMode.OpenOrCreate);
                //    if (fileCreated) {
                //        FileAttributes new_attributes = attributes;
                //        new_attributes |= FileAttributes.Archive; // Files are always created as Archive
                //        // FILE_ATTRIBUTE_NORMAL is override if any other attribute is set.
                //        new_attributes &= ~FileAttributes.Normal;
                //        File.SetAttributes(filePath, new_attributes);
                //    }
                //} catch (UnauthorizedAccessException) // don't have access rights
                //  {
                //    if (info.Context is FileStream fileStream) {
                //        // returning AccessDenied cleanup and close won't be called,
                //        // so we have to take care of the stream now
                //        fileStream.Dispose();
                //        info.Context = null;
                //    }
                //    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                //        DokanResult.AccessDenied);
                //} catch (DirectoryNotFoundException) {
                //    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                //        DokanResult.PathNotFound);
                //} catch (Exception ex) {
                //    var hr = (uint)Marshal.GetHRForException(ex);
                //    switch (hr) {
                //        case 0x80070020: //Sharing violation
                //            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                //                DokanResult.SharingViolation);
                //        default:
                //            throw;
                //    }
                //}
            }

            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                result);
        }


        public NtStatus DeleteDirectory(string fileName, IDokanFileInfo info) {
            //throw new NotImplementedException();
            return NtStatus.NotImplemented;
        }

        public NtStatus DeleteFile(string fileName, IDokanFileInfo info) {
            //throw new NotImplementedException();
            return NtStatus.NotImplemented;
        }

        public NtStatus FindFiles(string fileName, out IList<FileInformation> files, IDokanFileInfo info) {
            if (info.IsDirectory && fileName != "hello.txt") {
                files = new List<FileInformation>();
                return NtStatus.Success;
            }

            var bytes = Encoding.UTF8.GetBytes("Hello Dokan");

            //throw new NotImplementedException();
            files = new List<FileInformation>() {
                new FileInformation {
                    Length = bytes.Length,
                    FileName = "hello.txt",
                    Attributes = FileAttributes.Normal,
                    CreationTime = new DateTime(2024, 01, 01),
                    LastAccessTime = new DateTime(2024, 01, 01),
                    LastWriteTime = new DateTime(2024, 01, 01),
                }

            };
            return NtStatus.Success;
        }

        public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus FlushFileBuffers(string fileName, IDokanFileInfo info) {
            return NtStatus.Success;
        }

        public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info) {
            freeBytesAvailable = 0;
            totalNumberOfBytes = 0;
            totalNumberOfFreeBytes = 0;
            return NtStatus.Success;
        }

        public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info) {
            if (info.IsDirectory || fileName != "hello.txt") {
                fileInfo = new FileInformation();
                return NtStatus.NoSuchFile;
            }

            fileInfo = new FileInformation() {
                FileName = "hello.txt",
                Attributes = FileAttributes.None,
                CreationTime = new DateTime(2024, 01, 01),
                LastAccessTime = new DateTime(2024, 01, 01),
                LastWriteTime = new DateTime(2024, 01, 01),
            };
            return NtStatus.Success;
        }

        public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info) {
            security = new FileSecurity(fileName, sections);
            return NtStatus.NotImplemented;
        }

        public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info) {
            volumeLabel = "test";
            features = FileSystemFeatures.ReadOnlyVolume;
            fileSystemName = "passz";
            maximumComponentLength = 50;
            return NtStatus.Success;
            //throw new NotImplementedException();
        }

        public NtStatus LockFile(string fileName, long offset, long length, IDokanFileInfo info) {
            return NtStatus.Success;
            //throw new NotImplementedException();
        }

        public NtStatus Mounted(string mountPoint, IDokanFileInfo info) {
            return NtStatus.Success;
            //throw new NotImplementedException();
        }

        public NtStatus MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info) {
            if (info.IsDirectory || fileName != "hello.txt") {
                bytesRead = 0;
                return NtStatus.NoSuchFile;
            }

            using var ms = new MemoryStream(buffer);
            using var sw = new BinaryWriter(ms);

            var bytes = Encoding.UTF8.GetBytes("Hello Dokan");
            sw.Write(bytes);
            bytesRead = bytes.Length;

            return NtStatus.Success;
        }

        public NtStatus SetAllocationSize(string fileName, long length, IDokanFileInfo info) {
            return NtStatus.Success;
        }

        public NtStatus SetEndOfFile(string fileName, long length, IDokanFileInfo info) {
            return NtStatus.Success;
            //throw new NotImplementedException();
        }

        public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, IDokanFileInfo info) {
            return NtStatus.Success;
            //throw new NotImplementedException();
        }

        public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info) {
            return NtStatus.Success;
            //throw new NotImplementedException();
        }

        public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info) {
            return NtStatus.Success;
            //throw new NotImplementedException();
        }

        public NtStatus UnlockFile(string fileName, long offset, long length, IDokanFileInfo info) {
            return NtStatus.Success;
            //throw new NotImplementedException();
        }

        public NtStatus Unmounted(IDokanFileInfo info) {
            return NtStatus.Success;
            //throw new NotImplementedException();
        }

        public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info) {
            if (info.IsDirectory || fileName != "hello.txt") {
                bytesWritten = 0;
                return NtStatus.NoSuchFile;
            }

            bytesWritten = 0;
            return NtStatus.Success;
            //throw new NotImplementedException();
        }
    }
}
