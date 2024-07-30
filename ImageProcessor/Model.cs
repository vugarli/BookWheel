using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ImageProcessor
{
    public class S3Event
    {
        [JsonPropertyName("EventName")]
        public string EventName { get; set; }

        [JsonPropertyName("Key")]
        public string Key { get; set; }

        [JsonPropertyName("Records")]
        public List<S3Record> Records { get; set; }
    }

    public class S3Record
    {
        [JsonPropertyName("eventVersion")]
        public string EventVersion { get; set; }

        [JsonPropertyName("eventSource")]
        public string EventSource { get; set; }

        [JsonPropertyName("awsRegion")]
        public string AwsRegion { get; set; }

        [JsonPropertyName("eventTime")]
        public string EventTime { get; set; }

        [JsonPropertyName("eventName")]
        public string EventName { get; set; }

        [JsonPropertyName("userIdentity")]
        public UserIdentity UserIdentity { get; set; }

        [JsonPropertyName("requestParameters")]
        public RequestParameters RequestParameters { get; set; }

        [JsonPropertyName("responseElements")]
        public ResponseElements ResponseElements { get; set; }

        [JsonPropertyName("s3")]
        public S3 S3 { get; set; }

        [JsonPropertyName("source")]
        public Source Source { get; set; }
    }

    public class UserIdentity
    {
        [JsonPropertyName("principalId")]
        public string PrincipalId { get; set; }
    }

    public class RequestParameters
    {
        [JsonPropertyName("principalId")]
        public string PrincipalId { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("sourceIPAddress")]
        public string SourceIPAddress { get; set; }
    }

    public class ResponseElements
    {
        [JsonPropertyName("x-amz-id-2")]
        public string XAmzId2 { get; set; }

        [JsonPropertyName("x-amz-request-id")]
        public string XAmzRequestId { get; set; }

        [JsonPropertyName("x-minio-deployment-id")]
        public string XMinioDeploymentId { get; set; }

        [JsonPropertyName("x-minio-origin-endpoint")]
        public string XMinioOriginEndpoint { get; set; }
    }

    public class S3
    {
        [JsonPropertyName("s3SchemaVersion")]
        public string S3SchemaVersion { get; set; }

        [JsonPropertyName("configurationId")]
        public string ConfigurationId { get; set; }

        [JsonPropertyName("bucket")]
        public Bucket Bucket { get; set; }

        [JsonPropertyName("object")]
        public S3Object Object { get; set; }
    }

    public class Bucket
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ownerIdentity")]
        public OwnerIdentity OwnerIdentity { get; set; }

        [JsonPropertyName("arn")]
        public string Arn { get; set; }
    }

    public class OwnerIdentity
    {
        [JsonPropertyName("principalId")]
        public string PrincipalId { get; set; }
    }

    public class S3Object
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("eTag")]
        public string ETag { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("userMetadata")]
        public UserMetadata UserMetadata { get; set; }

        [JsonPropertyName("sequencer")]
        public string Sequencer { get; set; }
    }

    public class UserMetadata
    {
        [JsonPropertyName("content-type")]
        public string ContentType { get; set; }
    }

    public class Source
    {
        [JsonPropertyName("host")]
        public string Host { get; set; }

        [JsonPropertyName("port")]
        public string Port { get; set; }

        [JsonPropertyName("userAgent")]
        public string UserAgent { get; set; }
    }

}
