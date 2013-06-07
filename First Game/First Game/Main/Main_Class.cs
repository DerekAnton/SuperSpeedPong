using System;

namespace First_Game
{
#if WINDOWS || XBOX
    static class Main_Class
    {
        // The Main Entry Point For The Application //
        static void Main(string[] args)
        {
            using (System game = new System())
            {
                game.Run();
            }
        }
    }
#endif
}
