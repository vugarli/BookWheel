using Minio.DataModel.Args;
using Minio;
using NetVips;
using System.Threading.Channels;


namespace ImageProcessor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Channel<string> _channel;
    private  IMinioClient _minioClient;
    private readonly IConfiguration configuration;
    private readonly int QUALITY;
    private readonly string Endpoint;
    
    private readonly string SRC;
    private readonly string TARGET;

    public Worker
        (
        ILogger<Worker> logger,
        Channel<string> channel,
        IConfiguration configuration,
        IMinioClient minioClient
        )
    {
        Endpoint = configuration.GetSection("MINIO")["ENDPOINT"]!;
        TARGET = configuration.GetSection("PROCESSING")["TARGET"]!;
        SRC = configuration.GetSection("PROCESSING")["SRC"]!;
        QUALITY = Int32.Parse(configuration.GetSection("PROCESSING")["QUALITY"]?.ToString() ?? "50");
        
        _logger = logger;
        _channel = channel;
        _minioClient = minioClient;
    }

    static void CreateDirectoryIfNotExist(string directoryName)
    {
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
            Console.WriteLine($"Directory '{directoryName}' created.");
        }
        else
        {
            Console.WriteLine($"Directory '{directoryName}' already exists.");
        }
    }


    public async Task DeleteImageAsync(string objectName)
    {
        try
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(SRC)
                .WithObject(objectName)).ConfigureAwait(false);

            _logger.LogInformation($"Deleted: {SRC}/{objectName}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting image: {SRC}/{objectName}");
            throw;
        }
    }




    public async Task<string> UploadImageAsync(string filePath, string objectName)
    {
        try
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var response = await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(TARGET)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType("image/jpg"))
                .ConfigureAwait(false);

            fileStream.Close();

            var objectUrl = $"http://localhost:9000/images/{objectName}";
            _logger.LogInformation($"Uploaded: {objectUrl}");
            
            return objectUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error uploading image: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DownloadImageAsync(string url, string outputPath)
    {
        using HttpClient client = new HttpClient();
        try
        {
            byte[] imageBytes = await client.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(outputPath, imageBytes).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error downloading image: {ex.Message}");
            return false;
        }
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        CreateDirectoryIfNotExist("output");
        CreateDirectoryIfNotExist("temp");

        if (Endpoint is null) throw new Exception("No endpoint given!");
        if (SRC is null) throw new Exception("No SRC given!");
        if (TARGET is null) throw new Exception("No TARGET given!");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var reader = _channel.Reader;
            var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };

            await Parallel.ForEachAsync(reader.ReadAllAsync(stoppingToken), options, async (command, token) =>
            {
                try 
                {
                    _logger.LogInformation("New task received {sometask}", command);

                    (string bucketName, string imageName) = (command.Split('/')[0],command.Split('/')[1]);
                        
                    string imageUrl = "http://" + Endpoint + "/" + command;

                    string tempFilePath = $"./temp/{Guid.NewGuid()}{imageName}";

                    string outputPath = $"./output/{Guid.NewGuid()}{imageName.Split('.')[0]}.jpeg";


                    if (!await DownloadImageAsync(imageUrl, tempFilePath))
                    {
                        _logger.LogInformation("Failed to download image.");
                        return;
                    }

                    using var image = Image.NewFromFile(tempFilePath);
                    image.Jpegsave(outputPath, QUALITY);

                    FileInfo tempFile = new FileInfo(tempFilePath);
                    FileInfo outFile = new FileInfo(outputPath);

                    if (tempFile.Attributes.HasFlag(FileAttributes.ReadOnly))
                        tempFile.Attributes -= FileAttributes.ReadOnly;
                    
                    //TODO delete temp files

                    var url = await UploadImageAsync(outputPath, imageName.Split('.')[0]+".jpeg");
                    await DeleteImageAsync(imageName);

                    _logger.LogInformation($"Image saved to {url} with quality {QUALITY}");
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex.Message);
                }
            });
        }
    }
}
