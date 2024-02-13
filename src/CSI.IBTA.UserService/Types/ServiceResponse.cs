namespace CSI.IBTA.UserService.Types
{
    public class ServiceResponse <T>
    {
        public T value { get; set; }
        public string description { get; set; } 

        public ServiceResponse(T value, string description)
        {
            this.value = value;
            this.description = description;
        }
    }
}
