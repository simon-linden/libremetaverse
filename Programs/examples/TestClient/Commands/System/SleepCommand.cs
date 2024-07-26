using System;
using OpenMetaverse.Packets;

namespace OpenMetaverse.TestClient
{
    public class SleepCommand : Command
    {
        uint sleepSerialNum = 1;

        public SleepCommand(TestClient testClient)
        {
            Name = "sleep";
            Description = "Sleeps for a given number of seconds, and sends AgentPause/AgentResume messages to region. Usage: sleep <seconds> [nopause]";
            Category = CommandCategory.TestClient;
        }

        public override string Execute(string[] args, UUID fromAgentID)
        {
            int seconds;
            if (args.Length < 1 || args.Length > 2 || !int.TryParse(args[0], out seconds))
                return "Usage: sleep [seconds] <nopause>";

            // optional argument to not send pause/resume packets to region
            bool send_pause = true;
            if (args.Length == 2 && args[1] == "nopause")
                send_pause = false;

            if (send_pause)
            {
                AgentPausePacket pause = new AgentPausePacket
                {
                    AgentData =
                    {
                        AgentID = Client.Self.AgentID,
                        SessionID = Client.Self.SessionID,
                        SerialNum = sleepSerialNum++
                    }
                };

                Client.Network.SendPacket(pause);

                // Sleep
                System.Threading.Thread.Sleep(seconds * 1000);

                AgentResumePacket resume = new AgentResumePacket
                {
                    AgentData =
                    {
                        AgentID = Client.Self.AgentID,
                        SessionID = Client.Self.SessionID,
                        SerialNum = pause.AgentData.SerialNum
                    }
                };

                Client.Network.SendPacket(resume);
                return $"Paused region updates, slept thread for {seconds} second(s) and resumed";
            }

            // send_pause == false, just sleep
            System.Threading.Thread.Sleep(seconds * 1000);
            return $"Slept thread for {seconds} second(s)";
        }
    }
}
