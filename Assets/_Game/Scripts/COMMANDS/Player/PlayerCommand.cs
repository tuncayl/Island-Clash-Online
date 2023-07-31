using onlinetutorial.interfaces;

namespace COMMANDS.Playercommands
{
    public abstract class PlayerCommand: ICommand
    {
        public abstract void execute();

    }
}