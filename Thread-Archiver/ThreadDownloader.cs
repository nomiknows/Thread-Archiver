using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Web;
using System.Windows.Forms;
using System.IO;
namespace Thread_Archiver
{
    /// <summary>
    /// This class provides methods to download entire 4chan threads. 
    /// Create the object with the thread URL and call DownloadImages.
    /// This class makes use of Newtonsoft's JSON library, which you will need for compilation and linking.
    /// You can find this library at: http://james.newtonking.com/json
    /// </summary>
    public class ThreadDownloader
    {
        // 4Chan CDN Names
        private const string CDN_IMG = "https://i.4cdn.org";
        private const string CDN_THR = "https://a.4cdn.org";

        // Illegal characters in Windows Filenames
        private static char[] ILLEGAL_CHARS = Path.GetInvalidFileNameChars();

        private string threadURL;
        public Thread t;

        /// <summary>
        /// Creates a ThreadDownloaer object.
        /// </summary>
        /// <param name="url">URL to the thread.</param>
        public ThreadDownloader(string url)
        {
            threadURL = url;
        }

        private string filterFilename(string filename)
        {
            foreach (char c in ILLEGAL_CHARS)
            {
                filename = filename.Replace(c.ToString(), "");
            }
            return filename;
        }
        /// <summary>
        /// Downloads all of the images in a thread.
        /// </summary>
        /// <param name="directory">Directory to save downloaded images to.</param>
        public void DownloadImages(string directory)
        {
            try 
            {
                // Get rid of junk
                if (threadURL.Contains("http://"))
                    threadURL = threadURL.Replace("http://", "");

                // Find info
                string board = threadURL.Split('/')[1];
                string threadNum = threadURL.Split('/')[3];

                // Download JSON string and deserialize it
                WebClient wb = new WebClient();
                string jsonData = wb.DownloadString(CDN_THR + "/" + board + "/thread/" + threadNum + ".json");
                Thread thread = JsonConvert.DeserializeObject<Thread>(jsonData);

                // Save images with original filename
                foreach (Post p in thread.Posts)
                {
                    // Download the original file
                    if (p.filename != null && !File.Exists(directory + p.filename + p.ext))
                    {
                        
                        wb.DownloadFile(CDN_IMG + "/" + board + "/" + p.tim + p.ext, directory + filterFilename(p.filename + p.ext));
                    }
                }

                // Offer a notification
                MessageBox.Show("Download Complete!", "You Got Images!");
            } 
            catch (Exception e)
            {
                MessageBox.Show("There was an error m8. You're link was probably bad if it's a 404.\n" + e.Message + "\n" + e.StackTrace, "Something went horribly wrong.");
                return;
            }
        }
    }

    /// <summary>
    /// This class provides a way to group posts from the same thread together.
    /// </summary>
    public class Thread
    {
        public IList<Post> Posts;
    }

    /// <summary>
    /// This class provides a way to store any kind of post made in a thread based on the JSON data from 4chan.
    /// For more information regarding 4chans JSON data, refer to: https://github.com/4chan/4chan-API
    /// </summary>
    public class Post
    {
        public int no;
        public int resto;
        public int sticky;
        public int closed;
        public string now;
        public int time;
        public string name;
        public string trip;
        public string id;
        public string capcode;
        public string country;
        public string country_name;
        public string email;
        public string sub;
        public string com;
        public Int64 tim;
        public string filename;
        public string ext;
        public int fsize;
        public string md5;
        public int w;
        public int h;
        public int tn_w;
        public int tn_h;
        public int filedeleted;
        public int spoiler;
        public int custom_spoiler;
        public int ommitted_posts;
        public int omitted_images;
        public int replies;
        public int images;
        public int bumplimit;
        public int imagelimit;
        // capcode_replies - not included but available
        public int last_modified;
        public string tag;
        public string semantic_url;
    }
}
