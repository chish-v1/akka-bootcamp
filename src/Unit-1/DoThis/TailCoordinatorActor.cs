﻿using Akka.Actor;
using System;

namespace WinTail
{

    public class TailCoordinatorActor : UntypedActor
    {
        #region Message types
        public class StartTail
        {
            /// <summary>
            /// Start tailing the file at user-specified path.
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="reporterActor"></param>
            public StartTail(string filePath, IActorRef reporterActor)
            {
                FilePath = filePath;
                ReporterActor = reporterActor;
            }

            public string FilePath { get; private set; }

            public IActorRef ReporterActor { get; private set; }
        }

        /// <summary>
        /// Stop tailing the file at user-specified path.
        /// </summary>
        public class StopTail
        {
            public string FilePath { get; private set; }

            public StopTail(string filePath)
            {
                FilePath = filePath;
            }
        }

        #endregion
        protected override void OnReceive(object message)
        {
            if (message is StartTail startTail)
            {
                //here we are creating out first parent/child relationship!
                // the TailActor instance created here is a chile
                // of this instance of TailCoordinatorActor
                Context.ActorOf(Props.Create(() => new TailActor(startTail.ReporterActor, startTail.FilePath))); 
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                10, // maxNumberOfRetries
                TimeSpan.FromSeconds(30), // withinTimeRange
                x => // localOnlyDecider
                {
                    //Maybe we consider ArithmeticException to not be application critical
                    //so we just ignore the error and keep going.
                    if (x is ArithmeticException) return Directive.Resume;

                    //Error that we cannot recover from, stop the failing actor
                    else if (x is NotSupportedException) return Directive.Stop;

                    //In all other cases, just restart the failing actor
                    else return Directive.Restart;
                });
        }
    }
}