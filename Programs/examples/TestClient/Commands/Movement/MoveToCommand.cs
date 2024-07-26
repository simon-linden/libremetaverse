using System;

namespace OpenMetaverse.TestClient.Commands.Movement
{
    class MovetoCommand : Command
    {
        public MovetoCommand(TestClient client)
        {
            Name = "moveto";
            Description = "Moves the avatar to the specified local region position using simulator autopilot. Usage: moveto x y z";
            Category = CommandCategory.Movement;
        }

        public override string Execute(string[] args, UUID fromAgentID)
        {
            if (args.Length != 3)
                return "Usage: moveto x y z";

            uint regionX, regionY;
            Utils.LongToUInts(Client.Network.CurrentSim.Handle, out regionX, out regionY);

            double local_x, local_y, z;
            if (!double.TryParse(args[0], out local_x) ||
                !double.TryParse(args[1], out local_y) ||
                !double.TryParse(args[2], out z))
            {
                return "Usage: moveto x y z";
            }

            // Convert the local coordinates to global ones by adding the region handle parts to x and y
            double x = local_x + (double)regionX;
            double y = local_y + (double)regionY;

            Client.Self.AutoPilot(x, y, z);

            return $"Attempting to move to global <{x},{y},{z}>  region <{local_x},{local_y},{z}>";
        }
    }
}
