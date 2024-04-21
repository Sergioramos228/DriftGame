using System.Collections.Generic;

public class FinishDriftCoefficients
{
    private const int Unplaceable = 0;
    private const int FirstPlace = 1;
    private const int SecondPlace = 2;
    private const int ThirdPlace = 3;
    private const int FourthPlace = 4;
    private Dictionary<int, float> _placeCoefficients;

    public FinishDriftCoefficients()
    {
        _placeCoefficients = new Dictionary<int, float>()
        {
            {Unplaceable, 0},
            {FirstPlace, 2 },
            {SecondPlace, 1 },
            {ThirdPlace, 1 },
            {FourthPlace, 0.5f }
        };
    }

    public float GetCoefficient(int place)
    {
        if (_placeCoefficients.ContainsKey(place))
            return _placeCoefficients[place];
        else
            return 0;
    }
}
