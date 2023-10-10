namespace OpenMetaverse.TestClient
{
    public class JumpCommand: Command
    {
        public JumpCommand(TestClient testClient)
		{
			Name = "jump";
			Description = "Jumps or flies up.  Add 'stop' to finish";
            Category = CommandCategory.Movement;
		}

        public override string Execute(string[] args, UUID fromAgentID)
		{
            bool start = !(args.Length == 1 && args[0].ToLower() == "stop");

            if (start)
            {
                Client.Self.Jump(true);
                return "Started jumping";
            }
            else
            {
                Client.Self.Jump(false);
                return "Stopped jumping";
            }
		}
    }
}
