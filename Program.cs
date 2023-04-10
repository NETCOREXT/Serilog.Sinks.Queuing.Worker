using System.Reflection;
using Microsoft.Extensions.Options;
using Serilog.Sinks.Queuing.Redis.ElasticHook.Extensions;
using Serilog.Sinks.Queuing.Redis.Extensions;
using Serilog.Sinks.Queuing.Worker;

var builder = WebApplication.CreateBuilder(args);

builder.Host
       .ConfigureAppConfiguration((w, s) =>
                                  {
                                      var host = w.HostingEnvironment;

                                      s.SetBasePath(host.ContentRootPath)
                                       .AddJsonFile("appsettings.json", false, true)
                                       .AddJsonFile($"appsettings.{host.EnvironmentName}.json", true, true)
                                       .AddJsonFile($"appsettings.override.json", true, true)
                                       .AddEnvironmentVariables();
                                  })
       .ConfigureLogging(loggingBuilder =>
                         {
                             loggingBuilder.ClearProviders();
                         });

builder.Services.Configure<ConfigSettings>(builder.Configuration);

builder.Services.AddElasticLogStore((provider, options) =>
                                    {
                                        var config = provider.GetRequiredService<IOptions<ConfigSettings>>().Value;

                                        var host = Environment.GetEnvironmentVariable("ELASTICSEARCH_HOST") ?? config.ElasticsearchHost;
                                        var apiKey = Environment.GetEnvironmentVariable("ELASTICSEARCH_APIKEY") ?? config.ElasticsearchApiKey;
                                        var index = Environment.GetEnvironmentVariable("ELASTICSEARCH_INDEX") ?? config.ElasticsearchIndex;
                                        var indexFormatPattern = Environment.GetEnvironmentVariable("ELASTICSEARCH_INDEX_FORMAT_PATTERN") ?? config.ElasticsearchIndexFormatPattern;

                                        options.ElasticsearchHost = host;
                                        options.ApiKey = apiKey;
                                        options.Index = index;
                                        options.IndexFormatPattern = indexFormatPattern ?? "";
                                    });

builder.Services.AddRedisQueuingSink((provider, options) =>
                                     {
                                         var config = provider.GetRequiredService<IOptions<ConfigSettings>>().Value;

                                         var host = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ?? config.RedisConnectionString;
                                         var notificationChannel = Environment.GetEnvironmentVariable("REDIS_NOTIFICATION_CHANNEL") ?? config.RedisNotificationChannel;
                                         var streamKey = Environment.GetEnvironmentVariable("REDIS_STREAM_KEY") ?? config.RedisStreamKey;
                                         var streamGroup = Environment.GetEnvironmentVariable("REDIS_STREAM_GROUP") ?? config.RedisStreamGroup;

                                         if (string.IsNullOrWhiteSpace(streamGroup))
                                             streamGroup = Assembly.GetEntryAssembly()!.GetName().Name!;
                                         
                                         var machineName = Environment.GetEnvironmentVariable("REDIS_MACHINE_NAME") ?? config.RedisMachineName;

                                         if (string.IsNullOrWhiteSpace(machineName))
                                             machineName = Environment.GetEnvironmentVariable("HOSTNAME") ?? Environment.MachineName;
                                         
                                         var streamBatchSize = int.TryParse(Environment.GetEnvironmentVariable("REDIS_STREAM_BATCH_SIZE"), out var bs)
                                                                   ? bs
                                                                   : config.RedisStreamBatchSize;

                                         var workerTaskLimit = int.TryParse(Environment.GetEnvironmentVariable("WORKER_TASK_LIMIT"), out var tl)
                                                                   ? tl
                                                                   : config.WorkerTaskLimit;

                                         var retryLimit = int.TryParse(Environment.GetEnvironmentVariable("RETRY_LIMIT"), out var rl)
                                                              ? rl
                                                              : config.RetryLimit;

                                         options.RedisConnectionString = host;
                                         options.NotificationChannel = notificationChannel ?? streamKey;
                                         options.StreamKey = streamKey;
                                         options.StreamGroup = streamGroup;
                                         options.MachineName = machineName;
                                         options.StreamBatchSize = streamBatchSize;
                                         options.WorkerTaskLimit = workerTaskLimit;
                                         options.RetryLimit = retryLimit;
                                         options.EnableWorker = true;
                                     });

var app = builder.Build();

app.Run();