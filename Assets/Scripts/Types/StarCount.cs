internal static class StarCount
{
   public static int NumberOfStars(float timeRemaining)
    {
        if (timeRemaining > 40)
        {
            return 3;
        }
        else if (timeRemaining > 20)
        {
            return 2;
        }
        else if (timeRemaining > 0)
        {
            return 1;
        }

        return 0;
    }

}
