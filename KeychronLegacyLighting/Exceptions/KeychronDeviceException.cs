namespace KeychronLegacyLighting.Exceptions
{
    public class KeychronDeviceException : Exception
    {
        public KeychronDeviceException(string? message) : base(message)
        {
        }

        public KeychronDeviceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
