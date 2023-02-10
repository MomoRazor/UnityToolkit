public class PID
{
    private float _pFactor, _iFactor, _dFactor;

    private float _integral;
    private float _lastError;


    public PID(float pFactor, float iFactor, float dFactor)
    {
        this.pFactor = pFactor;
        this.iFactor = iFactor;
        this.dFactor = dFactor;
    }


    public float Update(float current, float target, float timeFrame)
    {
        float error = current - target;
        _integral += error * timeFrame;
        float deriv = (error - _lastError) / timeFrame;
        _lastError = error;
        return error * _pFactor + _integral * _iFactor + deriv * _dFactor;
    }
}
