namespace instock_server_application.AwsS3.Models; 

public class S3Model {
    public static readonly string S3BucketName = "instock-photos";
    
    public string Name { get; set; } = null!;
    public MemoryStream InputStream { get; set; } = null!;
    public string BucketName { get; set; } = null!;

    public S3Model(string name, MemoryStream inputStream, string bucketName) {
        Name = name;
        InputStream = inputStream;
        BucketName = bucketName;
    }

    public S3Model(string name, MemoryStream inputStream) {
        Name = name;
        InputStream = inputStream;
    }
}