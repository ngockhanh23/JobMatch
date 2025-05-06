using JobMatch.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace JobMatch.Services
{
    public class ResumeService
    {
        private readonly JobMatchContext _dbContext;

        public ResumeService(JobMatchContext dbContext)
        {
            _dbContext = dbContext;
        }

		public Resume? UploadResume(IFormFile resumeFile, int userId)
		{
			if (resumeFile != null && resumeFile.Length > 0)
			{
				using (var memoryStream = new MemoryStream())
				{
					resumeFile.CopyTo(memoryStream);					
					string resumeTitle = Path.GetFileNameWithoutExtension(resumeFile.FileName);

					var resume = new Resume
					{
						CandidateId = userId,
						ResumeTitle = resumeTitle,
						ResumeFile = memoryStream.ToArray()
					};

					_dbContext.Resumes.Add(resume);
					_dbContext.SaveChanges();

					return resume; // Trả về đối tượng Resume vừa tạo
				}
			}
			return null; // Nếu upload thất bại
		}



		public (byte[] fileData, string fileName, string contentType)? DownloadResume(int resumeId)
        {
            var resume = _dbContext.Resumes.Find(resumeId);
            if (resume != null && resume.ResumeFile != null)
            {
                return (resume.ResumeFile, $"{resume.ResumeTitle}.pdf", "application/pdf");
            }
            return null;
        }
    }
}
