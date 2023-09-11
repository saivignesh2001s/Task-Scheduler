using System.Threading;

namespace Samp
{
    public class Worker: BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly TimeSpan _period=TimeSpan.FromSeconds(5);

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public override  Task StartAsync(CancellationToken stoppingToken)
        {
            using(FileStream fs=new FileStream("file1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using(StreamWriter writer=new StreamWriter(fs))
                {
                 writer.WriteLineAsync("Starting");
                }
            }
            return base.StartAsync(stoppingToken);
     

        }

      

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (!(stoppingToken.IsCancellationRequested) && await timer.WaitForNextTickAsync(stoppingToken))
            {
                //await Task.Delay(5000);
                if(stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                string text=DateTime.Now.ToString()+" "+"\n";
                File.AppendAllText("file1.txt",text);
                _logger.LogInformation(DateTime.Now.ToString());
            }
        }
        public override  Task StopAsync(CancellationToken cancellationToken)
        {
            string text ="ending";
            File.AppendAllText("file1.txt", text);
            return base.StopAsync(cancellationToken);
           
        }
    }
}