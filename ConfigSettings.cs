using System.Reflection;

namespace Serilog.Sinks.Queuing.Worker;

public class ConfigSettings
{
    public const int DEFAULT_STREAM_IDLE_TIME = 5 * 1000;
    public const int DEFAULT_STREAM_BATCH_SIZE = 100;
    public const int DEFAULT_WORKER_TASK_LIMIT = 5;
    public const int DEFAULT_RETRY_LIMIT = 3;

    public string ElasticsearchHost { get; set; } = "http://0.0.0.0:9200";
    public string? ElasticsearchApiKey { get; set; }
    public string ElasticsearchIndex { get; set; } = "serilog-";
    public string? ElasticsearchIndexFormatPattern { get; set; } = "yyyyMMdd";


    public string RedisConnectionString { get; set; } = "0.0.0.0:6379";
    public string RedisStreamKey { get; set; } = "serilog";
    public string RedisNotificationChannel { get; set; } = "serilog";
    public string RedisStreamGroup { get; set; } = Assembly.GetEntryAssembly()!.GetName().Name!;
    public string RedisMachineName { get; set; } = Environment.GetEnvironmentVariable("HOSTNAME") ?? Environment.MachineName;
    public int? RedisStreamBatchSize { get; set; } = DEFAULT_STREAM_BATCH_SIZE;
    public int? RedisStreamIdleTime { get; set; } = DEFAULT_STREAM_IDLE_TIME;

    public int? WorkerTaskLimit { get; set; } = DEFAULT_WORKER_TASK_LIMIT;
    public int? RetryLimit { get; set; } = DEFAULT_RETRY_LIMIT;
}