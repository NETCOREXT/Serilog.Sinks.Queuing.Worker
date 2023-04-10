# Serilog.Sinks.Queuing.Worker

```json
{
  "ElasticsearchHost": "http://0.0.0.0:9200",
  "ElasticsearchApiKey": "",
  "ElasticsearchIndex": "serilog-",
  "ElasticsearchIndexFormatPattern": "yyyyMMdd",

  "RedisConnectionString": "0.0.0.0:6379",
  "RedisStreamKey": "serilog",
  "RedisNotificationChannel": "serilog",
  "RedisStreamGroup": "serilog",
  "RedisMachineName": "serilog",
  "RedisStreamBatchSize": 100,
  "RedisStreamIdleTime": 5000,
  "WorkerTaskLimit": 5,
  "RetryLimit": 3
}
```
