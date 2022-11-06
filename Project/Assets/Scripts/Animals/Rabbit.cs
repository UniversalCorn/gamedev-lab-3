
public class Rabbit : Animal
{
    protected override float AdditionalMoveSpeed()
    {
        return _isRunning ? 2 : 1;
    }
}
