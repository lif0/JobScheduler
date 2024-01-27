﻿using FabulousScheduler.Core.Types;
using FabulousScheduler.Cron.Interfaces;
using FabulousScheduler.Cron;
using FabulousScheduler.Cron.Result;

var config = new Config(
    maxParallelJobExecute: 5,
    sleepAfterCheck: TimeSpan.FromMilliseconds(100)
);
CronJobManager.SetConfig(config);

// Register callback for job's result
CronJobManager.JobResultEvent += (ref ICronJob job, ref JobResult<JobOk, JobFail> res) =>
{
    var now = DateTime.Now;
    if (res.IsSuccess)
        Console.WriteLine("[{0:hh:mm:ss}] {1} {2} IsSuccess", now, job.Name, res.ID);
    else
        Console.WriteLine("[{0:hh:mm:ss}] {1} {2} IsFail", now, job.Name, res.ID);
};

// Register a job
CronJobManager.Register(
    action: () =>
    {
        //do some work
        int a = 10;
        int b = 100;
        int c = a + b;
        
        return Task.CompletedTask;
    },
    sleepDuration: TimeSpan.FromSeconds(1),
    name: "ExampleJob"
);

// Start a job scheduler
CronJobManager.RunScheduler();

/*
 * The job will fall asleep for 1 second after success completion, then it will wake up and will be push the job pool
*/

Thread.Sleep(-1);