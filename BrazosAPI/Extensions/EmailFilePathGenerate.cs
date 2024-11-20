namespace BrazosAPI.Extensions
{
      public class EmailFilePathGenerate
      {
            private readonly IWebHostEnvironment _webHostEnvironment;
            public EmailFilePathGenerate(IWebHostEnvironment webHostEnvironment)
            {
                  _webHostEnvironment = webHostEnvironment;
            }
            public string EmailFilePath(string FileLocation)
            {
                  var emailcontent = "";
                  string webrootpath = _webHostEnvironment.WebRootPath;
                  var filePath = Path.Combine(webrootpath, FileLocation);
                  using (var file = new StreamReader(filePath))
                  {
                        emailcontent = file.ReadToEnd();
                        file.Close();
                  }
                  return emailcontent;
            }
      }
}
