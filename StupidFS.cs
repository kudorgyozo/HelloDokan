using DokanNet;
using DokanNet.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using FileAccess = DokanNet.FileAccess;

namespace HelloDokan {

    public class StupidFS : IDokanOperations {
        private const string theFileName = @"hello.txt";
        private const string theFilePath = @$"\{theFileName}";
        private const string FileContent = "CradftCode Crew 4 ever";
        private readonly ILogger logger;

        public StupidFS(ILogger logger)
        {
            this.logger = logger;
        }

        public void Cleanup(string fileName, IDokanFileInfo info) {
        }

        public void CloseFile(string fileName, IDokanFileInfo info) {
        }

        private NtStatus Trace(string method, string fileName, FileAccess? access, FileMode? mode, FileAttributes? attributes, IDokanFileInfo info, NtStatus result) {
            logger.Info($"[{method}] nam:{fileName} acc:{access} mod:{mode} att:{attributes} dir:{info.IsDirectory} -> {result}");
            return result;
        }

        public NtStatus CreateFile(string fileName, FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, IDokanFileInfo info) {

            if (info.IsDirectory) {
                if (fileName == "\\") {
                    info.IsDirectory = true;
                    return Trace(nameof(CreateFile), fileName, access, mode, attributes, info, DokanResult.Success);
                } else {
                    info.IsDirectory = false;
                    return Trace(nameof(CreateFile), fileName, access, mode, attributes, info, DokanResult.NotADirectory);
                }
            } else if (fileName.Equals(theFilePath)) {
                info.IsDirectory = false;
                info.Context = new MemoryStream(Encoding.ASCII.GetBytes(FileContent));
                return Trace(nameof(CreateFile), fileName, access, mode, attributes, info, DokanResult.Success);
                
            } else {
                if (fileName == "\\") {
                    info.IsDirectory = true;
                    return Trace(nameof(CreateFile), fileName, access, mode, attributes, info, DokanResult.Success);
                }
                
                return Trace(nameof(CreateFile), fileName, access, mode, attributes, info, DokanResult.FileNotFound);
            }

            //return DokanResult.Success;
        }

        public NtStatus DeleteDirectory(string fileName, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus DeleteFile(string fileName, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus FindFiles(string fileName, out IList<FileInformation> files, IDokanFileInfo info) {
            //logger.Info("FindFiles f:{0} d:{1}", fileName, info.IsDirectory);

            var fileInfo = new FileInformation {
                FileName = theFileName,
                Attributes = 0,
                CreationTime = DateTime.Now,
                LastAccessTime = DateTime.Now,
                LastWriteTime = DateTime.Now,
                Length = FileContent.Length * 8,
            };

            files = new List<FileInformation> { fileInfo };

            return Trace(nameof(FindFiles), fileName, null, null, null, info, DokanResult.Success);
            return DokanResult.Success;
        }

        public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, IDokanFileInfo info) {
            files = null;

            return DokanResult.NotImplemented;
        }

        public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus FlushFileBuffers(string fileName, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info) {
            freeBytesAvailable = 512 * 1024 * 1024;
            totalNumberOfBytes = 1024 * 1024 * 1024;
            totalNumberOfFreeBytes = 512 * 1024 * 1024;

            return DokanResult.Success;
        }

        public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info) {
            //logger.Info("GetFileInformation f:{0} d:{1}", fileName, info.IsDirectory);

            if (info.IsDirectory) {
                fileInfo = new FileInformation {
                    FileName = fileName,
                    Attributes = FileAttributes.Directory,
                    CreationTime = DateTime.Now,
                    LastAccessTime = DateTime.Now,
                    LastWriteTime = DateTime.Now,
                    Length = 0,
                };
                return DokanResult.Success;
            } else {
                fileInfo = new FileInformation {
                    FileName = theFilePath,
                    Attributes = 0,
                    CreationTime = DateTime.Now,
                    LastAccessTime = DateTime.Now,
                    LastWriteTime = DateTime.Now,
                    Length = FileContent.Length,
                };

                return DokanResult.Success;

            }

        }

        public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info) {
            security = null;
            return DokanResult.NotImplemented;
        }

        public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info) {
            volumeLabel = "CraftCode Crew";
            features = FileSystemFeatures.ReadOnlyVolume;
            fileSystemName = "CCCFS";
            maximumComponentLength = 256;


            return DokanResult.Success;
        }

        public NtStatus LockFile(string fileName, long offset, long length, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus Mounted(string mountPoint, IDokanFileInfo info) {
            return DokanResult.Success;
        }

        public NtStatus MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info) {
            //logger.Info("ReadFile f:{0} i:{1}", fileName, info.IsDirectory);
            bytesRead = 0;

            if (info.Context is MemoryStream stream) {
                stream.Position = offset;
                bytesRead = stream.Read(buffer, 0, buffer.Length);
            }

            return DokanResult.Success;
        }

        public NtStatus SetAllocationSize(string fileName, long length, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus SetEndOfFile(string fileName, long length, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus UnlockFile(string fileName, long offset, long length, IDokanFileInfo info) {
            throw new NotImplementedException();
        }

        public NtStatus Unmounted(IDokanFileInfo info) {
            return DokanResult.Success;
        }

        public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info) {
            throw new NotImplementedException();
        }
    }
}
