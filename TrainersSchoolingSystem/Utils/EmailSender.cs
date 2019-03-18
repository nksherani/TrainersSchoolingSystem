using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;

namespace TrainersSchoolingSystem.Utils
{
    public static class EmailSender
    {
        public static bool Send(string path)
        {
            try
            {
                if (!Compress(path))
                    return false;
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("trainers.system@gmail.com");
                mail.To.Add("nksherani@outlook.com");
                mail.Subject = "DB Backup";
                mail.Body = "DB Backup attachment";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(path+".gz");
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("trainers.system@gmail.com", "NED12345");
                SmtpServer.Credentials = new System.Net.NetworkCredential("trainers.system@gmail.com", "NED12345");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {

                Logger.Fatal(ex.Message);
                Logger.Fatal(ex.Source);
                Logger.Fatal(ex.TargetSite.Name);
                Logger.Fatal(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Logger.Fatal(ex.InnerException.Message);
                    Logger.Fatal(ex.InnerException.Source);
                    Logger.Fatal(ex.InnerException.TargetSite.Name);
                    Logger.Fatal(ex.InnerException.StackTrace);
                }
                return false;
            }
        }
        public static bool Compress(string path)
        {
            FileInfo fi = new FileInfo(path);
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((File.GetAttributes(fi.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    // Create the compressed file.
                    using (FileStream outFile =
                                File.Create(fi.FullName + ".gz"))
                    {
                        using (GZipStream Compress =
                            new GZipStream(outFile,
                            CompressionMode.Compress))
                        {
                            // Copy the source file into 
                            // the compression stream.
                            inFile.CopyTo(Compress);

                            Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                                fi.Name, fi.Length.ToString(), outFile.Length.ToString());
                        }
                    }
                }
            }
            return true;
        }
    }
}