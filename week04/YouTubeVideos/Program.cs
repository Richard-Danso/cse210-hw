using System;

namespace YouTubeVideos
{
    public class Comment
    {
        // Properties tracking the name of the person and the comment text
        public string CommenterName { get; set; }
        public string CommentText { get; set; }

        // Constructor to easily instantiate comments
        public Comment(string name, string text)
        {
            CommenterName = name;
            CommentText = text;
        }
    }
}

namespace YouTubeVideos
{
    public class Video
    {
        // Attributes tracking title, author, and length (in seconds)
        public string Title { get; set; }
        public string Author { get; set; }
        public int Length { get; set; } // in seconds
        
        // List responsible for storing comments
        private List<Comment> _comments = new List<Comment>();

        // Constructor
        public Video(string title, string author, int length)
        {
            Title = title;
            Author = author;
            Length = length;
        }

        // Method to add a comment to this video
        public void AddComment(Comment comment)
        {
            _comments.Add(comment);
        }

        // Method that returns the total count of comments
        public int GetCommentCount()
        {
            return _comments.Count;
        }

        // Method to return the internal list for iteration
        public List<Comment> GetComments()
        {
            return _comments;
        }
    }
}

namespace YouTubeVideos
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a list to store the videos
            List<Video> videosList = new List<Video>();

            // ==========================================
            // VIDEO 1
            // ==========================================
            Video video1 = new Video("C# Basics in 10 Minutes", "CodeAcademy", 600);
            video1.AddComment(new Comment("Alice Smith", "This was incredibly helpful! Clear and concise."));
            video1.AddComment(new Comment("Bob Johnson", "I finally understand classes and objects now."));
            video1.AddComment(new Comment("Charlie Brown", "Is there a part 2 coming up soon?"));
            videosList.Add(video1);

            // ==========================================
            // VIDEO 2
            // ==========================================
            Video video2 = new Video("Top 5 White Water Rafting Spots", "AdventureSeekers", 1200);
            video2.AddComment(new Comment("Dave Miller", "The Australian rapids look intense!"));
            video2.AddComment(new Comment("Emma Watson", "Adding these to my bucket list immediately."));
            video2.AddComment(new Comment("Frank Castle", "Great camera quality. What gear did you use?"));
            videosList.Add(video2);

            // ==========================================
            // VIDEO 3
            // ==========================================
            Video video3 = new Video("How to Compress Images for Web", "WebDev 101", 450);
            video3.AddComment(new Comment("Grace Hopper", "Saved me so much bandwidth. Thanks!"));
            video3.AddComment(new Comment("Harry Potter", "My page weight dropped under 500 kB instantly."));
            video3.AddComment(new Comment("Ivy League", "Simple, straight to the point tutorial."));
            videosList.Add(video3);

            // ==========================================
            // ITERATE AND DISPLAY CONTENT
            // ==========================================
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("            YOUTUBE VIDEO TRACKING SYSTEM         ");
            Console.WriteLine("==================================================\n");

            foreach (Video video in videosList)
            {
                Console.WriteLine($"Title: {video.Title}");
                Console.WriteLine($"Author: {video.Author}");
                Console.WriteLine($"Length: {video.Length} seconds");
                Console.WriteLine($"Number of Comments: {video.GetCommentCount()}");
                Console.WriteLine("Comments:");
                
                // Nested loop to print out individual comments for this video
                foreach (Comment comment in video.GetComments())
                {
                    Console.WriteLine($"  - {comment.CommenterName}: \"{comment.CommentText}\"");
                }
                
                Console.WriteLine("\n--------------------------------------------------\n");
            }
        }
    }
}